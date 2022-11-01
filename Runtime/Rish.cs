using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace RishUI
{
    public delegate void RefAction<T>(ref T value) where T : struct;

    // TODO: Split into partial classes
    public static partial class Rish
    {
        private static Stack<IOwner> Owners { get; } = new();
        
        private const int InitialPoolSize = 64;
        private static Dictionary<Type, Stack<ElementDefinition>> Pools { get; } = new();

        private static uint _nextElementId;
        private static uint DefinitionId
        {
            get
            {
                if (_nextElementId == uint.MaxValue)
                {
                    _nextElementId = 0;
                }
                _nextElementId += 1;

                return _nextElementId;
            }
        }
        private static Dictionary<uint, ElementDefinition> DefinitionsInUse { get; } = new();
        
        private static uint _nextChildrenId;
        private static uint ChildrenId
        {
            get
            {
                if (_nextChildrenId == uint.MaxValue)
                {
                    _nextChildrenId = 0;
                }
                _nextChildrenId += 1;

                return _nextChildrenId;
            }
        }
        private static Dictionary<uint, NativeArray<Element>> ArraysInUse { get; } = new();
        
#if UNITY_EDITOR
        static Rish()
        {
            ShowWarnings();
        }

        private static void ShowWarnings()
        {
            var playerAssemblies = UnityEditor.Compilation.CompilationPipeline.GetAssemblies(UnityEditor.Compilation.AssembliesType.PlayerWithoutTestAssemblies).Select(asm => asm.name).ToArray();
            var types = AppDomain.CurrentDomain.GetAssemblies().Where(ShouldIncludeAssembly).SelectMany(asm => asm.GetTypes()).Where(type => {
                var baseType = type.BaseType;
                if (baseType is not { IsGenericType: true })
                {
                    return false;
                }
                
                var genericBaseType = baseType.GetGenericTypeDefinition();
                return genericBaseType == typeof(RishElement<>) || genericBaseType == typeof(RishElement<,>);
            }).SelectMany(type =>
            {
                var baseType = type.BaseType;
                var genericArguments = baseType.GenericTypeArguments;
                return genericArguments.Length switch
                {
                    1 => new[] { genericArguments[0] },
                    2 => new[] { genericArguments[0], genericArguments[1] },
                    _ => throw new UnityException("Invalid type")
                };
            }).Where(type => !type.IsGenericParameter).ToArray();
            
            ShowComparersWarnings(types);
            ShowCopyWarnings(types);
            
            bool ShouldIncludeAssembly(System.Reflection.Assembly asm)
            {
                if (asm.IsDynamic)
                {
                    return true;
                }

                var name = asm.GetName().Name;
                return !name.Contains("Unity") && playerAssemblies.Contains(name);
            }
        }

        private static void ShowComparersWarnings(Type[] types)
        {
            foreach (var type in types)
            {
                if (Comparers.Contains(type))
                {
                    continue;
                }
                
                if (!UnsafeUtility.IsUnmanaged(type))
                {
                    Debug.LogWarning($"{GetTypeFullName(type)} is managed and needs a Comparer");
                }
                else
                {
                    var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    if (fields.Any(field => field.FieldType == typeof(Element) || field.FieldType == typeof(Children)))
                    {
                        Debug.LogWarning($"{GetTypeFullName(type)} has at least one Element or Children field and needs a Comparer");
                    }
                }
            }
        }

        private static void ShowCopyWarnings(Type[] types)
        {
            foreach (var type in types)
            {
                if (Copiers.Contains(type))
                {
                    continue;
                }

                var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (fields.Any(field => field.FieldType == typeof(Element) || field.FieldType == typeof(Children)))
                {
                    Debug.LogWarning($"{GetTypeFullName(type)} has at least one Element or Children field and needs a Copier");
                }
            }
        }

        private static string GetTypeFullName(Type type) => (type.IsGenericType ? type.GetGenericTypeDefinition() : type).FullName;
#endif
        
        private static T GetFromPool<T>() where T : ElementDefinition, new()
        {
            var type = typeof(T);
            if (!Pools.TryGetValue(type, out var pool))
            {
                pool = new Stack<ElementDefinition>(InitialPoolSize);
                Pools[type] = pool;
            }

            T element;
            if (pool.Count < 1)
            {
                element = new T();
            }
            else
            {
                element = (T)pool.Pop();
            }
            
            return element;
        }

        internal static void ReturnToPool(uint id)
        {
            if (!DefinitionsInUse.TryGetValue(id, out var definition))
            {
                return;
            }
            
            var type = definition.GetType();
            if (!Pools.TryGetValue(type, out var pool))
            {
                throw new UnityException($"{type} is an invalid element type. No pool found.");
            }
            
            pool.Push(definition);
            DefinitionsInUse.Remove(id);
        }

        internal static void Dispose(uint id)
        {
            if (!ArraysInUse.TryGetValue(id, out var children))
            {
                return;
            }
            
            children.Dispose();
            ArraysInUse.Remove(id);
        }

        internal static ElementDefinition GetDefinition(uint id) => DefinitionsInUse.TryGetValue(id, out var children) ? children : null;
        internal static NativeArray<Element> GetNativeArray(uint id) => ArraysInUse.TryGetValue(id, out var children) ? children : default;

        internal static void RegisterOwner(IOwner listener)
        {
            Owners.Push(listener);
        }

        internal static void UnregisterOwner(IOwner listener)
        {
            if (Owners.Peek() != listener)
            {
                throw new UnityException("You're not the current subscriber. This should never happen.");
            }
            
            Owners.Pop();
        }

        private static Element CreateElement(ElementDefinition definition)
        {
            var id = DefinitionId;
            DefinitionsInUse[id] = definition;
            var element = new Element(id);
            OnCreate(element);

            return element;
        }
        private static void OnCreate(Element element)
        {
            var owner = Owners.Peek();
            if (owner == null)
            {
                throw new UnityException("There's nobody to claim ownership of this ElementDefinition");
            }

            owner.TakeOwnership(element);
        }

        private static Children CreateChildren(NativeArray<Element> array)
        {
            var id = ChildrenId;
            ArraysInUse[id] = array;
            var children = new Children(id);
            OnCreate(children);

            return children;
        }
        private static void OnCreate(Children children)
        {
            var owner = Owners.Peek();
            if (owner == null)
            {
                throw new UnityException("There's nobody to claim ownership of this NativeArray<Element>");
            }
            
            owner.TakeOwnership(children);
        }

        public static Element Container(Children? children)
        {
            var element = GetFromPool<ContainerDefinition>();
            element.Factory(children ?? RishUI.Children.Empty);
            
            return CreateElement(element);
        }

        private class ContainerDefinition : ElementDefinition
        {
            private Children Children { get; set; }
            
            public void Factory(Children children)
            {
                Children = children;
            }

            public override Element New(Descriptor descriptor) => Rish.Container(Children.Copy());

            public override void Invoke(Node node)
            {
                node.SetChildren(Children);
            }

            public override bool Equals(ElementDefinition other)
            {
                return other is ContainerDefinition otherDefinition && RishUtils.Compare<Children>(Children, otherDefinition.Children);
            }
        }
        
        public static T RefProps<T>(RefAction<T> func) where T : struct => RefProps(Defaults.GetValue<T>(), func);
        public static T RefProps<T>(T d, RefAction<T> func) where T : struct
        {
            func?.Invoke(ref d);
                
            return d;
        }
    }
}
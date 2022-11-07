using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace RishUI
{
    public delegate void RefAction<T>(ref T value) where T : struct;

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
        
#if UNITY_EDITOR
        static Rish()
        {
            ShowWarnings();
        }
         
        private static Type[] _playerTypes;
        internal static Type[] PlayerTypes
        {
            get
            {
                if (_playerTypes == null)
                {
                    var playerAssemblies = UnityEditor.Compilation.CompilationPipeline.GetAssemblies(UnityEditor.Compilation.AssembliesType.PlayerWithoutTestAssemblies).Select(asm => asm.name).ToArray();
                    _playerTypes = AppDomain.CurrentDomain.GetAssemblies().Where(ShouldIncludeAssembly).SelectMany(asm => asm.GetTypes()).ToArray();
                    
                    bool ShouldIncludeAssembly(Assembly asm)
                    {
                        if (asm.IsDynamic)
                        {
                            return true;
                        }

                        var name = asm.GetName().Name;
                        return !name.Contains("Unity") && playerAssemblies.Contains(name);
                    }
                }

                return _playerTypes;
            }
        }

        private static void ShowWarnings()
        {
            var types = PlayerTypes.Where(type => {
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
                    if (fields.Any(field => field.FieldType == typeof(Element)))
                    {
                        Debug.LogWarning($"{GetTypeFullName(type)} has at least one Element field and needs a Comparer");
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
                if (fields.Any(field => field.FieldType == typeof(Element)))
                {
                    Debug.LogWarning($"{GetTypeFullName(type)} has at least one Element field and needs a Copier");
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

            definition.Dispose();
            pool.Push(definition);
            DefinitionsInUse.Remove(id);
        }

        internal static ElementDefinition GetDefinition(uint id) => DefinitionsInUse.TryGetValue(id, out var definition) ? definition : null;

        internal static int GetLength(uint id)
        {
            var definition = GetDefinition(id);
            if (definition == null)
            {
                return 0;
            }

            if (definition is ChildrenDefinition children)
            {
                return children.Length;
            }

            return 1;
        }

        internal static Descriptor GetDescriptor(uint id, int index)
        {
            if (index < 0)
            {
                throw new IndexOutOfRangeException();
            }
            var definition = GetDefinition(id);
            if (definition == null)
            {
                throw new IndexOutOfRangeException();
            }
            
            if (definition is ChildrenDefinition children)
            {
                if (index >= children.Length)
                {
                    throw new IndexOutOfRangeException();
                }

                return children.GetDescriptor(index);
            }
            
            if (index > 0)
            {
                throw new IndexOutOfRangeException();
            }

            return (definition as NodeElementDefinition)?.Descriptor ?? default;
        }

        internal static Element SetDescriptor(uint id, int index, Descriptor descriptor)
        {
            if (index < 0)
            {
                throw new IndexOutOfRangeException();
            }
            var definition = GetDefinition(id);
            if (definition == null)
            {
                throw new IndexOutOfRangeException();
            }
            
            if (definition is ChildrenDefinition children)
            {
                if (index >= children.Length)
                {
                    throw new IndexOutOfRangeException();
                }

                return children.SetDescriptor(index, descriptor);
            }
            
            if (index > 0)
            {
                throw new IndexOutOfRangeException();
            }

            return (definition as NodeElementDefinition)?.New(descriptor) ?? default;
        }

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
        
        public static T RefProps<T>(RefAction<T> func) where T : struct => RefProps(Defaults.GetValue<T>(), func);
        public static T RefProps<T>(T d, RefAction<T> func) where T : struct
        {
            func?.Invoke(ref d);
                
            return d;
        }
    }
}
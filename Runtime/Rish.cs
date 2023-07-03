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
         
        private static Type[] _playerTypes;
        internal static Type[] PlayerTypes
        {
            get
            {
                if (_playerTypes == null)
                {
#if UNITY_EDITOR
                    var playerAssemblies = UnityEditor.Compilation.CompilationPipeline.GetAssemblies(UnityEditor.Compilation.AssembliesType.PlayerWithoutTestAssemblies).Select(asm => asm.name).ToArray();
#else
                    var assembliesToIgnore = new[] {
                        "Unity.",
                        "UnityEngine",
                        "UnityEditor",
                        "mscorlib",
                        "System",
                        "bee,",
                        "unityplastic,",
                        "NSubstitute,",
                        "nunit",
                        "AWSSDK",
                        "Newtonsoft",
                        "Mono.",
                        "Microsoft.",
                        "Google.",
                        "log4net",
                    };
#endif
                    _playerTypes = AppDomain.CurrentDomain.GetAssemblies().Where(ShouldIncludeAssembly).SelectMany(asm => asm.GetTypes()).ToArray();
                    
                    bool ShouldIncludeAssembly(Assembly asm)
                    {
                        if (asm.IsDynamic)
                        {
                            return true;
                        }

                        var name = asm.GetName().Name;
#if UNITY_EDITOR
                        return !name.Contains("Unity") && playerAssemblies.Contains(name);
#else
                        foreach (var ignored in assembliesToIgnore)
                        {
                            if (name.StartsWith(ignored))
                            {
                                return false;
                            }
                        }

                        return true;
#endif
                    }

                }

                return _playerTypes;
            }
        }
        
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

        private static void ReturnToPool(uint id)
        {
            var definition = GetDefinition(id);
            if (definition == null)
            {
                return;
            }

            if (definition.ReferencesCount > 0)
            {
                throw new UnityException("Element isn't ready to be returned to the pool.");
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
            return definition switch
            {
                null => 0,
                ChildrenDefinition children => children.Length,
                _ => 1
            };
        }
        internal static Element GetChild(uint id, int index)
        {
            var definition = GetDefinition(id);
            return definition switch
            {
                null => throw new ArgumentNullException(),
                ChildrenDefinition children => children[index],
                _ => index == 0 ? new Element(id) : throw new ArgumentNullException()
            };
        }

        private static HashSet<uint> GarbageSet { get; } = new(InitialPoolSize);
        private static List<uint> Garbage { get; } = new(InitialPoolSize);

        internal static void CleanGarbage()
        {
            for (int i = 0, n = Garbage.Count; i < n; i++)
            {
                ReturnToPool(Garbage[i]);
            }
            
            GarbageSet.Clear();
            Garbage.Clear();
        }

        private static void AddGarbage(uint id)
        {
            if (GarbageSet.Contains(id))
            {
                return;
            }

            GarbageSet.Add(id);
            Garbage.Add(id);
        }

        private static void RemoveGarbage(uint id)
        {
            if (!GarbageSet.Contains(id))
            {
                return;
            }

            GarbageSet.Remove(id);
            Garbage.Remove(id);
        }

        internal static void RegisterReferenceTo(uint id, IOwner owner)
        {
            if (id == 0)
            {
                return;
            }
            var definition = GetDefinition(id);
            if (definition == null)
            {
                return;
            }
            
            if (definition.RegisterReference(owner) == 1)
            {
                RemoveGarbage(id);
            }
        }
        internal static void UnregisterReferenceTo(uint id, IOwner owner)
        {
            if (id == 0)
            {
                return;
            }
            var definition = GetDefinition(id);
            if (definition == null)
            {
                return;
            }
            
            if (definition.UnregisterReference(owner) <= 0)
            {
                AddGarbage(id);
            }
        }

        private static Children CreateChildren(ElementDefinition definition)
        {
            var id = DefinitionId;
            DefinitionsInUse[id] = definition;
            var element = new Children(id);
            AddGarbage(id);

            return element;
        }
        
        public static T RefProps<T>(RefAction<T> func) where T : struct => RefProps(Defaults.GetValue<T>(), func);
        public static T RefProps<T>(T d, RefAction<T> func) where T : struct
        {
            func?.Invoke(ref d);
                
            return d;
        }
    }
}
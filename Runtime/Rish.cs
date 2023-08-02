using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RishUI.MemoryManagement;
using UnityEngine;

namespace RishUI
{
    public delegate void RefAction<T>(ref T value) where T : struct;

    public static partial class Rish
    {
        private static List<IPool> PoolsList { get; } = new();
        private static Dictionary<Type, int> PoolsIndices { get; } = new();
        
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

        public static Reference GetReferenceTo<T>(uint id) where T : class, IManaged => new Reference
        {
            poolIndex = GetPoolIndex<T>(),
            managedID = id
        };
        private static int GetPoolIndex<T>() where T : class, IManaged
        {
            var type = typeof(T);
            if (!PoolsIndices.TryGetValue(type, out var poolIndex))
            {
                Type poolType;
                if (Attribute.IsDefined(type, typeof(CustomManagedAttribute)))
                {
                    var attribute = (CustomManagedAttribute)Attribute.GetCustomAttribute(type, typeof(CustomManagedAttribute));
                    poolType = attribute.poolType;
                }
                else
                {
                    return -1;
                }
                
                poolIndex = PoolsList.FindIndex(p => p.GetType() == poolType);
                if (poolIndex < 0)
                {
                    return -1;
                }

                PoolsIndices[type] = poolIndex;
            }

            return poolIndex;
        }
        private static IPool GetPool<T>() where T : class, IManaged
        {
            var type = typeof(T);
            if (!PoolsIndices.TryGetValue(type, out var poolIndex))
            {
                Type poolType;
                if (Attribute.IsDefined(type, typeof(CustomManagedAttribute)))
                {
                    var attribute = (CustomManagedAttribute)Attribute.GetCustomAttribute(type, typeof(CustomManagedAttribute));
                    poolType = attribute.poolType;
                }
                else
                {
                    throw new UnityException("Invalid pool");
                }
                
                poolIndex = PoolsList.FindIndex(p => p.GetType() == poolType);
                if (poolIndex < 0)
                {
                    throw new UnityException("Invalid pool");
                }

                PoolsIndices[type] = poolIndex;
            }

            return PoolsList[poolIndex];
        }
        private static IPool GetPoolOrCreate<T>() where T : class, IManaged, new()
        {
            var type = typeof(T);
            if (!PoolsIndices.TryGetValue(type, out var poolIndex))
            {
                Type poolType;
                if (Attribute.IsDefined(type, typeof(CustomManagedAttribute)))
                {
                    var attribute = (CustomManagedAttribute)Attribute.GetCustomAttribute(type, typeof(CustomManagedAttribute));
                    poolType = attribute.poolType;
                }
                else
                {
                    poolType = typeof(GenericPool<T>);
                }
                
                poolIndex = PoolsList.FindIndex(p => p.GetType() == poolType);
                if (poolIndex < 0)
                {
                    poolIndex = PoolsList.Count;
                    var pool = (IPool) Activator.CreateInstance(poolType);
                    PoolsList.Add(pool);
                }

                PoolsIndices[type] = poolIndex;
            }
            
            return PoolsList[poolIndex];
        }

        public static uint GetFreeID<T>() where T : class, IManaged, new() => GetPoolOrCreate<T>().GetFreeID<T>();

        private static (uint, T) GetFree<T>() where T : class, IManaged, new()
        {
            var id = GetFreeID<T>();
            var element = GetManaged<T>(id);
        
            return (id, element);
        }
        
        internal static T GetManaged<T>(uint id) where T : class, IManaged => id > 0 ? GetPool<T>()?.GetManaged<T>(id) : null;
        
        public static void RegisterReferenceTo<T>(uint id, IOwner owner) where T : class, IManaged
        {
            if (id <= 0)
            {
                return;
            }
            
            Debug.Log($"Register {typeof(T)}({id}) to {owner.GetType()}");
            
            GetPool<T>()?.RegisterReference(id, owner);
        }
        public static void UnregisterReferenceTo<T>(uint id, IOwner owner) where T : class, IManaged
        {
            if (id <= 0)
            {
                return;
            }
            
            Debug.Log($"Unregister {typeof(T)}({id}) from {owner.GetType()}");
            
            GetPool<T>()?.UnregisterReference(id, owner);
        }
        public static void RegisterReferenceTo(Reference reference, IOwner owner)
        {
            var poolIndex = reference.poolIndex;
            if (poolIndex < 0)
            {
                return;
            }

            var id = reference.managedID;
            if (id <= 0)
            {
                return;
            }
            
            var pool = PoolsList[poolIndex];
            Debug.Log($"Register {pool.GetType()}({id}) to {owner.GetType()}");
            pool.RegisterReference(id, owner);
        }
        public static void UnregisterReferenceTo(Reference reference, IOwner owner)
        {
            var poolIndex = reference.poolIndex;
            if (poolIndex < 0)
            {
                return;
            }

            var id = reference.managedID;
            if (id <= 0)
            {
                return;
            }

            var pool = PoolsList[poolIndex];
            Debug.Log($"Unregister {pool.GetType()}({id}) from {owner.GetType()}");
            pool.UnregisterReference(id, owner);
        }

        internal static void CleanGarbage()
        {
            foreach (var pool in PoolsList)
            {
                pool.CleanGarbage();
            }
        }
    }
}
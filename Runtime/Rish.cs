using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RishUI.MemoryManagement;
using UnityEngine;

namespace RishUI
{
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

            return PoolsList[poolIndex] as IPool;
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

        public static ulong GetFreeID<T>() where T : class, IManaged, new() => GetPoolOrCreate<T>().GetFreeID<T>();

        private static (ulong, T) GetFree<T>() where T : class, IManaged, new()
        {
            var id = GetFreeID<T>();
            var element = GetManaged<T>(id);
        
            return (id, element);
        }
        
        public static T GetManaged<T>(ulong id) where T : class, IManaged => id > 0 ? GetPool<T>()?.GetManaged<T>(id) : null;

        public static ManagedContext GetOwnerContext<T1, T2>(T1 reference) where T1 : struct, IReference<T2> where T2 : class, IManaged => GetPool<T2>()?.GetWrapper<T2>(reference.ID)?.OwnerContext;
        public static ManagedContext GetOwnerContext<T>(ulong id) where T : class, IManaged => GetManaged<T>(id)?.OwnerContext;
    }
}
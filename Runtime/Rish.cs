using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI
{
    public delegate void RefAction<T>(ref T value) where T : struct;

    public struct Element : IEquatable<Element>
    {
        private int _id;

        public bool Valid => _id > 0;
        
        public static Element Null => new ();

        public Descriptor Descriptor => GetDefinition().Descriptor; 

        private ElementDefinition GetDefinition()
        {
            var definition = Rish.GetDefinition(_id);
            if (definition == null)
            {
                throw new UnityException("Invalid element");
            }

            return definition;
        }

        public Element New(Descriptor descriptor) => Valid ? GetDefinition().New(descriptor) : Null;

        public Element New(RefAction<Descriptor> action) => Valid ? GetDefinition().New(action) : Null;

        public Element Copy() => Valid ? GetDefinition().Copy() : Null;

        internal void Invoke(Node node)
        {
            if (!Valid)
            {
                return;
            }
            
            Rish.GetDefinition(_id).Invoke(node);
        }

        internal void ReturnToPool() => Rish.ReturnToPool(_id);
        
        public static implicit operator Element(ElementDefinition definition) => new()
        {
            _id = definition.ID
        };
        
        bool IEquatable<Element>.Equals(Element other) => Equals(this, other);

        [Comparer]
        private static bool Equals(Element a, Element b)
        {
            var aSet = a.Valid;
            var bSet = b.Valid;
            if (aSet ^ bSet)
            {
                return false;
            }

            return !aSet || Rish.GetDefinition(a._id).Equals(Rish.GetDefinition(b._id));
        }
    }
    
    public abstract class ElementDefinition : IEquatable<ElementDefinition>
    {
        private static int _nextId;
        public int ID { get; } = ++_nextId;
        
        private IOwner _owner;
        internal IOwner Owner
        {
            get => _owner;
            set
            {
                if (_owner != null && value != null)
                {
                    throw new UnityException("ElementSetup already has an owner");
                }

                _owner = value;
            }
        }
        
        public Descriptor Descriptor { get; protected set; }

        internal void Restart()
        {
            Owner = null;
        }

        public Element Copy() => New(Descriptor);
        
        public abstract Element New(Descriptor descriptor);

        public Element New(RefAction<Descriptor> action)
        {
            var descriptor = Descriptor;
            action?.Invoke(ref descriptor);
            return New(descriptor);
        }

        public abstract void Invoke(Node node);
        public abstract bool Equals(ElementDefinition other);
    }

    public struct Descriptor : IEquatable<Descriptor>
    {
        public uint key;
        public Name name;
        public ClassName className;
        public Style style;
    
        public static Descriptor Default => new();
        
        public Descriptor(Descriptor other)
        {
            key = other.key;
            name = other.name;
            className = other.className;
            style = other.style;
        }
        
        bool IEquatable<Descriptor>.Equals(Descriptor other) => Equals(this, other);
    
        [Comparer]
        private static bool Equals(Descriptor a, Descriptor b)
        {
            return RishUtils.CompareUnmanaged<Unmanaged>(a, b) && Style.Equals(a.style, b.style);
        }
    
        private struct Unmanaged
        {
            private uint _key;
            private Name _name;
            private ClassName _className;
        
            public static implicit operator Unmanaged(Descriptor managed) => new()
            {
                _key = managed.key,
                _name = managed.name,
                _className = managed.className
            };
        }
    }

    public interface IOwner
    {
        void TakeOwnership(Element element);
        void TakeOwnership(Children children);
    }

    // TODO: Split into partial classes
    public static class Rish
    {
        private static Stack<IOwner> Owners { get; } = new();
        
        private const int InitialPoolSize = 64;
        private static Dictionary<Type, Stack<ElementDefinition>> Pools { get; } = new();

        // TODO: Replicate ActiveChildren so we know when an Element was "disposed" (so we can warn properly)
        private static uint _nextElementId;
        // private static List<ElementDefinition> All { get; } = new();
        private static Dictionary<uint, ElementDefinition> ActiveElements { get; } = new();
        private static uint _nextChildrenId;
        private static Dictionary<uint, NativeArray<Element>> ActiveChildren { get; } = new();
        
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
                // All.Add(element);
            }
            else
            {
                element = (T)pool.Pop();
            }

            element.Restart();
            
            return element;
        }

        internal static void ReturnToPool(int id)
        {
            var definition = GetDefinition(id);
            if (definition == null)
            {
                return;
            }
            
            var type = definition.GetType();
            if (!Pools.TryGetValue(type, out var pool))
            {
                throw new UnityException($"{type} is an invalid element type. No pool found.");
            }
            
            pool.Push(definition);
        }

        internal static void Dispose(uint id)
        {
            if (!ActiveChildren.TryGetValue(id, out var children))
            {
                return;
            }
            
            children.Dispose();
            ActiveChildren.Remove(id);
        }

        internal static ElementDefinition GetDefinition(int id) => All[id - 1];
        internal static NativeArray<Element> GetNativeArray(uint id)
        {
            if (!ActiveChildren.TryGetValue(id, out var children))
            {
                return default;
            }
            
            return children;
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

        private static void OnCreate(ElementDefinition definition)
        {
            var owner = Owners.Peek();
            if (owner == null)
            {
                throw new UnityException("There's nobody to claim ownership of this ElementDefinition");
            }

            definition.Owner = owner;
            owner.TakeOwnership(definition);
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

        private static Children CreateChildren(NativeArray<Element> array)
        {
            var id = _nextChildrenId == uint.MaxValue ? 0 : ++_nextChildrenId;
            ActiveChildren[id] = array;
            var children = new Children(id);
            OnCreate(children);

            return children;
        }

        public static Children Children() => RishUI.Children.Empty;
        public static Children Children(Element child0)
        {
            var array = new NativeArray<Element>(1, Allocator.Persistent);
            array[0] = child0;
            
            return CreateChildren(array);
        }
        public static Children Children(Element child0, Element child1)
        {
            var array = new NativeArray<Element>(2, Allocator.Persistent);
            array[0] = child0;
            array[1] = child1;
            
            return CreateChildren(array);
        }
        public static Children Children(Element child0, Element child1, Element child2)
        {
            var array = new NativeArray<Element>(3, Allocator.Persistent);
            array[0] = child0;
            array[1] = child1;
            array[2] = child2;
            
            return CreateChildren(array);
        }
        public static Children Children(Element child0, Element child1, Element child2, Element child3)
        {
            var array = new NativeArray<Element>(4, Allocator.Persistent);
            array[0] = child0;
            array[1] = child1;
            array[2] = child2;
            array[3] = child3;
            
            return CreateChildren(array);
        }
        public static Children Children(Element child0, Element child1, Element child2, Element child3, Element child4)
        {
            var array = new NativeArray<Element>(5, Allocator.Persistent);
            array[0] = child0;
            array[1] = child1;
            array[2] = child2;
            array[3] = child3;
            array[4] = child4;
            
            return CreateChildren(array);
        }
        public static Children Children(params Element[] children)
        {
            var length = children?.Length ?? 0;
            if (length <= 0)
            {
                return Children();
            }
            var array = new NativeArray<Element>(length, Allocator.Persistent);
            for (var i = 0; i < length; i++)
            {
                array[i] = children[i];
            }
            
            return CreateChildren(array);
        }
        public static Children Children(List<Element> children)
        {
            var length = children?.Count ?? 0;
            if (length <= 0)
            {
                return Children();
            }
            var array = new NativeArray<Element>(length, Allocator.Persistent);
            for (var i = 0; i < length; i++)
            {
                array[i] = children[i];
            }
            
            return CreateChildren(array);
        }
        internal static Children CopyChildren(Children children)
        {
            var length = children.Valid ? children.Count : 0;
            if (length <= 0)
            {
                return Children();
            }
            var array = new NativeArray<Element>(length, Allocator.Persistent);
            for (var i = 0; i < length; i++)
            {
                array[i] = children[i].Copy();
            }
            
            return CreateChildren(array);
        }
        
        // -------------------------------------------------------------------------------------------------------------
        // --- FUNCTION ELEMENTS ---------------------------------------------------------------------------------------
        // -------------------------------------------------------------------------------------------------------------
        
        // 0/4 -> 1
        public static Element Create(FunctionElement functionElement) => Create(functionElement, Descriptor.Default);
        // 1/4 -> 4
        public static Element Create(FunctionElement functionElement, uint key) => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            key = key
        });
        public static Element Create(FunctionElement functionElement, Name name) => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            name = name
        });
        public static Element Create(FunctionElement functionElement, ClassName className) => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            className = className
        });
        public static Element Create(FunctionElement functionElement, Style style) => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            style = style
        });
        // 2/4 -> 6
        public static Element Create(FunctionElement functionElement, uint key, Name name) => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name
        });
        public static Element Create(FunctionElement functionElement, uint key, ClassName className) => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            key = key,
            className = className
        });
        public static Element Create(FunctionElement functionElement, uint key, Style style) => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            key = key,
            style = style
        });
        public static Element Create(FunctionElement functionElement, Name name, ClassName className) => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            name = name,
            className = className,
        });
        public static Element Create(FunctionElement functionElement, Name name, Style style) => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            name = name,
            style = style
        });
        public static Element Create(FunctionElement functionElement, ClassName className, Style style) => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            className = className,
            style = style
        });
        // 3/4 -> 4
        public static Element Create(FunctionElement functionElement, uint key, Name name, ClassName className) => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            className = className
        });
        public static Element Create(FunctionElement functionElement, uint key, Name name, Style style) => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            style = style
        });
        public static Element Create(FunctionElement functionElement, uint key, ClassName className, Style style) => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            key = key,
            className = className,
            style = style
        });
        public static Element Create(FunctionElement functionElement, Name name, ClassName className, Style style) => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            name = name,
            className = className,
            style = style
        });
        // 4/4 -> 1
        public static Element Create(FunctionElement functionElement, uint key, Name name, ClassName className, Style style) => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            className = className,
            style = style
        });
        // Descriptor
        public static Element Create(FunctionElement functionElement, Descriptor descriptor)
        {
            var element = GetFromPool<FunctionalDefinition>();
            element.Factory(descriptor, functionElement);
            
            OnCreate(element);

            return element;
        }
        
        
        
        // 0/5 -> 1
        public static Element Create<P>(FunctionElement<P> functionElement) where P : struct => Create(functionElement, Descriptor.Default);
        // 1/5 -> 5
        public static Element Create<P>(FunctionElement<P> functionElement, uint key) where P : struct => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            key = key
        });
        public static Element Create<P>(FunctionElement<P> functionElement, Name name) where P : struct => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            name = name
        });
        public static Element Create<P>(FunctionElement<P> functionElement, ClassName className) where P : struct => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            className = className
        });
        public static Element Create<P>(FunctionElement<P> functionElement, Style style) where P : struct => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            style = style
        });
        public static Element Create<P>(FunctionElement<P> functionElement, P props) where P : struct => Create(functionElement, new Descriptor(Descriptor.Default), props);
        public static Element Create<P>(FunctionElement<P> functionElement, RefAction<P> props) where P : struct => Create(functionElement, new Descriptor(Descriptor.Default), props);
        // 2/5 -> 10
        public static Element Create<P>(FunctionElement<P> functionElement, uint key, Name name) where P : struct => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name
        });
        public static Element Create<P>(FunctionElement<P> functionElement, uint key, ClassName className) where P : struct => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            key = key,
            className = className
        });
        public static Element Create<P>(FunctionElement<P> functionElement, uint key, Style style) where P : struct => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            key = key,
            style = style
        });
        public static Element Create<P>(FunctionElement<P> functionElement, uint key, P props) where P : struct => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            key = key
        }, props);
        public static Element Create<P>(FunctionElement<P> functionElement, uint key, RefAction<P> props) where P : struct => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            key = key
        }, props);
        public static Element Create<P>(FunctionElement<P> functionElement, Name name, ClassName className) where P : struct => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            name = name,
            className = className
        });
        public static Element Create<P>(FunctionElement<P> functionElement, Name name, Style style) where P : struct => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            name = name,
            style = style
        });
        public static Element Create<P>(FunctionElement<P> functionElement, Name name, P props) where P : struct => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            name = name
        }, props);
        public static Element Create<P>(FunctionElement<P> functionElement, Name name, RefAction<P> props) where P : struct => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            name = name
        }, props);
        public static Element Create<P>(FunctionElement<P> functionElement, ClassName className, Style style) where P : struct => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            className = className,
            style = style
        });
        public static Element Create<P>(FunctionElement<P> functionElement, ClassName className, P props) where P : struct => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            className = className
        }, props);
        public static Element Create<P>(FunctionElement<P> functionElement, ClassName className, RefAction<P> props) where P : struct => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            className = className
        }, props);
        public static Element Create<P>(FunctionElement<P> functionElement, Style style, P props) where P : struct => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            style = style
        }, props);
        public static Element Create<P>(FunctionElement<P> functionElement, Style style, RefAction<P> props) where P : struct => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            style = style
        }, props);
        // 3/5 -> 10
        public static Element Create<P>(FunctionElement<P> functionElement, uint key, Name name, ClassName className) where P : struct => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            className = className
        });
        public static Element Create<P>(FunctionElement<P> functionElement, uint key, Name name, Style style) where P : struct => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            style = style
        });
        public static Element Create<P>(FunctionElement<P> functionElement, uint key, Name name, P props) where P : struct => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name
        }, props);
        public static Element Create<P>(FunctionElement<P> functionElement, uint key, Name name, RefAction<P> props) where P : struct => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name
        }, props);
        public static Element Create<P>(FunctionElement<P> functionElement, uint key, ClassName className, Style style) where P : struct => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            key = key,
            className = className,
            style = style
        });
        public static Element Create<P>(FunctionElement<P> functionElement, uint key, ClassName className, P props) where P : struct => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            key = key,
            className = className
        }, props);
        public static Element Create<P>(FunctionElement<P> functionElement, uint key, ClassName className, RefAction<P> props) where P : struct => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            key = key,
            className = className
        }, props);
        public static Element Create<P>(FunctionElement<P> functionElement, uint key, Style style, P props) where P : struct => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            key = key,
            style = style
        }, props);
        public static Element Create<P>(FunctionElement<P> functionElement, uint key, Style style, RefAction<P> props) where P : struct => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            key = key,
            style = style
        }, props);
        public static Element Create<P>(FunctionElement<P> functionElement, Name name, ClassName className, Style style) where P : struct => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            name = name,
            className = className,
            style = style
        });
        public static Element Create<P>(FunctionElement<P> functionElement, Name name, ClassName className, P props) where P : struct => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            name = name,
            className = className
        }, props);
        public static Element Create<P>(FunctionElement<P> functionElement, Name name, ClassName className, RefAction<P> props) where P : struct => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            name = name,
            className = className
        }, props);
        public static Element Create<P>(FunctionElement<P> functionElement, Name name, Style style, P props) where P : struct => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            name = name,
            style = style
        }, props);
        public static Element Create<P>(FunctionElement<P> functionElement, Name name, Style style, RefAction<P> props) where P : struct => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            name = name,
            style = style
        }, props);
        public static Element Create<P>(FunctionElement<P> functionElement, ClassName className, Style style, P props) where P : struct => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            className = className,
            style = style
        }, props);
        public static Element Create<P>(FunctionElement<P> functionElement, ClassName className, Style style, RefAction<P> props) where P : struct => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            className = className,
            style = style
        }, props);
        // 4/5 -> 5
        public static Element Create<P>(FunctionElement<P> functionElement, uint key, Name name, ClassName className, Style style) where P : struct => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            className = className,
            style = style
        });
        public static Element Create<P>(FunctionElement<P> functionElement, uint key, Name name, ClassName className, P props) where P : struct => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            className = className
        }, props);
        public static Element Create<P>(FunctionElement<P> functionElement, uint key, Name name, ClassName className, RefAction<P> props) where P : struct => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            className = className
        }, props);
        public static Element Create<P>(FunctionElement<P> functionElement, uint key, Name name, Style style, P props) where P : struct => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            style = style
        }, props);
        public static Element Create<P>(FunctionElement<P> functionElement, uint key, Name name, Style style, RefAction<P> props) where P : struct => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            style = style
        }, props);
        public static Element Create<P>(FunctionElement<P> functionElement, uint key, ClassName className, Style style, P props) where P : struct => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            key = key,
            className = className,
            style = style
        }, props);
        public static Element Create<P>(FunctionElement<P> functionElement, uint key, ClassName className, Style style, RefAction<P> props) where P : struct => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            key = key,
            className = className,
            style = style
        }, props);
        public static Element Create<P>(FunctionElement<P> functionElement, Name name, ClassName className, Style style, P props) where P : struct => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            name = name,
            className = className,
            style = style
        }, props);
        public static Element Create<P>(FunctionElement<P> functionElement, Name name, ClassName className, Style style, RefAction<P> props) where P : struct => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            name = name,
            className = className,
            style = style
        }, props);
        // 5/5 -> 1
        public static Element Create<P>(FunctionElement<P> functionElement, uint key, Name name, ClassName className, Style style, P props) where P : struct => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            className = className,
            style = style
        }, props);
        public static Element Create<P>(FunctionElement<P> functionElement, uint key, Name name, ClassName className, Style style, RefAction<P> props) where P : struct => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            className = className,
            style = style
        }, props);
        // Descriptor
        public static Element Create<P>(FunctionElement<P> functionElement, Descriptor descriptor) where P : struct => Create(functionElement, descriptor, Defaults.GetValue<P>());
        public static Element Create<P>(FunctionElement<P> functionElement, Descriptor descriptor, RefAction<P> props) where P : struct => Create(functionElement, descriptor, RefProps(props));
        public static Element Create<P>(FunctionElement<P> functionElement, Descriptor descriptor, P props) where P : struct
        {
            var element = GetFromPool<FunctionalDefinition<P>>();
            element.Factory(descriptor, functionElement, props);
            
            OnCreate(element);

            return element;
        }
        
        
        
        
        
        // -------------------------------------------------------------------------------------------------------------
        // --- NATIVE ELEMENTS -----------------------------------------------------------------------------------------
        // -------------------------------------------------------------------------------------------------------------
        
        // 0/4 -> 1
        public static Element Create<T>(Children? children = null) where T : VisualElement, IPrimitiveElement, new() => Create<T>(Descriptor.Default, children);
        // 1/4 -> 4
        public static Element Create<T>(uint key, Children? children = null) where T : VisualElement, IPrimitiveElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            key = key
        }, children);
        public static Element Create<T>(Name name, Children? children = null) where T : VisualElement, IPrimitiveElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            name = name
        }, children);
        public static Element Create<T>(ClassName className, Children? children = null) where T : VisualElement, IPrimitiveElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            className = className
        }, children);
        public static Element Create<T>(Style style, Children? children = null) where T : VisualElement, IPrimitiveElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            style = style
        }, children);
        // 2/4 -> 6
        public static Element Create<T>(uint key, Name name, Children? children = null) where T : VisualElement, IPrimitiveElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name
        }, children);
        public static Element Create<T>(uint key, ClassName className, Children? children = null) where T : VisualElement, IPrimitiveElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            key = key,
            className = className
        }, children);
        public static Element Create<T>(uint key, Style style, Children? children = null) where T : VisualElement, IPrimitiveElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            key = key,
            style = style
        }, children);
        public static Element Create<T>(Name name, ClassName className, Children? children = null) where T : VisualElement, IPrimitiveElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            name = name,
            className = className,
        }, children);
        public static Element Create<T>(Name name, Style style, Children? children = null) where T : VisualElement, IPrimitiveElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            name = name,
            style = style
        }, children);
        public static Element Create<T>(ClassName className, Style style, Children? children = null) where T : VisualElement, IPrimitiveElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            className = className,
            style = style
        }, children);
        // 3/4 -> 4
        public static Element Create<T>(uint key, Name name, ClassName className, Children? children = null) where T : VisualElement, IPrimitiveElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            className = className
        }, children);
        public static Element Create<T>(uint key, Name name, Style style, Children? children = null) where T : VisualElement, IPrimitiveElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            style = style
        }, children);
        public static Element Create<T>(uint key, ClassName className, Style style, Children? children = null) where T : VisualElement, IPrimitiveElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            key = key,
            className = className,
            style = style
        }, children);
        public static Element Create<T>(Name name, ClassName className, Style style, Children? children = null) where T : VisualElement, IPrimitiveElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            name = name,
            className = className,
            style = style
        }, children);
        // 4/4 -> 1
        public static Element Create<T>(uint key, Name name, ClassName className, Style style, Children? children = null) where T : VisualElement, IPrimitiveElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            className = className,
            style = style
        }, children);
        // Descriptor
        public static Element Create<T>(Descriptor descriptor, Children? children = null) where T : VisualElement, IPrimitiveElement, new()
        {
            var element = GetFromPool<PrimitiveDefinition<T>>();
            element.Factory(descriptor, children ?? RishUI.Children.Empty);
            
            OnCreate(element);

            return element;
        }



        // 0/5 -> 1
        public static Element Create<T, P>(Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(Descriptor.Default, children);
        // 1/5 -> 5
        public static Element Create<T, P>(uint key, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key
        }, children);
        public static Element Create<T, P>(Name name, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            name = name
        }, children);
        public static Element Create<T, P>(ClassName className, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            className = className
        }, children);
        public static Element Create<T, P>(Style style, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            style = style
        }, children);
        public static Element Create<T, P>(P props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default), props, children);
        public static Element Create<T, P>(RefAction<P> props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default), props, children);
        // 2/5 = 10
        public static Element Create<T, P>(uint key, Name name, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name
        }, children);
        public static Element Create<T, P>(uint key, ClassName className, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            className = className
        }, children);
        public static Element Create<T, P>(uint key, Style style, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            style = style
        }, children);
        public static Element Create<T, P>(uint key, P props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key
        }, props, children);
        public static Element Create<T, P>(uint key, RefAction<P> props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key
        }, props, children);
        public static Element Create<T, P>(Name name, ClassName className, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            name = name,
            className = className
        }, children);
        public static Element Create<T, P>(Name name, Style style, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            name = name,
            style = style
        }, children);
        public static Element Create<T, P>(Name name, P props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            name = name
        }, props, children);
        public static Element Create<T, P>(Name name, RefAction<P> props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            name = name
        }, props, children);
        public static Element Create<T, P>(ClassName className, Style style, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            className = className,
            style = style
        }, children);
        public static Element Create<T, P>(ClassName className, P props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            className = className
        }, props, children);
        public static Element Create<T, P>(ClassName className, RefAction<P> props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            className = className
        }, props, children);
        public static Element Create<T, P>(Style style, P props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            style = style
        }, props, children);
        public static Element Create<T, P>(Style style, RefAction<P> props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            style = style
        }, props, children);
        // 3/5 = 10
        public static Element Create<T, P>(uint key, Name name, ClassName className, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            className = className
        }, children);
        public static Element Create<T, P>(uint key, Name name, Style style, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            style = style
        }, children);
        public static Element Create<T, P>(uint key, Name name, P props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name
        }, props, children);
        public static Element Create<T, P>(uint key, Name name, RefAction<P> props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name
        }, props, children);
        public static Element Create<T, P>(uint key, ClassName className, Style style, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            className = className,
            style = style
        }, children);
        public static Element Create<T, P>(uint key, ClassName className, P props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            className = className
        }, props, children);
        public static Element Create<T, P>(uint key, ClassName className, RefAction<P> props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            className = className
        }, props, children);
        public static Element Create<T, P>(uint key, Style style, P props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            style = style
        }, props, children);
        public static Element Create<T, P>(uint key, Style style, RefAction<P> props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            style = style
        }, props, children);
        public static Element Create<T, P>(Name name, ClassName className, Style style, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            name = name,
            className = className,
            style = style
        }, children);
        public static Element Create<T, P>(Name name, ClassName className, P props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            name = name,
            className = className
        }, props, children);
        public static Element Create<T, P>(Name name, ClassName className, RefAction<P> props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            name = name,
            className = className
        }, props, children);
        public static Element Create<T, P>(Name name, Style style, P props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            name = name,
            style = style
        }, props, children);
        public static Element Create<T, P>(Name name, Style style, RefAction<P> props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            name = name,
            style = style
        }, props, children);
        public static Element Create<T, P>(ClassName className, Style style, P props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            className = className,
            style = style
        }, props, children);
        public static Element Create<T, P>(ClassName className, Style style, RefAction<P> props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            className = className,
            style = style
        }, props, children);
        // 4/5 = 5
        public static Element Create<T, P>(uint key, Name name, ClassName className, Style style, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            className = className,
            style = style
        }, children);
        public static Element Create<T, P>(uint key, Name name, ClassName className, P props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            className = className
        }, props, children);
        public static Element Create<T, P>(uint key, Name name, ClassName className, RefAction<P> props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            className = className
        }, props, children);
        public static Element Create<T, P>(uint key, Name name, Style style, P props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            style = style
        }, props, children);
        public static Element Create<T, P>(uint key, Name name, Style style, RefAction<P> props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            style = style
        }, props, children);
        public static Element Create<T, P>(uint key, ClassName className, Style style, P props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            className = className,
            style = style
        }, props, children);
        public static Element Create<T, P>(uint key, ClassName className, Style style, RefAction<P> props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            className = className,
            style = style
        }, props, children);
        public static Element Create<T, P>(Name name, ClassName className, Style style, P props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            name = name,
            className = className,
            style = style
        }, props, children);
        public static Element Create<T, P>(Name name, ClassName className, Style style, RefAction<P> props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            name = name,
            className = className,
            style = style
        }, props, children);
        // 5/5 = 1
        public static Element Create<T, P>(uint key, Name name, ClassName className, Style style, P props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            className = className,
            style = style
        }, props, children);
        public static Element Create<T, P>(uint key, Name name, ClassName className, Style style, RefAction<P> props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            className = className,
            style = style
        }, props, children);
        // Descriptor
        public static Element Create<T, P>(Descriptor descriptor, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(descriptor, Defaults.GetValue<P>(), children);
        public static Element Create<T, P>(Descriptor descriptor, RefAction<P> props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct => Create<T, P>(descriptor, RefProps(props), children);
        // Descriptor
        public static Element Create<T, P>(Descriptor descriptor, P props, Children? children = null) where T : VisualElement, IPrimitiveElement<P>, new() where P : struct
        {
            var element = GetFromPool<PrimitiveDefinition<T, P>>();
            element.Factory(descriptor, props, children ?? RishUI.Children.Empty);
            
            OnCreate(element);

            return element;
        }
        
        
        
        
        
        // -------------------------------------------------------------------------------------------------------------
        // --- RISH ELEMENTS -------------------------------------------------------------------------------------------
        // -------------------------------------------------------------------------------------------------------------
        
        // 0/4 -> 1
        public static Element Create<T>() where T : RishElement, new() => Create<T>(Descriptor.Default);
        // 1/4 -> 4
        public static Element Create<T>(uint key) where T : RishElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            key = key
        });
        public static Element Create<T>(Name name) where T : RishElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            name = name,
        });
        public static Element Create<T>(ClassName className) where T : RishElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            className = className
        });
        public static Element Create<T>(Style style) where T : RishElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            style = style
        });
        // 2/4 -> 6
        public static Element Create<T>(uint key, Name name) where T : RishElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name
        });
        public static Element Create<T>(uint key, ClassName className) where T : RishElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            key = key,
            className = className
        });
        public static Element Create<T>(uint key, Style style) where T : RishElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            key = key,
            style = style
        });
        public static Element Create<T>(Name name, ClassName className) where T : RishElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            name = name,
            className = className
        });
        public static Element Create<T>(Name name, Style style) where T : RishElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            name = name,
            style = style
        });
        public static Element Create<T>(ClassName className, Style style) where T : RishElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            className = className,
            style = style
        });
        // 3/4 -> 4
        public static Element Create<T>(uint key, Name name, ClassName className) where T : RishElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            className = className
        });
        public static Element Create<T>(uint key, Name name, Style style) where T : RishElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            style = style
        });
        public static Element Create<T>(uint key, ClassName className, Style style) where T : RishElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            key = key,
            className = className,
            style = style
        });
        public static Element Create<T>(Name name, ClassName className, Style style) where T : RishElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            name = name,
            className = className,
            style = style
        });
        // 4/4 -> 1
        public static Element Create<T>(uint key, Name name, ClassName className, Style style) where T : RishElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            className = className,
            style = style
        });
        // Descriptor
        public static Element Create<T>(Descriptor descriptor) where T : RishElement, new()
        {
            var element = GetFromPool<RishDefinition<T, NoProps>>();
            element.Factory(descriptor, default);
            
            OnCreate(element);

            return element;
        }

        // 0/5 -> 1
        public static Element Create<T, P>() where T : RishElement<P>, new() where P : struct => Create<T, P>(Descriptor.Default);
        // 1/5 -> 5
        public static Element Create<T, P>(uint key) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key
        });
        public static Element Create<T, P>(Name name) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            name = name
        });
        public static Element Create<T, P>(ClassName className) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            className = className
        });
        public static Element Create<T, P>(Style style) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            style = style
        });
        public static Element Create<T, P>(P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default), props);
        public static Element Create<T, P>(RefAction<P> props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default), props);
        // 2/5 = 10
        public static Element Create<T, P>(uint key, Name name) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name
        });
        public static Element Create<T, P>(uint key, ClassName className) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            className = className
        });
        public static Element Create<T, P>(uint key, Style style) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            style = style
        });
        public static Element Create<T, P>(uint key, P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key
        }, props);
        public static Element Create<T, P>(uint key, RefAction<P> props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key
        }, props);
        public static Element Create<T, P>(Name name, ClassName className) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            name = name,
            className = className
        });
        public static Element Create<T, P>(Name name, Style style) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            name = name,
            style = style
        });
        public static Element Create<T, P>(Name name, P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            name = name
        }, props);
        public static Element Create<T, P>(Name name, RefAction<P> props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            name = name
        }, props);
        public static Element Create<T, P>(ClassName className, Style style) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            className = className,
            style = style
        });
        public static Element Create<T, P>(ClassName className, P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            className = className
        }, props);
        public static Element Create<T, P>(ClassName className, RefAction<P> props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            className = className
        }, props);
        public static Element Create<T, P>(Style style, P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            style = style
        }, props);
        public static Element Create<T, P>(Style style, RefAction<P> props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            style = style
        }, props);
        // 3/5 = 10
        public static Element Create<T, P>(uint key, Name name, ClassName className) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            className = className
        });
        public static Element Create<T, P>(uint key, Name name, Style style) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            style = style
        });
        public static Element Create<T, P>(uint key, Name name, P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name
        }, props);
        public static Element Create<T, P>(uint key, Name name, RefAction<P> props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name
        }, props);
        public static Element Create<T, P>(uint key, ClassName className, Style style) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            className = className,
            style = style
        });
        public static Element Create<T, P>(uint key, ClassName className, P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            className = className
        }, props);
        public static Element Create<T, P>(uint key, ClassName className, RefAction<P> props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            className = className
        }, props);
        public static Element Create<T, P>(uint key, Style style, P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            style = style
        }, props);
        public static Element Create<T, P>(uint key, Style style, RefAction<P> props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            style = style
        }, props);
        public static Element Create<T, P>(Name name, ClassName className, Style style) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            name = name,
            className = className,
            style = style
        });
        public static Element Create<T, P>(Name name, ClassName className, P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            name = name,
            className = className
        }, props);
        public static Element Create<T, P>(Name name, ClassName className, RefAction<P> props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            name = name,
            className = className
        }, props);
        public static Element Create<T, P>(Name name, Style style, P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            name = name,
            style = style
        }, props);
        public static Element Create<T, P>(Name name, Style style, RefAction<P> props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            name = name,
            style = style
        }, props);
        public static Element Create<T, P>(ClassName className, Style style, P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            className = className,
            style = style
        }, props);
        public static Element Create<T, P>(ClassName className, Style style, RefAction<P> props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            className = className,
            style = style
        }, props);
        // 4/5 = 5
        public static Element Create<T, P>(uint key, Name name, ClassName className, Style style) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            className = className,
            style = style
        });
        public static Element Create<T, P>(uint key, Name name, ClassName className, P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            className = className
        }, props);
        public static Element Create<T, P>(uint key, Name name, ClassName className, RefAction<P> props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            className = className
        }, props);
        public static Element Create<T, P>(uint key, Name name, Style style, P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            style = style
        }, props);
        public static Element Create<T, P>(uint key, Name name, Style style, RefAction<P> props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            style = style
        }, props);
        public static Element Create<T, P>(uint key, ClassName className, Style style, P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            className = className,
            style = style
        }, props);
        public static Element Create<T, P>(uint key, ClassName className, Style style, RefAction<P> props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            className = className,
            style = style
        }, props);
        public static Element Create<T, P>(Name name, ClassName className, Style style, P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            name = name,
            className = className,
            style = style
        }, props);
        public static Element Create<T, P>(Name name, ClassName className, Style style, RefAction<P> props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            name = name,
            className = className,
            style = style
        }, props);
        // 5/5 = 1
        public static Element Create<T, P>(uint key, Name name, ClassName className, Style style, P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            className = className,
            style = style
        }, props);
        public static Element Create<T, P>(uint key, Name name, ClassName className, Style style, RefAction<P> props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            className = className,
            style = style
        }, props);
        // Descriptor
        public static Element Create<T, P>(Descriptor descriptor) where T : RishElement<P>, new() where P : struct => Create<T, P>(descriptor, Defaults.GetValue<P>());
        public static Element Create<T, P>(Descriptor descriptor, RefAction<P> props) where T : RishElement<P>, new() where P : struct => Create<T, P>(descriptor, RefProps(props));
        public static Element Create<T, P>(Descriptor descriptor, P props) where T : RishElement<P>, new() where P : struct
        {
            var element = GetFromPool<RishDefinition<T, P>>();
            element.Factory(descriptor, props);
            
            OnCreate(element);

            return element;
        }





        // -------------------------------------------------------------------------------------------------------------
        // --- SETUPS --------------------------------------------------------------------------------------------------
        // -------------------------------------------------------------------------------------------------------------
        private class FunctionalDefinition : ElementDefinition
        {
            private FunctionElement Element { get; set; }
            
            public void Factory(Descriptor descriptor, FunctionElement function)
            {
                Descriptor = descriptor;
                Element = function;
            }

            public override Element New(Descriptor descriptor) => Rish.Create(Element, descriptor);

            public override void Invoke(Node node)
            {
                var (_, element) = node.AddChild<FunctionalElement>(Descriptor.key);
                
                if (element is IManualStyling customElement)
                {
                    customElement.OnName(Descriptor.name);
                    customElement.OnClasses(Descriptor.className);
                    customElement.OnInline(Descriptor.style);
                }
                else
                {
                    element.name = Descriptor.name;
                    Descriptor.className.SetClasses(element);
                    Descriptor.style.SetInlineStyle(element);
                }
                
                element.Delegate = Element;
            }

            public override bool Equals(ElementDefinition other)
            {
                return other is FunctionalDefinition otherDefinition && RishUtils.Compare<Descriptor>(Descriptor, otherDefinition.Descriptor) && Element == otherDefinition.Element;
            }
        }

        private class FunctionalDefinition<P> : ElementDefinition where P : struct
        {
            private FunctionElement<P> Element { get; set; }
            private P Props { get; set; }

            public void Factory(Descriptor descriptor, FunctionElement<P> function, P props)
            {
                Descriptor = descriptor;
                Element = function;
                Props = props;
            }

            public override Element New(Descriptor descriptor) => Rish.Create<P>(Element, descriptor, Copiers.Copy(Props));

            public override void Invoke(Node node)
            {
                var (_, element) = node.AddChild<FunctionalElement<P>>(Descriptor.key);
                
                if (element is IManualStyling customElement)
                {
                    customElement.OnName(Descriptor.name);
                    customElement.OnClasses(Descriptor.className);
                    customElement.OnInline(Descriptor.style);
                }
                else
                {
                    element.name = Descriptor.name;
                    Descriptor.className.SetClasses(element);
                    Descriptor.style.SetInlineStyle(element);
                }
                
                element.Delegate = Element;
                element.Props = Props;
            }

            public override bool Equals(ElementDefinition other)
            {
                return other is FunctionalDefinition<P> otherDefinition && RishUtils.Compare<Descriptor>(Descriptor, otherDefinition.Descriptor) && RishUtils.Compare<P>(Props, otherDefinition.Props) && Element == otherDefinition.Element;
            }
        }
        
        private class PrimitiveDefinition<T> : ElementDefinition where T : VisualElement, IPrimitiveElement, new()
        {
            private Children Children { get; set; }

            public void Factory(Descriptor descriptor, Children children)
            {
                Descriptor = descriptor;
                Children = children;
            }

            public override Element New(Descriptor descriptor) => Rish.Create<T>(descriptor, Children.Copy());

            public override void Invoke(Node node)
            {
                var (child, element) = node.AddChild<T>(Descriptor.key);
                
                if (element is IManualStyling customElement)
                {
                    customElement.OnName(Descriptor.name);
                    customElement.OnClasses(Descriptor.className);
                    customElement.OnInline(Descriptor.style);
                }
                else
                {
                    element.name = Descriptor.name;
                    Descriptor.className.SetClasses(element);
                    Descriptor.style.SetInlineStyle(element);
                }
                
                element.Setup();

                child.SetChildren(Children);
            }

            public override bool Equals(ElementDefinition other)
            {
                return other is PrimitiveDefinition<T> otherDefinition && RishUtils.Compare<Descriptor>(Descriptor, otherDefinition.Descriptor) && RishUtils.Compare<Children>(Children, otherDefinition.Children);
            }
        }

        private class PrimitiveDefinition<T, P> : ElementDefinition where T: VisualElement, IPrimitiveElement<P>, new() where P : struct
        {
            private P Props { get; set; }
            private Children Children { get; set; }

            public void Factory(Descriptor descriptor, P props, Children children)
            {
                Descriptor = descriptor;
                Props = props;
                Children = children;
            }

            public override Element New(Descriptor descriptor) => Rish.Create<T, P>(descriptor, Copiers.Copy(Props), Children.Copy());

            public override void Invoke(Node node)
            {
                var (child, element) = node.AddChild<T>(Descriptor.key);

                if (element is IManualStyling customElement)
                {
                    customElement.OnName(Descriptor.name);
                    customElement.OnClasses(Descriptor.className);
                    customElement.OnInline(Descriptor.style);
                }
                else
                {
                    element.name = Descriptor.name;
                    Descriptor.className.SetClasses(element);
                    Descriptor.style.SetInlineStyle(element);
                }
                
                element.Setup(Props);

                child.SetChildren(Children);
            }

            public override bool Equals(ElementDefinition other)
            {
                return other is PrimitiveDefinition<T, P> otherDefinition && RishUtils.Compare<Descriptor>(Descriptor, otherDefinition.Descriptor) && RishUtils.Compare<P>(Props, otherDefinition.Props) && RishUtils.Compare<Children>(Children, otherDefinition.Children);
            }
        }

        private class RishDefinition<T, P> : ElementDefinition where T : RishElement<P>, new() where P : struct
        {
            private P Props { get; set; }

            public void Factory(Descriptor descriptor, P props)
            {
                Descriptor = descriptor;
                Props = props;
            }

            public override Element New(Descriptor descriptor) => Rish.Create<T, P>(descriptor, Copiers.Copy(Props));

            public override void Invoke(Node node)
            {
                var (_, element) = node.AddChild<T>(Descriptor.key);
                
                if (element is IManualStyling customElement)
                {
                    customElement.OnName(Descriptor.name);
                    customElement.OnClasses(Descriptor.className);
                    customElement.OnInline(Descriptor.style);
                }
                else
                {
                    element.name = Descriptor.name;
                    Descriptor.className.SetClasses(element);
                    Descriptor.style.SetInlineStyle(element);
                }

                var props = Props;
                
                element.Props = props;
                
                // TODO: Add event to report visit (in case Render isn't triggered)
            }

            public override bool Equals(ElementDefinition other)
            {
                return other is RishDefinition<T, P> otherDefinition && RishUtils.Compare<Descriptor>(Descriptor, otherDefinition.Descriptor) && RishUtils.Compare<P>(Props, otherDefinition.Props);
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
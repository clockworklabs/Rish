using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI.v3
{
    public delegate void RefAction<T>(ref T value) where T : struct;

    public struct Element : IEquatable<Element>
    {
        private int id;

        public static Element Null => new ();

        internal void Invoke(Node node)
        {
            if (id <= 0)
            {
                return;
            }
            
            Rish.GetDefinition(id).Invoke(node);
        }
        
        public static implicit operator Element(ElementDefinition definition) => new()
        {
            id = definition.ID
        };
        
        bool IEquatable<Element>.Equals(Element other) => Equals(this, other);

        [Comparer]
        private static bool Equals(Element a, Element b)
        {
            var aSet = a.id > 0;
            var bSet = b.id > 0;
            if (aSet ^ bSet)
            {
                return false;
            }

            return !aSet || Rish.GetDefinition(a.id).Equals(Rish.GetDefinition(b.id));
        }
    }
    
    public abstract class ElementDefinition : IEquatable<ElementDefinition>
    {
        private static int _nextId;
        public int ID { get; } = ++_nextId;
        
        private Node _owner;
        internal Node Owner
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

        internal void Restart()
        {
            Owner = null;
        }

        // public abstract Descriptor GetDescriptor();
        // TODO: Create new element when changing descriptor

        public abstract void Invoke(Node node);

        public abstract bool Equals(ElementDefinition other);
    }

    public struct Descriptor : IEquatable<Descriptor>
    {
        public uint key;
        public Name name;
        public ClassList classList;
        public Style style;

        public static Descriptor Default => new();
        
        public Descriptor(Descriptor other)
        {
            key = other.key;
            name = other.name;
            classList = other.classList;
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
            private ClassList _classList;
        
            public static implicit operator Unmanaged(Descriptor managed) => new()
            {
                _key = managed.key,
                _name = managed.name,
                _classList = managed.classList
            };
        }
    }

    public struct Descriptor<P> : IEquatable<Descriptor<P>> where P : struct
    {
        public uint key;
        public Name name;
        public ClassList classList;
        public Style style;
        public P props;

        public static Descriptor<P> Default => new()
        {
            props = Defaults.GetValue<P>()
        };
        
        public Descriptor(Descriptor<P> other)
        {
            key = other.key;
            name = other.name;
            classList = other.classList;
            style = other.style;
            props = other.props;
        }
        
        bool IEquatable<Descriptor<P>>.Equals(Descriptor<P> other) => Equals(this, other);

        [Comparer]
        private static bool Equals(Descriptor<P> a, Descriptor<P> b)
        {
            return RishUtils.CompareUnmanaged<Unmanaged>(a, b) && Style.Equals(a.style, b.style) && RishUtils.Compare<P>(a.props, b.props);
        }

        private struct Unmanaged
        {
            private uint _key;
            private Name _name;
            private ClassList _classList;
        
            public static implicit operator Unmanaged(Descriptor<P> managed) => new()
            {
                _key = managed.key,
                _name = managed.name,
                _classList = managed.classList
            };
        }
    }

    public interface IOwner
    {
        void TakeOwnership(ElementDefinition definition);
        void TakeOwnership(NativeArray<Element> children);
    }

    public static class Rish
    {
        private static Stack<IOwner> Owners { get; } = new();
        
        private const int InitialPoolSize = 64;
        private static Dictionary<Type, Stack<ElementDefinition>> Pools { get; } = new();

        private static List<ElementDefinition> All { get; } = new();
        
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
                All.Add(element);
            }
            else
            {
                element = (T)pool.Pop();
            }

            element.Restart();
            
            return element;
        }

        internal static void ReturnToPool(ElementDefinition definition)
        {
            var type = definition.GetType();
            if (!Pools.TryGetValue(type, out var pool))
            {
                throw new UnityException($"{type} is an invalid element type. No pool found.");
            }
            
            pool.Push(definition);
        }

        internal static ElementDefinition GetDefinition(int id) => All[id - 1];

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
            
            owner.TakeOwnership(definition);
        }
        private static void OnCreate(NativeArray<Element> children)
        {
            var owner = Owners.Peek();
            if (owner == null)
            {
                throw new UnityException("There's nobody to claim ownership of this NativeArray<Element>");
            }
            
            owner.TakeOwnership(children);
        }
        
        public static Children Children(Element child0)
        {
            var array = new NativeArray<Element>(1, Allocator.Persistent);
            array[0] = child0;

            OnCreate(array);
            
            return array;
        }
        public static Children Children(Element child0, Element child1)
        {
            var array = new NativeArray<Element>(2, Allocator.Persistent);
            array[0] = child0;
            array[1] = child1;

            OnCreate(array);
            
            return array;
        }
        public static Children Children(Element child0, Element child1, Element child2)
        {
            var array = new NativeArray<Element>(3, Allocator.Persistent);
            array[0] = child0;
            array[1] = child1;
            array[2] = child2;

            OnCreate(array);
            
            return array;
        }
        public static Children Children(Element child0, Element child1, Element child2, Element child3)
        {
            var array = new NativeArray<Element>(4, Allocator.Persistent);
            array[0] = child0;
            array[1] = child1;
            array[2] = child2;
            array[3] = child3;

            OnCreate(array);
            
            return array;
        }
        public static Children Children(Element child0, Element child1, Element child2, Element child3, Element child4)
        {
            var array = new NativeArray<Element>(5, Allocator.Persistent);
            array[0] = child0;
            array[1] = child1;
            array[2] = child2;
            array[3] = child3;
            array[4] = child4;

            OnCreate(array);
            
            return array;
        }
        public static Children Children(params Element[] children)
        {
            var length = children.Length;
            var array = new NativeArray<Element>(length, Allocator.Persistent);
            for (var i = 0; i < length; i++)
            {
                array[i] = children[i];
            }

            OnCreate(array);
            
            return array;
        }
        public static Children Children(List<Element> children)
        {
            var length = children.Count;
            var array = new NativeArray<Element>(length, Allocator.Persistent);
            for (var i = 0; i < length; i++)
            {
                array[i] = children[i];
            }

            OnCreate(array);
            
            return array;
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
        public static Element Create(FunctionElement functionElement, ClassList classList) => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            classList = classList
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
        public static Element Create(FunctionElement functionElement, uint key, ClassList classList) => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            key = key,
            classList = classList
        });
        public static Element Create(FunctionElement functionElement, uint key, Style style) => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            key = key,
            style = style
        });
        public static Element Create(FunctionElement functionElement, Name name, ClassList classList) => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            name = name,
            classList = classList,
        });
        public static Element Create(FunctionElement functionElement, Name name, Style style) => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            name = name,
            style = style
        });
        public static Element Create(FunctionElement functionElement, ClassList classList, Style style) => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            classList = classList,
            style = style
        });
        // 3/4 -> 4
        public static Element Create(FunctionElement functionElement, uint key, Name name, ClassList classList) => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            classList = classList
        });
        public static Element Create(FunctionElement functionElement, uint key, Name name, Style style) => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            style = style
        });
        public static Element Create(FunctionElement functionElement, uint key, ClassList classList, Style style) => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            key = key,
            classList = classList,
            style = style
        });
        public static Element Create(FunctionElement functionElement, Name name, ClassList classList, Style style) => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            name = name,
            classList = classList,
            style = style
        });
        // 4/4 -> 1
        public static Element Create(FunctionElement functionElement, uint key, Name name, ClassList classList, Style style) => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            classList = classList,
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
        public static Element Create<P>(FunctionElement<P> functionElement) where P : struct => Create(functionElement, Descriptor<P>.Default);
        // 1/5 -> 5
        public static Element Create<P>(FunctionElement<P> functionElement, uint key) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key
        });
        public static Element Create<P>(FunctionElement<P> functionElement, Name name) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name
        });
        public static Element Create<P>(FunctionElement<P> functionElement, ClassList classList) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            classList = classList
        });
        public static Element Create<P>(FunctionElement<P> functionElement, Style style) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            style = style
        });
        public static Element Create<P>(FunctionElement<P> functionElement, P props) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            props = props
        });
        public static Element Create<P>(FunctionElement<P> functionElement, RefAction<P> props) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            props = RefProps(props)
        });
        // 2/5 -> 10
        public static Element Create<P>(FunctionElement<P> functionElement, uint key, Name name) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name
        });
        public static Element Create<P>(FunctionElement<P> functionElement, uint key, ClassList classList) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            classList = classList
        });
        public static Element Create<P>(FunctionElement<P> functionElement, uint key, Style style) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            style = style
        });
        public static Element Create<P>(FunctionElement<P> functionElement, uint key, P props) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            props = props
        });
        public static Element Create<P>(FunctionElement<P> functionElement, uint key, RefAction<P> props) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            props = RefProps(props)
        });
        public static Element Create<P>(FunctionElement<P> functionElement, Name name, ClassList classList) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            classList = classList
        });
        public static Element Create<P>(FunctionElement<P> functionElement, Name name, Style style) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            style = style
        });
        public static Element Create<P>(FunctionElement<P> functionElement, Name name, P props) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            props = props
        });
        public static Element Create<P>(FunctionElement<P> functionElement, Name name, RefAction<P> props) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            props = RefProps(props)
        });
        public static Element Create<P>(FunctionElement<P> functionElement, ClassList classList, Style style) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            classList = classList,
            style = style
        });
        public static Element Create<P>(FunctionElement<P> functionElement, ClassList classList, P props) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            classList = classList,
            props = props
        });
        public static Element Create<P>(FunctionElement<P> functionElement, ClassList classList, RefAction<P> props) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            classList = classList,
            props = RefProps(props)
        });
        public static Element Create<P>(FunctionElement<P> functionElement, Style style, P props) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            style = style,
            props = props
        });
        public static Element Create<P>(FunctionElement<P> functionElement, Style style, RefAction<P> props) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            style = style,
            props = RefProps(props)
        });
        // 3/5 -> 10
        public static Element Create<P>(FunctionElement<P> functionElement, uint key, Name name, ClassList classList) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            classList = classList
        });
        public static Element Create<P>(FunctionElement<P> functionElement, uint key, Name name, Style style) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            style = style
        });
        public static Element Create<P>(FunctionElement<P> functionElement, uint key, Name name, P props) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            props = props
        });
        public static Element Create<P>(FunctionElement<P> functionElement, uint key, Name name, RefAction<P> props) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            props = RefProps(props)
        });
        public static Element Create<P>(FunctionElement<P> functionElement, uint key, ClassList classList, Style style) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            classList = classList,
            style = style
        });
        public static Element Create<P>(FunctionElement<P> functionElement, uint key, ClassList classList, P props) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            classList = classList,
            props = props
        });
        public static Element Create<P>(FunctionElement<P> functionElement, uint key, ClassList classList, RefAction<P> props) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            classList = classList,
            props = RefProps(props)
        });
        public static Element Create<P>(FunctionElement<P> functionElement, uint key, Style style, P props) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            style = style,
            props = props
        });
        public static Element Create<P>(FunctionElement<P> functionElement, uint key, Style style, RefAction<P> props) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            style = style,
            props = RefProps(props)
        });
        public static Element Create<P>(FunctionElement<P> functionElement, Name name, ClassList classList, Style style) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            classList = classList,
            style = style
        });
        public static Element Create<P>(FunctionElement<P> functionElement, Name name, ClassList classList, P props) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            classList = classList,
            props = props
        });
        public static Element Create<P>(FunctionElement<P> functionElement, Name name, ClassList classList, RefAction<P> props) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            classList = classList,
            props = RefProps(props)
        });
        public static Element Create<P>(FunctionElement<P> functionElement, Name name, Style style, P props) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            style = style,
            props = props
        });
        public static Element Create<P>(FunctionElement<P> functionElement, Name name, Style style, RefAction<P> props) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            style = style,
            props = RefProps(props)
        });
        public static Element Create<P>(FunctionElement<P> functionElement, ClassList classList, Style style, P props) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            classList = classList,
            style = style,
            props = props
        });
        public static Element Create<P>(FunctionElement<P> functionElement, ClassList classList, Style style, RefAction<P> props) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            classList = classList,
            style = style,
            props = RefProps(props)
        });
        // 4/5 -> 5
        public static Element Create<P>(FunctionElement<P> functionElement, uint key, Name name, ClassList classList, Style style) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            classList = classList,
            style = style
        });
        public static Element Create<P>(FunctionElement<P> functionElement, uint key, Name name, ClassList classList, P props) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            classList = classList,
            props = props
        });
        public static Element Create<P>(FunctionElement<P> functionElement, uint key, Name name, ClassList classList, RefAction<P> props) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            classList = classList,
            props = RefProps(props)
        });
        public static Element Create<P>(FunctionElement<P> functionElement, uint key, Name name, Style style, P props) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            style = style,
            props = props
        });
        public static Element Create<P>(FunctionElement<P> functionElement, uint key, Name name, Style style, RefAction<P> props) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            style = style,
            props = RefProps(props)
        });
        public static Element Create<P>(FunctionElement<P> functionElement, uint key, ClassList classList, Style style, P props) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            classList = classList,
            style = style,
            props = props
        });
        public static Element Create<P>(FunctionElement<P> functionElement, uint key, ClassList classList, Style style, RefAction<P> props) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            classList = classList,
            style = style,
            props = RefProps(props)
        });
        public static Element Create<P>(FunctionElement<P> functionElement, Name name, ClassList classList, Style style, P props) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            classList = classList,
            style = style,
            props = props
        });
        public static Element Create<P>(FunctionElement<P> functionElement, Name name, ClassList classList, Style style, RefAction<P> props) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            classList = classList,
            style = style,
            props = RefProps(props)
        });
        // 5/5 -> 1
        public static Element Create<P>(FunctionElement<P> functionElement, uint key, Name name, ClassList classList, Style style, P props) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            classList = classList,
            style = style,
            props = props
        });
        public static Element Create<P>(FunctionElement<P> functionElement, uint key, Name name, ClassList classList, Style style, RefAction<P> props) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            classList = classList,
            style = style,
            props = RefProps(props)
        });
        // Descriptor
        public static Element Create<P>(FunctionElement<P> functionElement, Descriptor<P> descriptor) where P : struct
        {
            var element = GetFromPool<FunctionalDefinition<P>>();
            element.Factory(descriptor, functionElement);
            
            OnCreate(element);

            return element;
        }
        
        
        
        
        
        // -------------------------------------------------------------------------------------------------------------
        // --- NATIVE ELEMENTS -----------------------------------------------------------------------------------------
        // -------------------------------------------------------------------------------------------------------------
        
        // 0/4 -> 1
        public static Element Create<T>(Children? children = null) where T : VisualElement, INativeElement, new() => Create<T>(Descriptor.Default, children);
        // 1/4 -> 4
        public static Element Create<T>(uint key, Children? children = null) where T : VisualElement, INativeElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            key = key
        }, children);
        public static Element Create<T>(Name name, Children? children = null) where T : VisualElement, INativeElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            name = name
        }, children);
        public static Element Create<T>(ClassList classList, Children? children = null) where T : VisualElement, INativeElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            classList = classList
        }, children);
        public static Element Create<T>(Style style, Children? children = null) where T : VisualElement, INativeElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            style = style
        }, children);
        // 2/4 -> 6
        public static Element Create<T>(uint key, Name name, Children? children = null) where T : VisualElement, INativeElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name
        }, children);
        public static Element Create<T>(uint key, ClassList classList, Children? children = null) where T : VisualElement, INativeElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            key = key,
            classList = classList
        }, children);
        public static Element Create<T>(uint key, Style style, Children? children = null) where T : VisualElement, INativeElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            key = key,
            style = style
        }, children);
        public static Element Create<T>(Name name, ClassList classList, Children? children = null) where T : VisualElement, INativeElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            name = name,
            classList = classList,
        }, children);
        public static Element Create<T>(Name name, Style style, Children? children = null) where T : VisualElement, INativeElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            name = name,
            style = style
        }, children);
        public static Element Create<T>(ClassList classList, Style style, Children? children = null) where T : VisualElement, INativeElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            classList = classList,
            style = style
        }, children);
        // 3/4 -> 4
        public static Element Create<T>(uint key, Name name, ClassList classList, Children? children = null) where T : VisualElement, INativeElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            classList = classList
        }, children);
        public static Element Create<T>(uint key, Name name, Style style, Children? children = null) where T : VisualElement, INativeElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            style = style
        }, children);
        public static Element Create<T>(uint key, ClassList classList, Style style, Children? children = null) where T : VisualElement, INativeElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            key = key,
            classList = classList,
            style = style
        }, children);
        public static Element Create<T>(Name name, ClassList classList, Style style, Children? children = null) where T : VisualElement, INativeElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            name = name,
            classList = classList,
            style = style
        }, children);
        // 4/4 -> 1
        public static Element Create<T>(uint key, Name name, ClassList classList, Style style, Children? children = null) where T : VisualElement, INativeElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            classList = classList,
            style = style
        }, children);
        // Descriptor
        public static Element Create<T>(Descriptor descriptor, Children? children = null) where T : VisualElement, INativeElement, new()
        {
            var element = GetFromPool<NativeDefinition<T>>();
            element.Factory(descriptor, children ?? v3.Children.Empty);
            
            OnCreate(element);

            return element;
        }



        // 0/5 -> 1
        public static Element Create<T, P>(Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(Descriptor<P>.Default, children);
        // 1/5 -> 5
        public static Element Create<T, P>(uint key, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key
        }, children);
        public static Element Create<T, P>(Name name, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name
        }, children);
        public static Element Create<T, P>(ClassList classList, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            classList = classList
        }, children);
        public static Element Create<T, P>(Style style, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            style = style
        }, children);
        public static Element Create<T, P>(P props, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            props = props
        }, children);
        public static Element Create<T, P>(RefAction<P> props, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            props = RefProps(props)
        }, children);
        // 2/5 = 10
        public static Element Create<T, P>(uint key, Name name, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name
        }, children);
        public static Element Create<T, P>(uint key, ClassList classList, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            classList = classList
        }, children);
        public static Element Create<T, P>(uint key, Style style, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            style = style
        }, children);
        public static Element Create<T, P>(uint key, P props, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            props = props
        }, children);
        public static Element Create<T, P>(uint key, RefAction<P> props, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            props = RefProps(props)
        }, children);
        public static Element Create<T, P>(Name name, ClassList classList, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            classList = classList
        }, children);
        public static Element Create<T, P>(Name name, Style style, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            style = style
        }, children);
        public static Element Create<T, P>(Name name, P props, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            props = props
        }, children);
        public static Element Create<T, P>(Name name, RefAction<P> props, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            props = RefProps(props)
        }, children);
        public static Element Create<T, P>(ClassList classList, Style style, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            classList = classList,
            style = style
        }, children);
        public static Element Create<T, P>(ClassList classList, P props, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            classList = classList,
            props = props
        }, children);
        public static Element Create<T, P>(ClassList classList, RefAction<P> props, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            classList = classList,
            props = RefProps(props)
        }, children);
        public static Element Create<T, P>(Style style, P props, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            style = style,
            props = props
        }, children);
        public static Element Create<T, P>(Style style, RefAction<P> props, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            style = style,
            props = RefProps(props)
        }, children);
        // 3/5 = 10
        public static Element Create<T, P>(uint key, Name name, ClassList classList, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            classList = classList
        }, children);
        public static Element Create<T, P>(uint key, Name name, Style style, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            style = style
        }, children);
        public static Element Create<T, P>(uint key, Name name, P props, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            props = props
        }, children);
        public static Element Create<T, P>(uint key, Name name, RefAction<P> props, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            props = RefProps(props)
        }, children);
        public static Element Create<T, P>(uint key, ClassList classList, Style style, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            classList = classList,
            style = style
        }, children);
        public static Element Create<T, P>(uint key, ClassList classList, P props, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            classList = classList,
            props = props
        }, children);
        public static Element Create<T, P>(uint key, ClassList classList, RefAction<P> props, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            classList = classList,
            props = RefProps(props)
        }, children);
        public static Element Create<T, P>(uint key, Style style, P props, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            style = style,
            props = props
        }, children);
        public static Element Create<T, P>(uint key, Style style, RefAction<P> props, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            style = style,
            props = RefProps(props)
        }, children);
        public static Element Create<T, P>(Name name, ClassList classList, Style style, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            classList = classList,
            style = style
        }, children);
        public static Element Create<T, P>(Name name, ClassList classList, P props, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            classList = classList,
            props = props
        }, children);
        public static Element Create<T, P>(Name name, ClassList classList, RefAction<P> props, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            classList = classList,
            props = RefProps(props)
        }, children);
        public static Element Create<T, P>(Name name, Style style, P props, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            style = style,
            props = props
        }, children);
        public static Element Create<T, P>(Name name, Style style, RefAction<P> props, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            style = style,
            props = RefProps(props)
        }, children);
        public static Element Create<T, P>(ClassList classList, Style style, P props, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            classList = classList,
            style = style,
            props = props
        }, children);
        public static Element Create<T, P>(ClassList classList, Style style, RefAction<P> props, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            classList = classList,
            style = style,
            props = RefProps(props)
        }, children);
        // 4/5 = 5
        public static Element Create<T, P>(uint key, Name name, ClassList classList, Style style, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            classList = classList,
            style = style
        }, children);
        public static Element Create<T, P>(uint key, Name name, ClassList classList, P props, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            classList = classList,
            props = props
        }, children);
        public static Element Create<T, P>(uint key, Name name, ClassList classList, RefAction<P> props, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            classList = classList,
            props = RefProps(props)
        }, children);
        public static Element Create<T, P>(uint key, Name name, Style style, P props, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            style = style,
            props = props
        }, children);
        public static Element Create<T, P>(uint key, Name name, Style style, RefAction<P> props, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            style = style,
            props = RefProps(props)
        }, children);
        public static Element Create<T, P>(uint key, ClassList classList, Style style, P props, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            classList = classList,
            style = style,
            props = props
        }, children);
        public static Element Create<T, P>(uint key, ClassList classList, Style style, RefAction<P> props, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            classList = classList,
            style = style,
            props = RefProps(props)
        }, children);
        public static Element Create<T, P>(Name name, ClassList classList, Style style, P props, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            classList = classList,
            style = style,
            props = props
        }, children);
        public static Element Create<T, P>(Name name, ClassList classList, Style style, RefAction<P> props, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            classList = classList,
            style = style,
            props = RefProps(props)
        }, children);
        // 5/5 = 1
        public static Element Create<T, P>(uint key, Name name, ClassList classList, Style style, P props, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            classList = classList,
            style = style,
            props = props
        }, children);
        public static Element Create<T, P>(uint key, Name name, ClassList classList, Style style, RefAction<P> props, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            classList = classList,
            style = style,
            props = RefProps(props)
        }, children);
        // Descriptor
        public static Element Create<T, P>(Descriptor<P> descriptor, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct
        {
            var element = GetFromPool<NativeDefinition<T, P>>();
            element.Factory(descriptor, children ?? v3.Children.Empty);
            
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
        public static Element Create<T>(ClassList classList) where T : RishElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            classList = classList
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
        public static Element Create<T>(uint key, ClassList classList) where T : RishElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            key = key,
            classList = classList
        });
        public static Element Create<T>(uint key, Style style) where T : RishElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            key = key,
            style = style
        });
        public static Element Create<T>(Name name, ClassList classList) where T : RishElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            name = name,
            classList = classList
        });
        public static Element Create<T>(Name name, Style style) where T : RishElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            name = name,
            style = style
        });
        public static Element Create<T>(ClassList classList, Style style) where T : RishElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            classList = classList,
            style = style
        });
        // 3/4 -> 4
        public static Element Create<T>(uint key, Name name, ClassList classList) where T : RishElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            classList = classList
        });
        public static Element Create<T>(uint key, Name name, Style style) where T : RishElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            style = style
        });
        public static Element Create<T>(uint key, ClassList classList, Style style) where T : RishElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            key = key,
            classList = classList,
            style = style
        });
        public static Element Create<T>(Name name, ClassList classList, Style style) where T : RishElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            name = name,
            classList = classList,
            style = style
        });
        // 4/4 -> 1
        public static Element Create<T>(uint key, Name name, ClassList classList, Style style) where T : RishElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            classList = classList,
            style = style
        });
        // Descriptor
        public static Element Create<T>(Descriptor descriptor) where T : RishElement, new()
        {
            var element = GetFromPool<RishDefinition<T>>();
            element.Factory(descriptor);
            
            OnCreate(element);

            return element;
        }

        // 0/5 -> 1
        public static Element Create<T, P>() where T : RishElement<P>, new() where P : struct => Create<T, P>(Descriptor<P>.Default);
        // 1/5 -> 5
        public static Element Create<T, P>(uint key) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key
        });
        public static Element Create<T, P>(Name name) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name
        });
        public static Element Create<T, P>(ClassList classList) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            classList = classList
        });
        public static Element Create<T, P>(Style style) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            style = style
        });
        public static Element Create<T, P>(P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            props = props
        });
        public static Element Create<T, P>(RefAction<P> props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            props = RefProps(props)
        });
        // 2/5 = 10
        public static Element Create<T, P>(uint key, Name name) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name
        });
        public static Element Create<T, P>(uint key, ClassList classList) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            classList = classList
        });
        public static Element Create<T, P>(uint key, Style style) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            style = style
        });
        public static Element Create<T, P>(uint key, P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            props = props
        });
        public static Element Create<T, P>(uint key, RefAction<P> props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            props = RefProps(props)
        });
        public static Element Create<T, P>(Name name, ClassList classList) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            classList = classList
        });
        public static Element Create<T, P>(Name name, Style style) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            style = style
        });
        public static Element Create<T, P>(Name name, P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            props = props
        });
        public static Element Create<T, P>(Name name, RefAction<P> props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            props = RefProps(props)
        });
        public static Element Create<T, P>(ClassList classList, Style style) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            classList = classList,
            style = style
        });
        public static Element Create<T, P>(ClassList classList, P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            classList = classList,
            props = props
        });
        public static Element Create<T, P>(ClassList classList, RefAction<P> props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            classList = classList,
            props = RefProps(props)
        });
        public static Element Create<T, P>(Style style, P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            style = style,
            props = props
        });
        public static Element Create<T, P>(Style style, RefAction<P> props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            style = style,
            props = RefProps(props)
        });
        // 3/5 = 10
        public static Element Create<T, P>(uint key, Name name, ClassList classList) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            classList = classList
        });
        public static Element Create<T, P>(uint key, Name name, Style style) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            style = style
        });
        public static Element Create<T, P>(uint key, Name name, P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            props = props
        });
        public static Element Create<T, P>(uint key, Name name, RefAction<P> props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            props = RefProps(props)
        });
        public static Element Create<T, P>(uint key, ClassList classList, Style style) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            classList = classList,
            style = style
        });
        public static Element Create<T, P>(uint key, ClassList classList, P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            classList = classList,
            props = props
        });
        public static Element Create<T, P>(uint key, ClassList classList, RefAction<P> props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            classList = classList,
            props = RefProps(props)
        });
        public static Element Create<T, P>(uint key, Style style, P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            style = style,
            props = props
        });
        public static Element Create<T, P>(uint key, Style style, RefAction<P> props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            style = style,
            props = RefProps(props)
        });
        public static Element Create<T, P>(Name name, ClassList classList, Style style) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            classList = classList,
            style = style
        });
        public static Element Create<T, P>(Name name, ClassList classList, P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            classList = classList,
            props = props
        });
        public static Element Create<T, P>(Name name, ClassList classList, RefAction<P> props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            classList = classList,
            props = RefProps(props)
        });
        public static Element Create<T, P>(Name name, Style style, P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            style = style,
            props = props
        });
        public static Element Create<T, P>(Name name, Style style, RefAction<P> props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            style = style,
            props = RefProps(props)
        });
        public static Element Create<T, P>(ClassList classList, Style style, P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            classList = classList,
            style = style,
            props = props
        });
        public static Element Create<T, P>(ClassList classList, Style style, RefAction<P> props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            classList = classList,
            style = style,
            props = RefProps(props)
        });
        // 4/5 = 5
        public static Element Create<T, P>(uint key, Name name, ClassList classList, Style style) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            classList = classList,
            style = style
        });
        public static Element Create<T, P>(uint key, Name name, ClassList classList, P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            classList = classList,
            props = props
        });
        public static Element Create<T, P>(uint key, Name name, ClassList classList, RefAction<P> props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            classList = classList,
            props = RefProps(props)
        });
        public static Element Create<T, P>(uint key, Name name, Style style, P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            style = style,
            props = props
        });
        public static Element Create<T, P>(uint key, Name name, Style style, RefAction<P> props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            style = style,
            props = RefProps(props)
        });
        public static Element Create<T, P>(uint key, ClassList classList, Style style, P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            classList = classList,
            style = style,
            props = props
        });
        public static Element Create<T, P>(uint key, ClassList classList, Style style, RefAction<P> props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            classList = classList,
            style = style,
            props = RefProps(props)
        });
        public static Element Create<T, P>(Name name, ClassList classList, Style style, P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            classList = classList,
            style = style,
            props = props
        });
        public static Element Create<T, P>(Name name, ClassList classList, Style style, RefAction<P> props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            classList = classList,
            style = style,
            props = RefProps(props)
        });
        // 5/5 = 1
        public static Element Create<T, P>(uint key, Name name, ClassList classList, Style style, P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            classList = classList,
            style = style,
            props = props
        });
        public static Element Create<T, P>(uint key, Name name, ClassList classList, Style style, RefAction<P> props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            classList = classList,
            style = style,
            props = RefProps(props)
        });
        // Descriptor
        public static Element Create<T, P>(Descriptor<P> descriptor) where T : RishElement<P>, new() where P : struct
        {
            var element = GetFromPool<RishDefinition<T, P>>();
            element.Factory(descriptor);
            
            OnCreate(element);

            return element;
        }





        // -------------------------------------------------------------------------------------------------------------
        // --- SETUPS --------------------------------------------------------------------------------------------------
        // -------------------------------------------------------------------------------------------------------------
        private class FunctionalDefinition : ElementDefinition
        {
            private Descriptor Descriptor { get; set; }
            private FunctionElement Element { get; set; }
            
            public void Factory(Descriptor descriptor, FunctionElement function)
            {
                Descriptor = descriptor;
                Element = function;
            }

            public override void Invoke(Node node)
            {
                var (_, element) = node.AddChild<FunctionalElement>(Descriptor.key);

                element.name = Descriptor.name;
                
                Descriptor.classList.SetClasses(element);
                Descriptor.style.SetInlineStyle(element);
                
                element.Delegate = Element;
            }

            public override bool Equals(ElementDefinition other)
            {
                return other is FunctionalDefinition otherDefinition && RishUtils.Compare<Descriptor>(Descriptor, otherDefinition.Descriptor) && Element == otherDefinition.Element;
            }
        }

        private class FunctionalDefinition<P> : ElementDefinition where P : struct
        {
            private Descriptor<P> Descriptor { get; set; }
            private FunctionElement<P> Element { get; set; }

            public void Factory(Descriptor<P> descriptor, FunctionElement<P> function)
            {
                Descriptor = descriptor;
                Element = function;
            }

            public override void Invoke(Node node)
            {
                var (_, element) = node.AddChild<FunctionalElement<P>>(Descriptor.key);

                element.name = Descriptor.name;
                
                Descriptor.classList.SetClasses(element);
                Descriptor.style.SetInlineStyle(element);
                
                element.Delegate = Element;
                element.Props = Descriptor.props;
            }

            public override bool Equals(ElementDefinition other)
            {
                return other is FunctionalDefinition<P> otherDefinition && RishUtils.Compare<Descriptor<P>>(Descriptor, otherDefinition.Descriptor) && Element == otherDefinition.Element;
            }
        }
        
        private class NativeDefinition<T> : ElementDefinition where T : VisualElement, INativeElement, new()
        {
            private Descriptor Descriptor { get; set; }
            private Children Children { get; set; }

            public void Factory(Descriptor descriptor, Children children)
            {
                Descriptor = descriptor;
                Children = children;
            }

            public override void Invoke(Node node)
            {
                var (child, element) = node.AddChild<T>(Descriptor.key);

                element.name = Descriptor.name;
                
                Descriptor.classList.SetClasses(element);
                Descriptor.style.SetInlineStyle(element);
                
                element.Setup();

                child.SetChildren(Children);
            }

            public override bool Equals(ElementDefinition other)
            {
                return other is NativeDefinition<T> otherDefinition && RishUtils.Compare<Descriptor>(Descriptor, otherDefinition.Descriptor) && RishUtils.Compare<Children>(Children, otherDefinition.Children);
            }
        }

        private class NativeDefinition<T, P> : ElementDefinition where T: VisualElement, INativeElement<P>, new() where P : struct
        {
            private Descriptor<P> Descriptor { get; set; }
            private Children Children { get; set; }

            public void Factory(Descriptor<P> descriptor, Children children)
            {
                Descriptor = descriptor;
                Children = children;
            }

            public override void Invoke(Node node)
            {
                var (child, element) = node.AddChild<T>(Descriptor.key);

                element.name = Descriptor.name;
                
                Descriptor.classList.SetClasses(element);
                Descriptor.style.SetInlineStyle(element);
                
                element.Setup(Descriptor.props);

                child.SetChildren(Children);
            }

            public override bool Equals(ElementDefinition other)
            {
                return other is NativeDefinition<T, P> otherDefinition && RishUtils.Compare<Descriptor<P>>(Descriptor, otherDefinition.Descriptor) && RishUtils.Compare<Children>(Children, otherDefinition.Children);
            }
        }

        private class RishDefinition<T> : ElementDefinition where T : RishElement, new()
        {
            private Descriptor Descriptor { get; set; }

            public void Factory(Descriptor descriptor)
            {
                Descriptor = descriptor;
            }

            public override void Invoke(Node node)
            {
                var (_, element) = node.AddChild<T>(Descriptor.key);

                element.name = Descriptor.name;
                
                Descriptor.classList.SetClasses(element);
                Descriptor.style.SetInlineStyle(element);
            }

            public override bool Equals(ElementDefinition other)
            {
                return other is RishDefinition<T> otherDefinition && RishUtils.Compare<Descriptor>(Descriptor, otherDefinition.Descriptor);
            }
        }

        private class RishDefinition<T, P> : ElementDefinition where T : RishElement<P>, new() where P : struct
        {
            private Descriptor<P> Descriptor { get; set; }

            public void Factory(Descriptor<P> descriptor)
            {
                Descriptor = descriptor;
            }

            public override void Invoke(Node node)
            {
                var (_, element) = node.AddChild<T>(Descriptor.key);

                element.name = Descriptor.name;
                
                Descriptor.classList.SetClasses(element);
                Descriptor.style.SetInlineStyle(element);
                
                element.Props = Descriptor.props;
            }

            public override bool Equals(ElementDefinition other)
            {
                return other is RishDefinition<T, P> otherDefinition && RishUtils.Compare<Descriptor<P>>(Descriptor, otherDefinition.Descriptor);
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
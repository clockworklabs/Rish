using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI.v3
{
    public delegate void RefAction<T>(ref T value) where T : struct;

    public struct Element
    {
        private int id;

        public static Element Null => new ();

        internal void Invoke(Node node)
        {
            if (id <= 0)
            {
                return;
            }
            
            Rish.GetDefinition(id)?.Invoke(node);
        }
        
        public static implicit operator Element(ElementDefinition definition) => new()
        {
            id = definition.ID
        };
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

    public struct Descriptor
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
    }

    public struct Descriptor<P> where P : struct
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
        
        public static implicit operator Descriptor(Descriptor<P> descriptor) => new()
        {
            key = descriptor.key,
            name = descriptor.name,
            classList = descriptor.classList,
            style = descriptor.style
        };
    }

    public static class Rish
    {
        private static Stack<Action<ElementDefinition>> OnCreateCallbacks { get; } = new();
        
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

        internal static void SubscribeToElementCreation(Action<ElementDefinition> callback)
        {
            OnCreateCallbacks.Push(callback);
        }

        internal static void UnsubscribeFromElementCreation(Action<ElementDefinition> callback)
        {
            if (OnCreateCallbacks.Peek() != callback)
            {
                throw new UnityException("You're not the current subscriber. This should never happen.");
            }
            
            OnCreateCallbacks.Pop();
        }

        private static void OnCreate(ElementDefinition definition)
        {
            OnCreateCallbacks.Peek()?.Invoke(definition);
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
            var element = GetFromPool<FunctionDefinition>();
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
            var element = GetFromPool<FunctionDefinition<P>>();
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
            element.Factory(descriptor, children ?? Children.Empty);
            
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
            element.Factory(descriptor, children ?? Children.Empty);
            
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
        private class FunctionDefinition : ElementDefinition
        {
            private Descriptor Descriptor { get; set; }
            private FunctionElement Function { get; set; }
            
            public void Factory(Descriptor descriptor, FunctionElement function)
            {
                Descriptor = descriptor;
                Function = function;
            }

            public override void Invoke(Node node)
            {
                var (_, element) = node.AddChild<AnonymousElement>(Descriptor.key);

                element.name = Descriptor.name;
                
                Descriptor.classList.SetClasses(element);
                Descriptor.style.SetInlineStyle(element);
                
                element.Delegate = Function;
            }

            public override bool Equals(ElementDefinition other)
            {
                return false;
            }
        }

        private class FunctionDefinition<P> : ElementDefinition where P : struct
        {
            private Descriptor<P> Descriptor { get; set; }
            private FunctionElement<P> Function { get; set; }

            public void Factory(Descriptor<P> descriptor, FunctionElement<P> function)
            {
                Descriptor = descriptor;
                Function = function;
            }

            public override void Invoke(Node node)
            {
                var (_, element) = node.AddChild<AnonymousElement<P>>(Descriptor.key);

                element.name = Descriptor.name;
                
                Descriptor.classList.SetClasses(element);
                Descriptor.style.SetInlineStyle(element);
                
                element.Delegate = Function;
                element.Props = Descriptor.props;
            }

            public override bool Equals(ElementDefinition other)
            {
                return false;
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
                return false;
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
                return false;
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
                return false;
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
                return false;
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
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI.v3
{
    public delegate void RefAction<T>(ref T value) where T : struct;

    public struct ElementDefinition
    {
        private int id;

        public static ElementDefinition Null => new ();
        
        internal ElementSetup GetSetup() => id > 0 ? Rish.GetSetup(id) : null;
        
        public static implicit operator ElementDefinition(ElementSetup setup) => new()
        {
            id = setup.ID
        };
    }
    
    public abstract class ElementSetup : IEquatable<ElementSetup>
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

        public abstract bool Equals(ElementSetup other);
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
        private static Stack<Action<ElementSetup>> OnCreateCallbacks { get; } = new();
        
        private const int InitialPoolSize = 64;
        private static Dictionary<Type, Stack<ElementSetup>> Pools { get; } = new();

        private static List<ElementSetup> AllSetups { get; } = new();
        
        private static T GetElement<T>() where T : ElementSetup, new()
        {
            var type = typeof(T);
            if (!Pools.TryGetValue(type, out var pool))
            {
                pool = new Stack<ElementSetup>(InitialPoolSize);
                Pools[type] = pool;
            }

            T element;
            if (pool.Count < 1)
            {
                element = new T();
                AllSetups.Add(element);
            }
            else
            {
                element = (T)pool.Pop();
            }

            element.Restart();
            
            return element;
        }

        internal static void ReturnToPool(ElementSetup elementSetup)
        {
            var type = elementSetup.GetType();
            if (!Pools.TryGetValue(type, out var pool))
            {
                throw new UnityException($"{type} is an invalid element type. No pool found.");
            }
            
            pool.Push(elementSetup);
        }

        internal static ElementSetup GetSetup(int id) => AllSetups[id - 1];

        internal static void SubscribeToElementCreation(Action<ElementSetup> callback)
        {
            OnCreateCallbacks.Push(callback);
        }

        internal static void UnsubscribeFromElementCreation(Action<ElementSetup> callback)
        {
            if (OnCreateCallbacks.Peek() != callback)
            {
                throw new UnityException("You're not the current subscriber. This should never happen.");
            }
            
            OnCreateCallbacks.Pop();
        }

        private static void OnCreate(ElementSetup setup)
        {
            OnCreateCallbacks.Peek()?.Invoke(setup);
        }
        
        // -------------------------------------------------------------------------------------------------------------
        // --- FUNCTION ELEMENTS ---------------------------------------------------------------------------------------
        // -------------------------------------------------------------------------------------------------------------
        
        // 0/4 -> 1
        public static ElementDefinition Create(FunctionElement functionElement) => Create(functionElement, Descriptor.Default);
        // 1/4 -> 4
        public static ElementDefinition Create(FunctionElement functionElement, uint key) => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            key = key
        });
        public static ElementDefinition Create(FunctionElement functionElement, Name name) => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            name = name
        });
        public static ElementDefinition Create(FunctionElement functionElement, ClassList classList) => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            classList = classList
        });
        public static ElementDefinition Create(FunctionElement functionElement, Style style) => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            style = style
        });
        // 2/4 -> 6
        public static ElementDefinition Create(FunctionElement functionElement, uint key, Name name) => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name
        });
        public static ElementDefinition Create(FunctionElement functionElement, uint key, ClassList classList) => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            key = key,
            classList = classList
        });
        public static ElementDefinition Create(FunctionElement functionElement, uint key, Style style) => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            key = key,
            style = style
        });
        public static ElementDefinition Create(FunctionElement functionElement, Name name, ClassList classList) => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            name = name,
            classList = classList,
        });
        public static ElementDefinition Create(FunctionElement functionElement, Name name, Style style) => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            name = name,
            style = style
        });
        public static ElementDefinition Create(FunctionElement functionElement, ClassList classList, Style style) => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            classList = classList,
            style = style
        });
        // 3/4 -> 4
        public static ElementDefinition Create(FunctionElement functionElement, uint key, Name name, ClassList classList) => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            classList = classList
        });
        public static ElementDefinition Create(FunctionElement functionElement, uint key, Name name, Style style) => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            style = style
        });
        public static ElementDefinition Create(FunctionElement functionElement, uint key, ClassList classList, Style style) => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            key = key,
            classList = classList,
            style = style
        });
        public static ElementDefinition Create(FunctionElement functionElement, Name name, ClassList classList, Style style) => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            name = name,
            classList = classList,
            style = style
        });
        // 4/4 -> 1
        public static ElementDefinition Create(FunctionElement functionElement, uint key, Name name, ClassList classList, Style style) => Create(functionElement, new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            classList = classList,
            style = style
        });
        // Descriptor
        public static ElementDefinition Create(FunctionElement functionElement, Descriptor descriptor)
        {
            var element = GetElement<FunctionSetup>();
            element.Factory(descriptor, functionElement);
            
            OnCreate(element);

            return element;
        }
        
        
        
        // 0/5 -> 1
        public static ElementDefinition Create<P>(FunctionElement<P> functionElement) where P : struct => Create(functionElement, Descriptor<P>.Default);
        // 1/5 -> 5
        public static ElementDefinition Create<P>(FunctionElement<P> functionElement, uint key) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key
        });
        public static ElementDefinition Create<P>(FunctionElement<P> functionElement, Name name) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name
        });
        public static ElementDefinition Create<P>(FunctionElement<P> functionElement, ClassList classList) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            classList = classList
        });
        public static ElementDefinition Create<P>(FunctionElement<P> functionElement, Style style) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            style = style
        });
        public static ElementDefinition Create<P>(FunctionElement<P> functionElement, P props) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            props = props
        });
        public static ElementDefinition Create<P>(FunctionElement<P> functionElement, RefAction<P> props) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            props = RefProps(props)
        });
        // 2/5 -> 10
        public static ElementDefinition Create<P>(FunctionElement<P> functionElement, uint key, Name name) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name
        });
        public static ElementDefinition Create<P>(FunctionElement<P> functionElement, uint key, ClassList classList) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            classList = classList
        });
        public static ElementDefinition Create<P>(FunctionElement<P> functionElement, uint key, Style style) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            style = style
        });
        public static ElementDefinition Create<P>(FunctionElement<P> functionElement, uint key, P props) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            props = props
        });
        public static ElementDefinition Create<P>(FunctionElement<P> functionElement, uint key, RefAction<P> props) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            props = RefProps(props)
        });
        public static ElementDefinition Create<P>(FunctionElement<P> functionElement, Name name, ClassList classList) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            classList = classList
        });
        public static ElementDefinition Create<P>(FunctionElement<P> functionElement, Name name, Style style) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            style = style
        });
        public static ElementDefinition Create<P>(FunctionElement<P> functionElement, Name name, P props) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            props = props
        });
        public static ElementDefinition Create<P>(FunctionElement<P> functionElement, Name name, RefAction<P> props) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            props = RefProps(props)
        });
        public static ElementDefinition Create<P>(FunctionElement<P> functionElement, ClassList classList, Style style) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            classList = classList,
            style = style
        });
        public static ElementDefinition Create<P>(FunctionElement<P> functionElement, ClassList classList, P props) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            classList = classList,
            props = props
        });
        public static ElementDefinition Create<P>(FunctionElement<P> functionElement, ClassList classList, RefAction<P> props) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            classList = classList,
            props = RefProps(props)
        });
        public static ElementDefinition Create<P>(FunctionElement<P> functionElement, Style style, P props) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            style = style,
            props = props
        });
        public static ElementDefinition Create<P>(FunctionElement<P> functionElement, Style style, RefAction<P> props) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            style = style,
            props = RefProps(props)
        });
        // 3/5 -> 10
        public static ElementDefinition Create<P>(FunctionElement<P> functionElement, uint key, Name name, ClassList classList) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            classList = classList
        });
        public static ElementDefinition Create<P>(FunctionElement<P> functionElement, uint key, Name name, Style style) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            style = style
        });
        public static ElementDefinition Create<P>(FunctionElement<P> functionElement, uint key, Name name, P props) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            props = props
        });
        public static ElementDefinition Create<P>(FunctionElement<P> functionElement, uint key, Name name, RefAction<P> props) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            props = RefProps(props)
        });
        public static ElementDefinition Create<P>(FunctionElement<P> functionElement, uint key, ClassList classList, Style style) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            classList = classList,
            style = style
        });
        public static ElementDefinition Create<P>(FunctionElement<P> functionElement, uint key, ClassList classList, P props) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            classList = classList,
            props = props
        });
        public static ElementDefinition Create<P>(FunctionElement<P> functionElement, uint key, ClassList classList, RefAction<P> props) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            classList = classList,
            props = RefProps(props)
        });
        public static ElementDefinition Create<P>(FunctionElement<P> functionElement, uint key, Style style, P props) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            style = style,
            props = props
        });
        public static ElementDefinition Create<P>(FunctionElement<P> functionElement, uint key, Style style, RefAction<P> props) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            style = style,
            props = RefProps(props)
        });
        public static ElementDefinition Create<P>(FunctionElement<P> functionElement, Name name, ClassList classList, Style style) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            classList = classList,
            style = style
        });
        public static ElementDefinition Create<P>(FunctionElement<P> functionElement, Name name, ClassList classList, P props) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            classList = classList,
            props = props
        });
        public static ElementDefinition Create<P>(FunctionElement<P> functionElement, Name name, ClassList classList, RefAction<P> props) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            classList = classList,
            props = RefProps(props)
        });
        public static ElementDefinition Create<P>(FunctionElement<P> functionElement, Name name, Style style, P props) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            style = style,
            props = props
        });
        public static ElementDefinition Create<P>(FunctionElement<P> functionElement, Name name, Style style, RefAction<P> props) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            style = style,
            props = RefProps(props)
        });
        public static ElementDefinition Create<P>(FunctionElement<P> functionElement, ClassList classList, Style style, P props) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            classList = classList,
            style = style,
            props = props
        });
        public static ElementDefinition Create<P>(FunctionElement<P> functionElement, ClassList classList, Style style, RefAction<P> props) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            classList = classList,
            style = style,
            props = RefProps(props)
        });
        // 4/5 -> 5
        public static ElementDefinition Create<P>(FunctionElement<P> functionElement, uint key, Name name, ClassList classList, Style style) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            classList = classList,
            style = style
        });
        public static ElementDefinition Create<P>(FunctionElement<P> functionElement, uint key, Name name, ClassList classList, P props) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            classList = classList,
            props = props
        });
        public static ElementDefinition Create<P>(FunctionElement<P> functionElement, uint key, Name name, ClassList classList, RefAction<P> props) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            classList = classList,
            props = RefProps(props)
        });
        public static ElementDefinition Create<P>(FunctionElement<P> functionElement, uint key, Name name, Style style, P props) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            style = style,
            props = props
        });
        public static ElementDefinition Create<P>(FunctionElement<P> functionElement, uint key, Name name, Style style, RefAction<P> props) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            style = style,
            props = RefProps(props)
        });
        public static ElementDefinition Create<P>(FunctionElement<P> functionElement, uint key, ClassList classList, Style style, P props) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            classList = classList,
            style = style,
            props = props
        });
        public static ElementDefinition Create<P>(FunctionElement<P> functionElement, uint key, ClassList classList, Style style, RefAction<P> props) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            classList = classList,
            style = style,
            props = RefProps(props)
        });
        public static ElementDefinition Create<P>(FunctionElement<P> functionElement, Name name, ClassList classList, Style style, P props) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            classList = classList,
            style = style,
            props = props
        });
        public static ElementDefinition Create<P>(FunctionElement<P> functionElement, Name name, ClassList classList, Style style, RefAction<P> props) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            classList = classList,
            style = style,
            props = RefProps(props)
        });
        // 5/5 -> 1
        public static ElementDefinition Create<P>(FunctionElement<P> functionElement, uint key, Name name, ClassList classList, Style style, P props) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            classList = classList,
            style = style,
            props = props
        });
        public static ElementDefinition Create<P>(FunctionElement<P> functionElement, uint key, Name name, ClassList classList, Style style, RefAction<P> props) where P : struct => Create(functionElement, new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            classList = classList,
            style = style,
            props = RefProps(props)
        });
        // Descriptor
        public static ElementDefinition Create<P>(FunctionElement<P> functionElement, Descriptor<P> descriptor) where P : struct
        {
            var element = GetElement<FunctionSetup<P>>();
            element.Factory(descriptor, functionElement);
            
            OnCreate(element);

            return element;
        }
        
        
        
        
        
        // -------------------------------------------------------------------------------------------------------------
        // --- NATIVE ELEMENTS -----------------------------------------------------------------------------------------
        // -------------------------------------------------------------------------------------------------------------
        
        // 0/4 -> 1
        public static ElementDefinition Create<T>(Children? children = null) where T : VisualElement, INativeElement, new() => Create<T>(Descriptor.Default, children);
        // 1/4 -> 4
        public static ElementDefinition Create<T>(uint key, Children? children = null) where T : VisualElement, INativeElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            key = key
        }, children);
        public static ElementDefinition Create<T>(Name name, Children? children = null) where T : VisualElement, INativeElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            name = name
        }, children);
        public static ElementDefinition Create<T>(ClassList classList, Children? children = null) where T : VisualElement, INativeElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            classList = classList
        }, children);
        public static ElementDefinition Create<T>(Style style, Children? children = null) where T : VisualElement, INativeElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            style = style
        }, children);
        // 2/4 -> 6
        public static ElementDefinition Create<T>(uint key, Name name, Children? children = null) where T : VisualElement, INativeElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name
        }, children);
        public static ElementDefinition Create<T>(uint key, ClassList classList, Children? children = null) where T : VisualElement, INativeElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            key = key,
            classList = classList
        }, children);
        public static ElementDefinition Create<T>(uint key, Style style, Children? children = null) where T : VisualElement, INativeElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            key = key,
            style = style
        }, children);
        public static ElementDefinition Create<T>(Name name, ClassList classList, Children? children = null) where T : VisualElement, INativeElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            name = name,
            classList = classList,
        }, children);
        public static ElementDefinition Create<T>(Name name, Style style, Children? children = null) where T : VisualElement, INativeElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            name = name,
            style = style
        }, children);
        public static ElementDefinition Create<T>(ClassList classList, Style style, Children? children = null) where T : VisualElement, INativeElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            classList = classList,
            style = style
        }, children);
        // 3/4 -> 4
        public static ElementDefinition Create<T>(uint key, Name name, ClassList classList, Children? children = null) where T : VisualElement, INativeElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            classList = classList
        }, children);
        public static ElementDefinition Create<T>(uint key, Name name, Style style, Children? children = null) where T : VisualElement, INativeElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            style = style
        }, children);
        public static ElementDefinition Create<T>(uint key, ClassList classList, Style style, Children? children = null) where T : VisualElement, INativeElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            key = key,
            classList = classList,
            style = style
        }, children);
        public static ElementDefinition Create<T>(Name name, ClassList classList, Style style, Children? children = null) where T : VisualElement, INativeElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            name = name,
            classList = classList,
            style = style
        }, children);
        // 4/4 -> 1
        public static ElementDefinition Create<T>(uint key, Name name, ClassList classList, Style style, Children? children = null) where T : VisualElement, INativeElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            classList = classList,
            style = style
        }, children);
        // Descriptor
        public static ElementDefinition Create<T>(Descriptor descriptor, Children? children = null) where T : VisualElement, INativeElement, new()
        {
            var element = GetElement<NativeSetup<T>>();
            element.Factory(descriptor, children ?? Children.Empty);
            
            OnCreate(element);

            return element;
        }



        // 0/5 -> 1
        public static ElementDefinition Create<T, P>(Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(Descriptor<P>.Default, children);
        // 1/5 -> 5
        public static ElementDefinition Create<T, P>(uint key, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key
        }, children);
        public static ElementDefinition Create<T, P>(Name name, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name
        }, children);
        public static ElementDefinition Create<T, P>(ClassList classList, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            classList = classList
        }, children);
        public static ElementDefinition Create<T, P>(Style style, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            style = style
        }, children);
        public static ElementDefinition Create<T, P>(P props, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            props = props
        }, children);
        public static ElementDefinition Create<T, P>(RefAction<P> props, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            props = RefProps(props)
        }, children);
        // 2/5 = 10
        public static ElementDefinition Create<T, P>(uint key, Name name, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name
        }, children);
        public static ElementDefinition Create<T, P>(uint key, ClassList classList, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            classList = classList
        }, children);
        public static ElementDefinition Create<T, P>(uint key, Style style, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            style = style
        }, children);
        public static ElementDefinition Create<T, P>(uint key, P props, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            props = props
        }, children);
        public static ElementDefinition Create<T, P>(uint key, RefAction<P> props, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            props = RefProps(props)
        }, children);
        public static ElementDefinition Create<T, P>(Name name, ClassList classList, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            classList = classList
        }, children);
        public static ElementDefinition Create<T, P>(Name name, Style style, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            style = style
        }, children);
        public static ElementDefinition Create<T, P>(Name name, P props, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            props = props
        }, children);
        public static ElementDefinition Create<T, P>(Name name, RefAction<P> props, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            props = RefProps(props)
        }, children);
        public static ElementDefinition Create<T, P>(ClassList classList, Style style, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            classList = classList,
            style = style
        }, children);
        public static ElementDefinition Create<T, P>(ClassList classList, P props, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            classList = classList,
            props = props
        }, children);
        public static ElementDefinition Create<T, P>(ClassList classList, RefAction<P> props, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            classList = classList,
            props = RefProps(props)
        }, children);
        public static ElementDefinition Create<T, P>(Style style, P props, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            style = style,
            props = props
        }, children);
        public static ElementDefinition Create<T, P>(Style style, RefAction<P> props, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            style = style,
            props = RefProps(props)
        }, children);
        // 3/5 = 10
        public static ElementDefinition Create<T, P>(uint key, Name name, ClassList classList, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            classList = classList
        }, children);
        public static ElementDefinition Create<T, P>(uint key, Name name, Style style, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            style = style
        }, children);
        public static ElementDefinition Create<T, P>(uint key, Name name, P props, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            props = props
        }, children);
        public static ElementDefinition Create<T, P>(uint key, Name name, RefAction<P> props, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            props = RefProps(props)
        }, children);
        public static ElementDefinition Create<T, P>(uint key, ClassList classList, Style style, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            classList = classList,
            style = style
        }, children);
        public static ElementDefinition Create<T, P>(uint key, ClassList classList, P props, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            classList = classList,
            props = props
        }, children);
        public static ElementDefinition Create<T, P>(uint key, ClassList classList, RefAction<P> props, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            classList = classList,
            props = RefProps(props)
        }, children);
        public static ElementDefinition Create<T, P>(uint key, Style style, P props, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            style = style,
            props = props
        }, children);
        public static ElementDefinition Create<T, P>(uint key, Style style, RefAction<P> props, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            style = style,
            props = RefProps(props)
        }, children);
        public static ElementDefinition Create<T, P>(Name name, ClassList classList, Style style, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            classList = classList,
            style = style
        }, children);
        public static ElementDefinition Create<T, P>(Name name, ClassList classList, P props, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            classList = classList,
            props = props
        }, children);
        public static ElementDefinition Create<T, P>(Name name, ClassList classList, RefAction<P> props, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            classList = classList,
            props = RefProps(props)
        }, children);
        public static ElementDefinition Create<T, P>(Name name, Style style, P props, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            style = style,
            props = props
        }, children);
        public static ElementDefinition Create<T, P>(Name name, Style style, RefAction<P> props, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            style = style,
            props = RefProps(props)
        }, children);
        public static ElementDefinition Create<T, P>(ClassList classList, Style style, P props, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            classList = classList,
            style = style,
            props = props
        }, children);
        public static ElementDefinition Create<T, P>(ClassList classList, Style style, RefAction<P> props, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            classList = classList,
            style = style,
            props = RefProps(props)
        }, children);
        // 4/5 = 5
        public static ElementDefinition Create<T, P>(uint key, Name name, ClassList classList, Style style, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            classList = classList,
            style = style
        }, children);
        public static ElementDefinition Create<T, P>(uint key, Name name, ClassList classList, P props, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            classList = classList,
            props = props
        }, children);
        public static ElementDefinition Create<T, P>(uint key, Name name, ClassList classList, RefAction<P> props, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            classList = classList,
            props = RefProps(props)
        }, children);
        public static ElementDefinition Create<T, P>(uint key, Name name, Style style, P props, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            style = style,
            props = props
        }, children);
        public static ElementDefinition Create<T, P>(uint key, Name name, Style style, RefAction<P> props, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            style = style,
            props = RefProps(props)
        }, children);
        public static ElementDefinition Create<T, P>(uint key, ClassList classList, Style style, P props, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            classList = classList,
            style = style,
            props = props
        }, children);
        public static ElementDefinition Create<T, P>(uint key, ClassList classList, Style style, RefAction<P> props, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            classList = classList,
            style = style,
            props = RefProps(props)
        }, children);
        public static ElementDefinition Create<T, P>(Name name, ClassList classList, Style style, P props, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            classList = classList,
            style = style,
            props = props
        }, children);
        public static ElementDefinition Create<T, P>(Name name, ClassList classList, Style style, RefAction<P> props, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            classList = classList,
            style = style,
            props = RefProps(props)
        }, children);
        // 5/5 = 1
        public static ElementDefinition Create<T, P>(uint key, Name name, ClassList classList, Style style, P props, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            classList = classList,
            style = style,
            props = props
        }, children);
        public static ElementDefinition Create<T, P>(uint key, Name name, ClassList classList, Style style, RefAction<P> props, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            classList = classList,
            style = style,
            props = RefProps(props)
        }, children);
        // Descriptor
        public static ElementDefinition Create<T, P>(Descriptor<P> descriptor, Children? children = null) where T : VisualElement, INativeElement<P>, new() where P : struct
        {
            var element = GetElement<NativeSetup<T, P>>();
            element.Factory(descriptor, children ?? Children.Empty);
            
            OnCreate(element);

            return element;
        }
        
        
        
        
        
        // -------------------------------------------------------------------------------------------------------------
        // --- RISH ELEMENTS -------------------------------------------------------------------------------------------
        // -------------------------------------------------------------------------------------------------------------
        
        // 0/4 -> 1
        public static ElementDefinition Create<T>() where T : RishElement, new() => Create<T>(Descriptor.Default);
        // 1/4 -> 4
        public static ElementDefinition Create<T>(uint key) where T : RishElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            key = key
        });
        public static ElementDefinition Create<T>(Name name) where T : RishElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            name = name,
        });
        public static ElementDefinition Create<T>(ClassList classList) where T : RishElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            classList = classList
        });
        public static ElementDefinition Create<T>(Style style) where T : RishElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            style = style
        });
        // 2/4 -> 6
        public static ElementDefinition Create<T>(uint key, Name name) where T : RishElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name
        });
        public static ElementDefinition Create<T>(uint key, ClassList classList) where T : RishElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            key = key,
            classList = classList
        });
        public static ElementDefinition Create<T>(uint key, Style style) where T : RishElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            key = key,
            style = style
        });
        public static ElementDefinition Create<T>(Name name, ClassList classList) where T : RishElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            name = name,
            classList = classList
        });
        public static ElementDefinition Create<T>(Name name, Style style) where T : RishElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            name = name,
            style = style
        });
        public static ElementDefinition Create<T>(ClassList classList, Style style) where T : RishElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            classList = classList,
            style = style
        });
        // 3/4 -> 4
        public static ElementDefinition Create<T>(uint key, Name name, ClassList classList) where T : RishElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            classList = classList
        });
        public static ElementDefinition Create<T>(uint key, Name name, Style style) where T : RishElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            style = style
        });
        public static ElementDefinition Create<T>(uint key, ClassList classList, Style style) where T : RishElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            key = key,
            classList = classList,
            style = style
        });
        public static ElementDefinition Create<T>(Name name, ClassList classList, Style style) where T : RishElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            name = name,
            classList = classList,
            style = style
        });
        // 4/4 -> 1
        public static ElementDefinition Create<T>(uint key, Name name, ClassList classList, Style style) where T : RishElement, new() => Create<T>(new Descriptor(Descriptor.Default)
        {
            key = key,
            name = name,
            classList = classList,
            style = style
        });
        // Descriptor
        public static ElementDefinition Create<T>(Descriptor descriptor) where T : RishElement, new()
        {
            var element = GetElement<RishSetup<T>>();
            element.Factory(descriptor);
            
            OnCreate(element);

            return element;
        }

        // 0/5 -> 1
        public static ElementDefinition Create<T, P>() where T : RishElement<P>, new() where P : struct => Create<T, P>(Descriptor<P>.Default);
        // 1/5 -> 5
        public static ElementDefinition Create<T, P>(uint key) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key
        });
        public static ElementDefinition Create<T, P>(Name name) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name
        });
        public static ElementDefinition Create<T, P>(ClassList classList) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            classList = classList
        });
        public static ElementDefinition Create<T, P>(Style style) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            style = style
        });
        public static ElementDefinition Create<T, P>(P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            props = props
        });
        public static ElementDefinition Create<T, P>(RefAction<P> props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            props = RefProps(props)
        });
        // 2/5 = 10
        public static ElementDefinition Create<T, P>(uint key, Name name) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name
        });
        public static ElementDefinition Create<T, P>(uint key, ClassList classList) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            classList = classList
        });
        public static ElementDefinition Create<T, P>(uint key, Style style) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            style = style
        });
        public static ElementDefinition Create<T, P>(uint key, P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            props = props
        });
        public static ElementDefinition Create<T, P>(uint key, RefAction<P> props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            props = RefProps(props)
        });
        public static ElementDefinition Create<T, P>(Name name, ClassList classList) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            classList = classList
        });
        public static ElementDefinition Create<T, P>(Name name, Style style) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            style = style
        });
        public static ElementDefinition Create<T, P>(Name name, P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            props = props
        });
        public static ElementDefinition Create<T, P>(Name name, RefAction<P> props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            props = RefProps(props)
        });
        public static ElementDefinition Create<T, P>(ClassList classList, Style style) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            classList = classList,
            style = style
        });
        public static ElementDefinition Create<T, P>(ClassList classList, P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            classList = classList,
            props = props
        });
        public static ElementDefinition Create<T, P>(ClassList classList, RefAction<P> props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            classList = classList,
            props = RefProps(props)
        });
        public static ElementDefinition Create<T, P>(Style style, P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            style = style,
            props = props
        });
        public static ElementDefinition Create<T, P>(Style style, RefAction<P> props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            style = style,
            props = RefProps(props)
        });
        // 3/5 = 10
        public static ElementDefinition Create<T, P>(uint key, Name name, ClassList classList) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            classList = classList
        });
        public static ElementDefinition Create<T, P>(uint key, Name name, Style style) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            style = style
        });
        public static ElementDefinition Create<T, P>(uint key, Name name, P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            props = props
        });
        public static ElementDefinition Create<T, P>(uint key, Name name, RefAction<P> props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            props = RefProps(props)
        });
        public static ElementDefinition Create<T, P>(uint key, ClassList classList, Style style) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            classList = classList,
            style = style
        });
        public static ElementDefinition Create<T, P>(uint key, ClassList classList, P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            classList = classList,
            props = props
        });
        public static ElementDefinition Create<T, P>(uint key, ClassList classList, RefAction<P> props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            classList = classList,
            props = RefProps(props)
        });
        public static ElementDefinition Create<T, P>(uint key, Style style, P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            style = style,
            props = props
        });
        public static ElementDefinition Create<T, P>(uint key, Style style, RefAction<P> props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            style = style,
            props = RefProps(props)
        });
        public static ElementDefinition Create<T, P>(Name name, ClassList classList, Style style) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            classList = classList,
            style = style
        });
        public static ElementDefinition Create<T, P>(Name name, ClassList classList, P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            classList = classList,
            props = props
        });
        public static ElementDefinition Create<T, P>(Name name, ClassList classList, RefAction<P> props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            classList = classList,
            props = RefProps(props)
        });
        public static ElementDefinition Create<T, P>(Name name, Style style, P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            style = style,
            props = props
        });
        public static ElementDefinition Create<T, P>(Name name, Style style, RefAction<P> props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            style = style,
            props = RefProps(props)
        });
        public static ElementDefinition Create<T, P>(ClassList classList, Style style, P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            classList = classList,
            style = style,
            props = props
        });
        public static ElementDefinition Create<T, P>(ClassList classList, Style style, RefAction<P> props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            classList = classList,
            style = style,
            props = RefProps(props)
        });
        // 4/5 = 5
        public static ElementDefinition Create<T, P>(uint key, Name name, ClassList classList, Style style) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            classList = classList,
            style = style
        });
        public static ElementDefinition Create<T, P>(uint key, Name name, ClassList classList, P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            classList = classList,
            props = props
        });
        public static ElementDefinition Create<T, P>(uint key, Name name, ClassList classList, RefAction<P> props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            classList = classList,
            props = RefProps(props)
        });
        public static ElementDefinition Create<T, P>(uint key, Name name, Style style, P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            style = style,
            props = props
        });
        public static ElementDefinition Create<T, P>(uint key, Name name, Style style, RefAction<P> props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            style = style,
            props = RefProps(props)
        });
        public static ElementDefinition Create<T, P>(uint key, ClassList classList, Style style, P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            classList = classList,
            style = style,
            props = props
        });
        public static ElementDefinition Create<T, P>(uint key, ClassList classList, Style style, RefAction<P> props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            classList = classList,
            style = style,
            props = RefProps(props)
        });
        public static ElementDefinition Create<T, P>(Name name, ClassList classList, Style style, P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            classList = classList,
            style = style,
            props = props
        });
        public static ElementDefinition Create<T, P>(Name name, ClassList classList, Style style, RefAction<P> props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            name = name,
            classList = classList,
            style = style,
            props = RefProps(props)
        });
        // 5/5 = 1
        public static ElementDefinition Create<T, P>(uint key, Name name, ClassList classList, Style style, P props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            classList = classList,
            style = style,
            props = props
        });
        public static ElementDefinition Create<T, P>(uint key, Name name, ClassList classList, Style style, RefAction<P> props) where T : RishElement<P>, new() where P : struct => Create<T, P>(new Descriptor<P>(Descriptor<P>.Default)
        {
            key = key,
            name = name,
            classList = classList,
            style = style,
            props = RefProps(props)
        });
        // Descriptor
        public static ElementDefinition Create<T, P>(Descriptor<P> descriptor) where T : RishElement<P>, new() where P : struct
        {
            var element = GetElement<RishSetup<T, P>>();
            element.Factory(descriptor);
            
            OnCreate(element);

            return element;
        }





        // -------------------------------------------------------------------------------------------------------------
        // --- SETUPS --------------------------------------------------------------------------------------------------
        // -------------------------------------------------------------------------------------------------------------
        private class FunctionSetup : ElementSetup
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

            public override bool Equals(ElementSetup other)
            {
                return false;
            }
        }

        private class FunctionSetup<P> : ElementSetup where P : struct
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

            public override bool Equals(ElementSetup other)
            {
                return false;
            }
        }
        
        private class NativeSetup<T> : ElementSetup where T : VisualElement, INativeElement, new()
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

            public override bool Equals(ElementSetup other)
            {
                return false;
            }
        }

        private class NativeSetup<T, P> : ElementSetup where T: VisualElement, INativeElement<P>, new() where P : struct
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

            public override bool Equals(ElementSetup other)
            {
                return false;
            }
        }

        private class RishSetup<T> : ElementSetup where T : RishElement, new()
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

            public override bool Equals(ElementSetup other)
            {
                return false;
            }
        }

        private class RishSetup<T, P> : ElementSetup where T : RishElement<P>, new() where P : struct
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

            public override bool Equals(ElementSetup other)
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RishUI
{
    public static partial class Rish
    {
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
            
            return CreateElement(element);
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
            
            return CreateElement(element);
        }

        private class RishDefinition<T, P> : NodeElementDefinition where T : RishElement<P>, new() where P : struct
        {
            private P Props { get; set; }

            public void Factory(Descriptor descriptor, P props)
            {
                Descriptor = descriptor;
                Props = props;
            }

            public override void Dispose() { }

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
    }
}
namespace RishUI
{
    public static partial class Rish
    {
        
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
            
            return CreateElement(element);
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
            
            return CreateElement(element);
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
    } 
}
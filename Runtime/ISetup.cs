using System;
using UnityEngine;

namespace RishUI
{
    public interface ISetup : IEquatable<ISetup>
    {
        Action<IRishComponent> ExtraSetup { get; set; }
        
        void Reset();
        void Setup(IRishComponent component);
    }
    
    public class NoSetup : ISetup
    {
        public Action<IRishComponent> ExtraSetup { get; set; }
        
        public void Reset()
        {
            ExtraSetup = null;
        }

        public void Setup(IRishComponent component)
        {
            ExtraSetup?.Invoke(component);
        }

        public bool Equals(ISetup other)
        {
            if (!(other is NoSetup)) return false;

            return ExtraSetup == null && other.ExtraSetup == null;
        }
    }
    
    public class BasicSetup<P> : ISetup where P : struct, IProps<P>
    {
        public P Props { get; set; }
        public Action<IRishComponent> ExtraSetup { get; set; }

        public void Reset()
        {
            Props = default;
            ExtraSetup = null;
        }

        public void Setup(IRishComponent component)
        {
            if (component is IRishComponent<P> propsComponent)
            {
                propsComponent.Props = Props;
            }
            
            ExtraSetup?.Invoke(component);
        }

        public bool Equals(ISetup other)
        {
            if (!(other is BasicSetup<P> otherBasic)) return false;

            if (!Props.Equals(otherBasic.Props)) return false; 

            return ExtraSetup == null && other.ExtraSetup == null;
        }
    }
    
    public class AdvancedSetup<P> : ISetup where P : struct, IProps<P>
    {
        public Props<P> Props { get; set; }
        public Action<IRishComponent> ExtraSetup { get; set; }

        public void Reset()
        {
            Props = default;
            ExtraSetup = null;
        }
        
        public void Setup(IRishComponent component)
        {
            if (component is IRishComponent<P> propsComponent)
            {
                var props = propsComponent.Props;
                Props(ref props);
                propsComponent.Props = props;
            }
            
            ExtraSetup?.Invoke(component);
        }

        public bool Equals(ISetup other) => false;
    }
}


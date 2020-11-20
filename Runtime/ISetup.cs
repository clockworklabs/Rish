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
    
    internal class NoSetup : ISetup
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
            if (other == null)
            {
                return ExtraSetup == null;
            }

            if (!(other is NoSetup))
            {
                return false;
            }

            if (ExtraSetup != null || other.ExtraSetup != null) return false;

            return true;
        }
    }
    
    internal class BasicSetup<P> : ISetup where P : struct, IRishData<P>
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
            if (other == null)
            {
                return false;
            }

            if (ExtraSetup != null || other.ExtraSetup != null) return false;

            if (!(other is BasicSetup<P> otherBasic)) return false;

            return Props.Equals(otherBasic.Props);
        }
    }
    
    internal class AdvancedSetup<P> : ISetup where P : struct, IRishData<P>
    {
        public RefAction<P> Props { get; set; }
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

        public bool Equals(ISetup other)
        {
            if (other == null)
            {
                return false;
            }

            if (ExtraSetup != null || other.ExtraSetup != null) return false;
            
            if (!(other is AdvancedSetup<P> otherAdvanced)) return false;

            if (Props == null && otherAdvanced.Props == null) return true;
            if (Props == null || otherAdvanced.Props == null) return false;
            if (Props == otherAdvanced.Props) return false;
            
            P props = default;
            Props(ref props);
            
            P otherProps = default;
            otherAdvanced.Props(ref otherProps);

            return props.Equals(otherProps);
        }
    }
}


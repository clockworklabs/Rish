using System;

namespace RishUI
{
    public abstract class ElementDefinition : IEquatable<ElementDefinition>
    {
        public Descriptor Descriptor { get; protected set; }

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
}

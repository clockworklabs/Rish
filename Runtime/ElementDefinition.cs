using System;

namespace RishUI
{
    public abstract class ElementDefinition : IEquatable<ElementDefinition>
    {
        public abstract Element Copy();
        public abstract void Dispose();

        public abstract void Invoke(Node node);
        public abstract bool Equals(ElementDefinition other);
    }
    
    public abstract class NodeElementDefinition : ElementDefinition
    {
        public Descriptor Descriptor { get; protected set; }

        public override Element Copy() => New(Descriptor);
        
        public abstract Element New(Descriptor descriptor);

        public Element New(RefAction<Descriptor> action)
        {
            var descriptor = Descriptor;
            action?.Invoke(ref descriptor);
            return New(descriptor);
        }
    }
}

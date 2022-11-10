using System;

namespace RishUI
{
    public abstract class ElementDefinition : IEquatable<ElementDefinition>
    {
        public abstract Children Copy();
        public abstract void Dispose();

        public abstract void Invoke(Node node);
        public abstract bool Equals(ElementDefinition other);
    }
    
    public abstract class NodeElementDefinition : ElementDefinition
    {
        public Descriptor Descriptor { get; protected set; }

        public override Children Copy() => New(Descriptor);
        
        public abstract Children New(Descriptor descriptor);

        public Children New(RefAction<Descriptor> action)
        {
            var descriptor = Descriptor;
            action?.Invoke(ref descriptor);
            return New(descriptor);
        }
    }
}

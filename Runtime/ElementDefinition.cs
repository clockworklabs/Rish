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
    
    // public abstract class NodeElementDefinition : ElementDefinition
    // {
    //     public DOMDescriptor Descriptor { get; protected set; }
    //
    //     public override Children Copy() => New(Descriptor);
    //     
    //     public abstract Children New(DOMDescriptor descriptor);
    //
    //     public Children New(RefAction<DOMDescriptor> action)
    //     {
    //         var descriptor = Descriptor;
    //         action?.Invoke(ref descriptor);
    //         return New(descriptor);
    //     }
    // }
    
    public abstract class SingleElementDefinition : ElementDefinition
    {
        public uint Key { get; protected set; }

        public sealed override Children Copy() => New(Key);
        
        public abstract Children New(uint key);

        public Children New(RefAction<uint> action)
        {
            var key = Key;
            action?.Invoke(ref key);
            return New(key);
        }
    }
    
    public abstract class VirtualElementDefinition : SingleElementDefinition
    {
        public sealed override void Dispose() { }
    }
}

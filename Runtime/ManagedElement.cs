using System;
using RishUI.MemoryManagement;

namespace RishUI
{
    [CustomManaged(typeof(ElementDefinitionPool))]
    public abstract class ManagedElement : IManaged, IEquatable<ManagedElement>
    {        
        public ulong Key { get; protected set; }
        public abstract Type Type { get; }
        
        internal abstract void Invoke(Node parent);
        public abstract bool Equals(ManagedElement other);
        
        public abstract bool TryGetProps<P>(out P props) where P : struct;
        // public abstract bool TrySetProps<P>(P props) where P : struct;
        //
        // public abstract void UpdateKey(ulong key);

        void IManaged.Close() { }
        void IManaged.Dispose() { }

        bool IEquatable<ManagedElement>.Equals(ManagedElement other) => Equals(other);
    }
}

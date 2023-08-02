using System;
using RishUI.MemoryManagement;

namespace RishUI
{
    [CustomManaged(typeof(ElementDefinitionPool))]
    public abstract class ManagedElement : IManaged, IEquatable<ManagedElement>
    {        
        public ulong Key { get; protected set; }
        public abstract Type Type { get; }
        
        protected abstract void ReferenceRegistered(IOwner owner);
        protected abstract void ReferenceUnregistered(IOwner owner);
        public abstract void Invoke(Node node);
        public abstract bool Equals(ManagedElement other);

        void IManaged.Dispose() { }
        void IManaged.ReferenceRegistered(IOwner owner) => ReferenceRegistered(owner);
        void IManaged.ReferenceUnregistered(IOwner owner) => ReferenceUnregistered(owner);

        bool IEquatable<ManagedElement>.Equals(ManagedElement other) => Equals(other);
    }
}

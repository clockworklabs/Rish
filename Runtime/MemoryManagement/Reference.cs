namespace RishUI.MemoryManagement
{
    public struct Reference
    {
        public int poolIndex;
        public uint managedID;

        internal bool Valid => poolIndex >= 0 && managedID > 0;

        internal void RegisterReference(IOwner owner) => Rish.RegisterReferenceTo(this, owner);
        internal void UnregisterReference(IOwner owner) => Rish.UnregisterReferenceTo(this, owner);
    }
}

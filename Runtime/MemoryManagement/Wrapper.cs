namespace RishUI.MemoryManagement
{
    internal interface IWrapper
    {
        public ulong ID { get; }
        public IManaged Managed { get; }
        public ManagedContext OwnerContext { get; }
        void Claimed(ManagedContext context);
        void Close();
        void Free();
    }
    internal class Wrapper<T> : IWrapper where T : class, IManaged, new()
    {
        public ulong ID { get; }
        ulong IWrapper.ID => ID;
        public T Managed { get; }
        IManaged IWrapper.Managed => Managed;
        private IPool Pool { get; }
        
        private ManagedContext OwnerContext { get; set; }
        ManagedContext IWrapper.OwnerContext => OwnerContext;

        public Wrapper(ulong id, T managed, IPool pool)
        {
            ID = id;
            Managed = managed;
            Pool = pool;
        }

        void IWrapper.Claimed(ManagedContext context)
        {
            OwnerContext = context;
            Managed.Claimed(context);
        }
        void IWrapper.Close() => Managed.Close();
        void IWrapper.Free()
        {
            OwnerContext = null;
            Managed.Dispose();
            Pool.Free<T>(ID);
        }
    }
}

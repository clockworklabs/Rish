namespace RishUI.MemoryManagement
{
    internal interface IWrapper
    {
        public ulong ID { get; }
        public IManaged Managed { get; }
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

        public Wrapper(ulong id, T managed, IPool pool)
        {
            ID = id;
            Managed = managed;
            Pool = pool;
        }
        
        void IWrapper.Close() => Managed.Close();
        void IWrapper.Free() => Pool.Free<T>(ID);
    }
}

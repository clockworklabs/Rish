namespace RishUI
{
    public interface IAdvancedStateListener<S> where S : struct
    {
        void StateSet(S oldState);
    }
}

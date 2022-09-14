namespace RishUI.Deprecated
{
    public interface IAdvancedStateListener<S> where S : struct
    {
        void StateSet(S oldState);
    }
}

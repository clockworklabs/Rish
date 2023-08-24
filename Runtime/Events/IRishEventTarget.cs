namespace RishUI.Events
{
    public interface IRishEventTarget
    {
        void HandleRishEvent(RishEventBase evt, EventPhase phase);
    }
}

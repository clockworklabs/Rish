using UnityEngine.UIElements;

public class ResetEvent : EventBase<ResetEvent>
{
    public static ResetEvent GetPooled(VisualElement target)
    {
        var pooled = EventBase<ResetEvent>.GetPooled();
        pooled.target = target;
        
        return pooled;
    }
}

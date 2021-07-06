namespace RishUI
{
    public static class RishUtils
    {
        public static bool HasPointerOver(IRishComponent component)
        {
            switch (component)
            {
                case RishComponent rishComponent:
                    return rishComponent.HasPointerOver;
                case UnityComponent unityComponent:
                    return unityComponent.HasPointerOver;
                default:
                    return false;
            }
        }
        
        public static bool HasPointerDown(IRishComponent component)
        {
            switch (component)
            {
                case RishComponent rishComponent:
                    return rishComponent.HasPointerDown;
                case UnityComponent unityComponent:
                    return unityComponent.HasPointerDown;
                default:
                    return false;
            }
        }
    }
}
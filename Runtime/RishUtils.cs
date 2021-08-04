using UnityEngine;

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

        public static IRishComponent GetParent(IRishComponent component)
        {
            switch (component)
            {
                case RishComponent rishComponent:
                    return rishComponent.Parent;
                case UnityComponent unityComponent:
                    return unityComponent.Parent;
                default:
                    throw new UnityException("Component type not supported");
            }
        }
        
        public static RishTransform GetRishWorld(IRishComponent component)
        {
            if (component == null)
            {
                return RishTransform.Null;
            }
            
            var world = component.Local;
            
            var parent = GetParent(component);
            while (parent != null)
            {
                world = parent.Local * world;
                parent = GetParent(parent);
            }

            return world;
        }
    }
}
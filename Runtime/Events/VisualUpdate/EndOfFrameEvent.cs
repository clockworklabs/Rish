using System.Collections.Generic;

namespace RishUI.Events
{
    internal class EndOfFrameEvent
    {
        private static List<VisualChangeManipulator> JustMountedElements { get; } = new();
        
        public static void Register(VisualChangeManipulator manipulator) => JustMountedElements.Add(manipulator);

        public static void SendEvents()
        {
            foreach (var manipulator in JustMountedElements)
            {
                manipulator.OnEndOfFrame();
            }
            
            JustMountedElements.Clear();
        }
     }
}
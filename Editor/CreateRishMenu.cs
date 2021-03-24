using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RishUI.Editor
{
    public static class CreateRishMenu
    {
        [MenuItem("GameObject/Rish", false, 10)]
        private static void CreateRishTree(MenuCommand menuCommand)
        {
            Undo.IncrementCurrentGroup();
            Undo.SetCurrentGroupName("Undo Create Rish");
            var undoGroupIndex = Undo.GetCurrentGroup();
            
            var rishGO = new GameObject("Rish", typeof(Rish));
            GameObjectUtility.SetParentAndAlign(rishGO, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(rishGO, null);

            var rootGO = new GameObject("Root", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster), typeof(DimensionsTracker));
            rootGO.transform.SetParent(rishGO.transform, false);
            Undo.RegisterCreatedObjectUndo(rootGO, null);

            var canvas = rootGO.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            var eventSystem = Object.FindObjectOfType<EventSystem>();
            if (eventSystem == null)
            {
                var eventSystemGO = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
                eventSystem = eventSystemGO.GetComponent<EventSystem>();
                Undo.RegisterCreatedObjectUndo(eventSystemGO, null);
            }
            
            var rish = rishGO.GetComponent<Rish>();
            var rootTransform = rootGO.GetComponent<RectTransform>();
            
            var rishSO = new SerializedObject(rish);
            rishSO.FindProperty("_eventSystem").objectReferenceValue = eventSystem;
            rishSO.FindProperty("_rootTransform").objectReferenceValue = rootTransform;
            rishSO.ApplyModifiedProperties();
            
            Undo.CollapseUndoOperations(undoGroupIndex);
            
            Selection.activeObject = rishGO;
        }
    }
}
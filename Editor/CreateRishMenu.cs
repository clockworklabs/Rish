using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RishUI.Editor
{
    public static class CreateRishMenu
    {
        // Add a menu item to create custom GameObjects.
        // Priority 1 ensures it is grouped with the other menu items of the same kind
        // and propagated to the hierarchy dropdown and hierarchy context menus.
        [MenuItem("GameObject/Rish", false, 10)]
        private static void CreateRishTree(MenuCommand menuCommand)
        {
            Undo.IncrementCurrentGroup();
            Undo.SetCurrentGroupName("Undo Create Rish");
            var undoGroupIndex = Undo.GetCurrentGroup();
            
            var rishGO = new GameObject("Rish", typeof(Rish));
            GameObjectUtility.SetParentAndAlign(rishGO, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(rishGO, null);

            var rootGO = new GameObject("Root", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            rootGO.transform.SetParent(rishGO.transform, false);
            Undo.RegisterCreatedObjectUndo(rootGO, null);

            var canvas = rootGO.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            var eventSystem = Object.FindObjectOfType<EventSystem>();
            if (eventSystem == null)
            {
                var eventSystemGO = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
                Undo.RegisterCreatedObjectUndo(eventSystemGO, null);
            }
            
            var rish = rishGO.GetComponent<Rish>();
            var rootTransform = rootGO.GetComponent<RectTransform>();
            
            var rishSO = new SerializedObject(rish);
            rishSO.FindProperty("_rootTransform").objectReferenceValue = rootTransform;
            rishSO.ApplyModifiedProperties();
            
            Undo.CollapseUndoOperations(undoGroupIndex);
            
            Selection.activeObject = rishGO;
        }
    }
}
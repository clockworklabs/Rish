using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rish.Editor
{
    public static class CreateRishMenu
    {
        // Add a menu item to create custom GameObjects.
        // Priority 1 ensures it is grouped with the other menu items of the same kind
        // and propagated to the hierarchy dropdown and hierarchy context menus.
        [MenuItem("GameObject/Rish/New DOM", false, 10)]
        static void CreateCustomGameObject(MenuCommand menuCommand)
        {
            Undo.IncrementCurrentGroup();
            Undo.SetCurrentGroupName("Undo Create Rish DOM");
            var undoGroupIndex = Undo.GetCurrentGroup();
            
            var rishGO = new GameObject("Rish", typeof(Rish));
            GameObjectUtility.SetParentAndAlign(rishGO, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(rishGO, null);

            var appGO = new GameObject("App", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            appGO.transform.SetParent(rishGO.transform, false);
            Undo.RegisterCreatedObjectUndo(appGO, null);

            var canvas = appGO.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            var eventSystem = Object.FindObjectOfType<EventSystem>();
            if (eventSystem == null)
            {
                var eventSystemGO = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
                Undo.RegisterCreatedObjectUndo(eventSystemGO, null);
            }
            
            Undo.CollapseUndoOperations(undoGroupIndex);
            
            Selection.activeObject = rishGO;
        }
    }
}
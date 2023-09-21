using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI.Editor
{
    [CustomEditor(typeof(RishRoot))]
    public class RishRootInspector : UnityEditor.Editor
    {
        private SerializedProperty RootScriptIdProperty { get; set; }
        private SerializedProperty RootClassNameProperty { get; set; }
        
        public override VisualElement CreateInspectorGUI()
        {
            var container = new VisualElement();

            container.Add(new PropertyField(serializedObject.FindProperty("_styleSheets")));

            RootScriptIdProperty = serializedObject.FindProperty("_rootScriptId");
            RootClassNameProperty = serializedObject.FindProperty("_rootClassName");
            
            var scriptField = new ObjectField("Root App")
            {
                allowSceneObjects = false,
                objectType = typeof(MonoScript),
                value = EditorUtility.InstanceIDToObject(RootScriptIdProperty.intValue)
            };
            scriptField.RegisterValueChangedCallback(OnRootChange);
            
            container.Add(scriptField);
            
            return container;
        }

        private void OnRootChange(ChangeEvent<Object> evt)
        {
            if (evt.target is not ObjectField target)
            {
                return;
            }

            if (evt.newValue is not MonoScript monoScript)
            {
                RootScriptIdProperty.intValue = -1;
                RootClassNameProperty.stringValue = null;
                serializedObject.ApplyModifiedProperties();
                return;
            }

            var type = monoScript.GetClass();
            
            if (!typeof(IApp).IsAssignableFrom(type))
            {
                target.UnregisterValueChangedCallback(OnRootChange);
                target.value = evt.previousValue;
                target.RegisterValueChangedCallback(OnRootChange);
            }
            else
            {
                RootScriptIdProperty.intValue = monoScript.GetInstanceID();
                RootClassNameProperty.stringValue = type.FullName;
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}

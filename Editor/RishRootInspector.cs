using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace RishUI.Editor
{
    [CustomEditor(typeof(RishRoot))]
    public class RishRootInspector : UnityEditor.Editor
    {
        private SerializedProperty RootGUIDProperty { get; set; }
        private SerializedProperty RootClassNameProperty { get; set; }
        
        public override VisualElement CreateInspectorGUI()
        {
            var container = new VisualElement();

            container.Add(new PropertyField(serializedObject.FindProperty("_styleSheets")));

            RootGUIDProperty = serializedObject.FindProperty("_rootGUID");
            RootClassNameProperty = serializedObject.FindProperty("_rootClassName");

            var script = AssetDatabase.LoadAssetAtPath<MonoScript>(AssetDatabase.GUIDToAssetPath(RootGUIDProperty.stringValue));
            var scriptField = new ObjectField("Root App")
            {
                allowSceneObjects = false,
                objectType = typeof(MonoScript),
                value = script
            };
            scriptField.RegisterValueChangedCallback(OnRootChange);
            
            container.Add(scriptField);
            if (script != null)
            {
                container.Add(new Label($"\t({RootClassNameProperty.stringValue})"));
            }
            
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
                RootGUIDProperty.stringValue = null;
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
                var guid = AssetDatabase.GUIDFromAssetPath(AssetDatabase.GetAssetPath(monoScript)).ToString();
                
                RootGUIDProperty.stringValue = guid;
                RootClassNameProperty.stringValue = type.FullName;
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}

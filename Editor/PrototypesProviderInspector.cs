using Malee.List;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace RishUI.Editor
{
    [CustomEditor(typeof(PrototypesProvider))]
    public class PrototypesProviderInspector : UnityEditor.Editor
    {
        private SerializedProperty ScriptProperty { get; set; }
        
        private ReorderableList ReorderableList { get; set; }

        private void OnEnable()
        {
            ScriptProperty = serializedObject.FindProperty("m_Script");
            
            var listProperty = serializedObject.FindProperty("prototypes");

            ReorderableList = new ReorderableList(listProperty)
            {
                elementLabels = false
            };

            ReorderableList.onAddCallback += OnAdd;
            ReorderableList.onValidateDragAndDropCallback += OnValidateDragAndDrop;
            ReorderableList.onAppendDragDropCallback += OnAppendDragAndDrop;
        }

        private void OnDisable()
        {
            ReorderableList.onAddCallback -= OnAdd;
            ReorderableList.onValidateDragAndDropCallback -= OnValidateDragAndDrop;
            ReorderableList.onAppendDragDropCallback -= OnAppendDragAndDrop;
        }

        public override void OnInspectorGUI()
        {
            GUI.enabled = false;
            EditorGUILayout.PropertyField(ScriptProperty);
            GUI.enabled = true;
            
            serializedObject.Update();
            ReorderableList.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }

        private void OnAdd(ReorderableList list)
        {
            var item = list.AddItem();
            var componentProperty = item.FindPropertyRelative("component");
            var initialCountProperty = item.FindPropertyRelative("initialCount");

            componentProperty.objectReferenceValue = null;
            initialCountProperty.intValue = 1;
        }

        private UnityObject OnValidateDragAndDrop(UnityObject[] references, ReorderableList list)
        {
            if(references == null || references.Length != 1)
            {
                return null;
            }

            var reference = references[0];
            if (reference is GameObject gameObject)
            {
                return gameObject.GetComponent<UnityComponent>();
            }

            return null;
        }

        private void OnAppendDragAndDrop(UnityObject reference, ReorderableList list)
        {
            if(reference is UnityComponent component)
            {
                var item = list.AddItem();

                var elementProperty = item.FindPropertyRelative("component");
                var initialCountProperty = item.FindPropertyRelative("initialCount");

                elementProperty.objectReferenceValue = component;
                initialCountProperty.intValue = 1;
            }
        }
    }
}
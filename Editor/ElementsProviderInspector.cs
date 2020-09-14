using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Inspector = UnityEditor.Editor;
using Object = UnityEngine.Object;

namespace RishUI.Editor
{
    [CustomEditor(typeof(ComponentsProvider))]
    public class ElementsProviderInspector : Inspector
    {
        private SerializedProperty ScriptProperty { get; set; }
        private SerializedProperty StylesProperty { get; set; }
        
        private List<SerializedProperty> StylesObjects { get; } = new List<SerializedProperty>();
        
        private HashSet<int> StylesIDs { get; } = new HashSet<int>();
        
        private void OnEnable()
        {
            ScriptProperty = serializedObject.FindProperty("m_Script");
            StylesProperty = serializedObject.FindProperty("styles");
            
            StylesObjects.Clear();
            for (int i = 0, n = StylesProperty.arraySize; i < n; i++)
            {
                var element = StylesProperty.GetArrayElementAtIndex(i);
                var serializedObject = new SerializedObject(element.objectReferenceValue); 
                StylesObjects.Add(serializedObject.FindProperty("prototypes"));
            }
        }

        public override void OnInspectorGUI()
        {
            GUI.enabled = false;
            EditorGUILayout.PropertyField(ScriptProperty);
            GUI.enabled = true;
            
            serializedObject.Update();
            
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(StylesProperty);
            if (EditorGUI.EndChangeCheck())
            {
                for (var i = StylesProperty.arraySize - 1; i >= 0; i--)
                {
                    var element = StylesProperty.GetArrayElementAtIndex(i);
                    if (element.objectReferenceValue == null)
                    {
                        StylesProperty.DeleteArrayElementAtIndex(i);
                    }
                }

                StylesIDs.Clear();
                for (var i = StylesProperty.arraySize - 1; i >= 0; i--)
                {
                    var element = StylesProperty.GetArrayElementAtIndex(i);
                    var id = element.objectReferenceValue.GetInstanceID();
                    if (StylesIDs.Contains(id))
                    {
                        StylesProperty.DeleteArrayElementAtIndex(i);
                        continue;
                    }

                    StylesIDs.Add(id);
                }
                
                serializedObject.ApplyModifiedProperties();
                
                StylesObjects.Clear();
                for (int i = 0, n = StylesProperty.arraySize; i < n; i++)
                {
                    var element = StylesProperty.GetArrayElementAtIndex(i);
                    var serializedObject = new SerializedObject(element.objectReferenceValue); 
                    StylesObjects.Add(serializedObject.FindProperty("prototypes"));
                }
                return;
            }
            
            serializedObject.ApplyModifiedProperties();

            GUI.enabled = false;
            if (StylesProperty.arraySize > 0)
            {
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                
                var defaultStyle = StylesObjects[0];
            
                EditorGUILayout.LabelField("Prototypes", EditorStyles.boldLabel);
                for (int i = 0, n = defaultStyle.arraySize; i < n; i++)
                {
                    var defaultProperty = defaultStyle.GetArrayElementAtIndex(i);

                    EditorGUILayout.PropertyField(defaultProperty, GUIContent.none);
                    
                    var defaultElement = defaultProperty.FindPropertyRelative("component").objectReferenceValue;
                    if (defaultElement == null)
                    {
                        continue;
                    }
                    var type = defaultElement.GetType();
                    
                    EditorGUI.indentLevel += 1;
                    for (int j = 1, m = StylesObjects.Count; j < m; j++)
                    {
                        var style = StylesObjects[j];

                        for (var k = style.arraySize - 1; k >= 0; k--)
                        {
                            var styleProperty = style.GetArrayElementAtIndex(k);
                            var styleElement = styleProperty.FindPropertyRelative("component").objectReferenceValue;
                            if (styleElement != null && styleElement.GetType() == type)
                            {
                                EditorGUILayout.PropertyField(styleProperty, GUIContent.none);
                                break;
                            }
                        }
                        
                    }
                    EditorGUI.indentLevel -= 1;
                    EditorGUILayout.Space();
                }
                GUI.enabled = true;
            }
        }

        /*
        private ElementsProvider Target => target as ElementsProvider;
        
        private SerializedProperty StyleProperty { get; set; }
        private SerializedProperty DefaultStyleProperty { get; set; }
        private SerializedProperty StyleOverloadsProperty { get; set; }
        private SerializedProperty PrototypesProperty { get; set; }
        private SerializedProperty ParentsPrototypesProperty { get; set; }

        private bool Foldout { get; set; } = true;

        private void OnEnable()
        {
            StyleProperty = serializedObject.FindProperty("style");
            DefaultStyleProperty = serializedObject.FindProperty("defaultStyle");
            StyleOverloadsProperty = serializedObject.FindProperty("styleOverloads");
            PrototypesProperty = serializedObject.FindProperty("prototypes");

            if (DefaultStyleProperty.objectReferenceValue != null)
            {
                var parent = new SerializedObject(DefaultStyleProperty.objectReferenceValue);
                ParentsPrototypesProperty = parent.FindProperty("prototypes");
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (DefaultStyleProperty.objectReferenceValue == null)
            {
                Foldout = EditorGUILayout.Foldout(Foldout, StyleOverloadsProperty.displayName);
                var rect = GUILayoutUtility.GetLastRect();
                Event evt = Event.current;
                switch (evt.type) {
                    case EventType.DragUpdated:
                        if (rect.Contains(evt.mousePosition))
                        {
                            DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
                            foreach(var dragged in DragAndDrop.objectReferences)
                            {
                                if (dragged is ElementsProvider)
                                {
                                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                                    break;
                                }
                            }
                        }
                        break;
                    case EventType.DragPerform:
                        if (rect.Contains(evt.mousePosition))
                        {
                            DragAndDrop.AcceptDrag();

                            foreach(var dragged in DragAndDrop.objectReferences)
                            {
                                if (dragged is ElementsProvider newProvider)
                                {
                                    var instanceID = dragged.GetInstanceID();
                                    if (instanceID == target.GetInstanceID())
                                    {
                                        continue;
                                    }

                                    bool isNew = true;
                                    for (int i = 0, n = StyleOverloadsProperty.arraySize; i < n; i++)
                                    {
                                        if (StyleOverloadsProperty.GetArrayElementAtIndex(i).objectReferenceValue.GetInstanceID() ==
                                            instanceID)
                                        {
                                            isNew = false;
                                            break;
                                        }
                                    }

                                    if (isNew)
                                    {
                                        StyleOverloadsProperty.arraySize++;
                                        StyleOverloadsProperty
                                            .GetArrayElementAtIndex(StyleOverloadsProperty.arraySize - 1)
                                            .objectReferenceValue = newProvider;
                                        
                                        var child = new SerializedObject(newProvider);
                                        child.Update();
                                        child.FindProperty("defaultStyle").objectReferenceValue = target;
                                        child.FindProperty("style").intValue = StyleOverloadsProperty.arraySize;
                                        child.ApplyModifiedPropertiesWithoutUndo();
                                    }
                                }
                            }
                            
                            serializedObject.ApplyModifiedProperties();
                            return;
                        }
                        break;
                }
                
                if (Foldout)
                {
                    EditorGUI.indentLevel += 1;
                    GUI.enabled = false;
                    EditorGUILayout.IntField("Size", StyleOverloadsProperty.arraySize);
                    GUI.enabled = true;
                    for (int i = 0, n = StyleOverloadsProperty.arraySize; i < n; i++)
                    {
                        var prev = StyleOverloadsProperty.GetArrayElementAtIndex(i).objectReferenceValue;
                        EditorGUI.BeginChangeCheck();
                        EditorGUILayout.PropertyField(StyleOverloadsProperty.GetArrayElementAtIndex(i));
                        if (EditorGUI.EndChangeCheck())
                        {
                            var element = StyleOverloadsProperty.GetArrayElementAtIndex(i);
                            if (element.objectReferenceValue == null)
                            {
                                var child = new SerializedObject(prev);
                                child.Update();
                                child.FindProperty("defaultStyle").objectReferenceValue = null;
                                child.FindProperty("style").intValue = 0;
                                child.ApplyModifiedPropertiesWithoutUndo();
                                
                                StyleOverloadsProperty.DeleteArrayElementAtIndex(i);
                            }
                            else
                            {
                                var instanceID = element.objectReferenceValue.GetInstanceID();
                                var changed = true;
                                for (var j = 0; j < n && j != i; j++)
                                {
                                    if (StyleOverloadsProperty.GetArrayElementAtIndex(j).objectReferenceValue.GetInstanceID() ==
                                        instanceID)
                                    {
                                        element.objectReferenceValue = prev;
                                        changed = false;
                                        break;
                                    }
                                }

                                if (changed)
                                {
                                    var child = new SerializedObject(element.objectReferenceValue);
                                    child.Update();
                                    child.FindProperty("defaultStyle").objectReferenceValue = target;
                                    child.FindProperty("style").intValue = i + 1;
                                    child.ApplyModifiedPropertiesWithoutUndo();
                                    
                                    child = new SerializedObject(prev);
                                    child.Update();
                                    child.FindProperty("defaultStyle").objectReferenceValue = null;
                                    child.FindProperty("style").intValue = 0;
                                    child.ApplyModifiedPropertiesWithoutUndo();
                                }
                            }

                            break;
                        }
                    }
                    EditorGUI.indentLevel -= 1;
                }
            
                EditorGUILayout.PropertyField(PrototypesProperty);
            } else {
                GUI.enabled = false;
                EditorGUILayout.PropertyField(DefaultStyleProperty);
                EditorGUILayout.PropertyField(StyleProperty);
                GUI.enabled = true;
                
                Foldout = EditorGUILayout.Foldout(Foldout, PrototypesProperty.displayName);
                var rect = GUILayoutUtility.GetLastRect();
                EditorGUI.indentLevel += 1;
                GUI.enabled = false;
                EditorGUILayout.IntField("Size", PrototypesProperty.arraySize);
                GUI.enabled = true;
                for (int i = 0, n = ParentsPrototypesProperty.arraySize; i < n; i++)
                {
                    var parentPrototype = ParentsPrototypesProperty.GetArrayElementAtIndex(i).objectReferenceValue;
                    if (parentPrototype == null)
                    {
                        continue;
                    }

                    var type = parentPrototype.GetType();

                    UnityEngine.Object current = null;
                    for (int j = 0, m = PrototypesProperty.arraySize; j < m; j++)
                    {
                        var protoype = PrototypesProperty.GetArrayElementAtIndex(j).objectReferenceValue;
                        if (protoype != null && protoype.GetType() == type)
                        {
                            current = protoype;
                        }
                    }

                    EditorGUILayout.ObjectField(type.Name, current, typeof(DOMElement), false);
                }

                EditorGUI.indentLevel -= 1;
            }

            serializedObject.ApplyModifiedProperties();
        }
        */
    }
}
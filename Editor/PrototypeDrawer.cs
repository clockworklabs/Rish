using UnityEditor;
using UnityEngine;

namespace RishUI.Editor
{
    [CustomPropertyDrawer(typeof(Prototype))]
    public class PrototypeDrawer : PropertyDrawer
    {
        private const int InitialCountWidth = 40;
        private const int Margin = 4;
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            int indentLevel = EditorGUI.indentLevel;

            Rect elementPosition = new Rect(position.x, position.y, position.width - Margin - InitialCountWidth, position.height);
            SerializedProperty elementProperty = property.FindPropertyRelative("element");
            EditorGUI.PropertyField(elementPosition, elementProperty, GUIContent.none);

            EditorGUI.indentLevel = 0;
            
            Rect initialCountPosition = new Rect(elementPosition.xMax + Margin, position.y, InitialCountWidth, position.height);
            SerializedProperty initialCountProperty = property.FindPropertyRelative("initialCount");
            EditorGUI.PropertyField(initialCountPosition, initialCountProperty, GUIContent.none);

            EditorGUI.indentLevel = indentLevel;

            EditorGUI.EndProperty();
        }
    }
}
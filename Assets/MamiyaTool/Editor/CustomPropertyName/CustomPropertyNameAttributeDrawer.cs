using UnityEngine;
using UnityEditor;

namespace MamiyaTool
{
    [CustomPropertyDrawer(typeof(CustomPropertyNameAttribute))]
    public class CustomPropertyNameAtrributeDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            CustomPropertyNameAttribute tempAttribute = (CustomPropertyNameAttribute)attribute;
            EditorGUI.PropertyField(position, property, new GUIContent(tempAttribute.displayName), true);
        }
    }
}
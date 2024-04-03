using UnityEngine;
using UnityEditor;

namespace MFramework {
    [CustomPropertyDrawer(typeof(EnumFlagsAttribute))]
    public class EnumFlagsDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginChangeCheck();
            property.intValue = EditorGUI.MaskField(position, label, property.intValue, property.enumNames);
            if(EditorGUI.EndChangeCheck()) {
                property.serializedObject.ApplyModifiedProperties();
                FieldChangeDrawer.CheckFieldChange(property);
            }
        }
    }
}
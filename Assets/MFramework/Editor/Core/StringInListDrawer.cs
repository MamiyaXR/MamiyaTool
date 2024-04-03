using System;
using UnityEngine;
using UnityEditor;

namespace MFramework {
    [CustomPropertyDrawer(typeof(StringInListAttribute))]
    public class StringInListDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginChangeCheck();

            var stringInList = attribute as StringInListAttribute;
            var list = stringInList.GetList(property.serializedObject.targetObject);
            if(list != null && list.Length > 0) {
                if(property.propertyType == SerializedPropertyType.String) {
                    int index = Mathf.Max(0, Array.IndexOf(list, property.stringValue));
                    index = EditorGUI.Popup(position, property.displayName, index, list);
                    property.stringValue = list[index];
                } else if(property.propertyType == SerializedPropertyType.Integer) {
                    property.intValue = EditorGUI.Popup(position, property.displayName, property.intValue, list);
                } else {
                    EditorGUI.PropertyField(position, property, label);
                }
            } else {
                EditorGUI.PropertyField(position, property, label);
            }

            if(EditorGUI.EndChangeCheck()) {
                property.serializedObject.ApplyModifiedProperties();
                FieldChangeDrawer.CheckFieldChange(property);
            }
        }
    }
}
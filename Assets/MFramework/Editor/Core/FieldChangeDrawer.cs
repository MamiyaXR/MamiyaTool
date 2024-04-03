using System;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using MamiyaTool;

namespace MFramework {
    [CustomPropertyDrawer(typeof(FieldChangeAttribute))]
    public class FieldChangeDrawer : PropertyDrawer {
        private static BindingFlags GBindingFlags = BindingFlags.Instance | BindingFlags.Public
                                                    | BindingFlags.NonPublic | BindingFlags.Static;
        /******************************************************************
         *
         * 
         *
         ******************************************************************/
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(position, property, label);
            if(EditorGUI.EndChangeCheck()) {
                property.serializedObject.ApplyModifiedProperties();

                var fieldChangeProp = attribute as FieldChangeAttribute;
                CheckFieldChange(property, fieldChangeProp);
            }
        }
        /******************************************************************
         *
         *      public method
         *
         ******************************************************************/
        public static void CheckFieldChange(SerializedProperty property) {
            var targetObj = property.serializedObject.targetObject;
            var fieldName = property.name;
            var field = targetObj.GetType().GetFieldInTree(fieldName, GBindingFlags);
            if(field == null) {
                Debug.LogError($"Invalid field name: {targetObj.GetType()}.{fieldName}");
                return;
            }

            var attrs = field.GetCustomAttributes(typeof(FieldChangeAttribute), false);
            if(attrs == null || attrs.Length == 0)
                return;
            var fieldChangeAttr = attrs[0] as FieldChangeAttribute;
            if(fieldChangeAttr != null) {
                CheckFieldChange(property, fieldChangeAttr);
            }
        }
        /******************************************************************
         *
         *      private method
         *
         ******************************************************************/
        private static bool CheckProperty(object parent, FieldChangeAttribute fieldChangeProp) {
            Type type = parent.GetType();
            var action = type.GetMethodInTree(fieldChangeProp.MethodName, GBindingFlags);
            if(action == null)
                return false;
            else {
                action.Invoke(parent, null);
                return true;
            }
        }
        private static void CheckFieldChange(SerializedProperty property, FieldChangeAttribute fieldChangeProp) {
            var propertyPath = property.propertyPath;
            var index = propertyPath.LastIndexOf('.');
            if(index != -1) {
                var parentPath = propertyPath[..index];
                var t = property.serializedObject.FindProperty(parentPath);
                if(CheckProperty(t.managedReferenceValue, fieldChangeProp))
                    return;
            }

            object parent = property.serializedObject.targetObject;
            Type type = parent.GetType();
            var action = type.GetMethodInTree(fieldChangeProp.MethodName, GBindingFlags);
            if(action == null)
                Debug.LogError($"Invalid method name: {type}.{fieldChangeProp.MethodName}");
            else
                action.Invoke(parent, null);
            
            var go = (parent as Component).gameObject;
            EditorUtility.SetDirty(go);
        }
    }
}
using System;
using System.Reflection;
using UnityEngine;
using UnityEditor;

namespace MamiyaTool
{
    [CustomPropertyDrawer(typeof(InspectorButtonAttribute))]
    public class InspectorButtonAttributeDrawer : PropertyDrawer
    {
        private MethodInfo methodInfo;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            InspectorButtonAttribute tempAttribute = (InspectorButtonAttribute)attribute;

            GUI.skin.button.alignment = TextAnchor.MiddleLeft;
            if(GUI.Button(position, tempAttribute.methodName)) {
                Type type = property.serializedObject.targetObject.GetType();

                if(methodInfo == null) {
                    methodInfo = type.GetMethod(tempAttribute.methodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                }
                if(methodInfo != null) {
                    methodInfo.Invoke(property.serializedObject.targetObject, null);
                    EditorUtility.SetDirty(property.serializedObject.targetObject);
                } else {
                    Debug.LogError($"Not found method '{tempAttribute.methodName}' in class {type.Name}.");
                }
            }
        }
    }
}
using UnityEngine;
using UnityEditor;

namespace MFramework {
    [CustomPropertyDrawer(typeof(AnimClipToFromAttribute))]
    public class AnimClipToFromDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            var propertyPath = property.propertyPath;
            var index = propertyPath.LastIndexOf('.');
            var parentPath = propertyPath[..index];
            var parent = property.serializedObject.FindProperty(parentPath);
            var animClip = parent.managedReferenceValue as AnimClip;
            var isFrom = animClip.isFrom;
            var rc = new Rect(position.x + 10, position.y, 80, EditorGUIUtility.singleLineHeight);
            if(GUI.Button(rc, isFrom ? "From" : "To")) {
                animClip.isFrom = !isFrom;
            }
            EditorGUI.PropertyField(position, property, null, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
    }
}
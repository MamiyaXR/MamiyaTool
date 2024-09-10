using UnityEngine;
using UnityEditor;

namespace MFramework {
    [CustomPropertyDrawer(typeof(AnimLoop))]
    public class AnimLoopDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            var loops = property.FindPropertyRelative("loops");
            var loopType = property.FindPropertyRelative("loopType");
            if(loops.intValue == -1) {
                var loopsRect = new Rect(position.x, position.y, position.width / 2, EditorGUIUtility.singleLineHeight);
                EditorGUI.PropertyField(loopsRect, loops, label);
                var loopTypeRect = new Rect(position.x + position.width / 2, position.y, position.width / 2, EditorGUIUtility.singleLineHeight);
                EditorGUI.PropertyField(loopTypeRect, loopType, GUIContent.none);
            } else {
                EditorGUI.PropertyField(position, loops, label);
            }
        }
    }
}
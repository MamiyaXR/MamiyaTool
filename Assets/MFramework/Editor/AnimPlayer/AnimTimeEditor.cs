using UnityEngine;
using UnityEditor;
using DG.Tweening;

namespace MFramework {
    [CustomPropertyDrawer(typeof(AnimTime))]
    public class AnimTimeEditor : PropertyDrawer {
        internal static readonly string[] FilteredEaseTypes = new string[] {
            "Unset", "Linear", "InSine", "OutSine", "InOutSine", "InQuad", "OutQuad", "InOutQuad", "InCubic", "OutCubic", "InOutCubic",
            "InQuart", "OutQuart", "InOutQuart", "InQuint", "OutQuint", "InOutQuint", "InExpo", "OutExpo", "InOutExpo", "InCirc",
            "OutCirc", "InOutCirc", "InElastic", "OutElastic", "InOutElastic", "InBack", "OutBack", "InOutBack", "InBounce", "OutBounce",
            "InOutBounce", ":: AnimationCurve"
        };

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            var propDelay = property.FindPropertyRelative("delay");
            var propDuration = property.FindPropertyRelative("duration");
            var propEase = property.FindPropertyRelative("ease");

            var newPosition = EditorGUI.PrefixLabel(position, label);
            var w = newPosition.width / 3;
            var delayRect = new Rect(newPosition.x, newPosition.y, w, EditorGUIUtility.singleLineHeight);
            var durationRect = new Rect(newPosition.x + w, newPosition.y, w, EditorGUIUtility.singleLineHeight);
            var easeRect = new Rect(newPosition.x + w * 2, newPosition.y, w, EditorGUIUtility.singleLineHeight);
            GUI.Label(new Rect(delayRect.x, delayRect.y, 16, EditorGUIUtility.singleLineHeight), "T");
            EditorGUI.PropertyField(delayRect, propDelay, GUIContent.none);
            GUI.Label(new Rect(durationRect.x, delayRect.y, 16, EditorGUIUtility.singleLineHeight), "D");
            EditorGUI.PropertyField(durationRect, propDuration, GUIContent.none);

            var ease = (Ease)propEase.enumValueFlag;
            var idx = ease == Ease.INTERNAL_Custom ? FilteredEaseTypes.Length - 1 : (int)ease;
            GUI.Label(easeRect, "E");
            idx = EditorGUI.Popup(easeRect, idx, FilteredEaseTypes);
            ease = idx == FilteredEaseTypes.Length - 1 ? Ease.INTERNAL_Custom : (Ease)idx;
            //propEase.SetEnumValue(ease);
            propEase.enumValueFlag = (int)ease;

            if(ease == Ease.INTERNAL_Custom) {
                var propCurve = property.FindPropertyRelative("easeCurve");
                var acv = propCurve.animationCurveValue;
                if(acv == null || acv.length == 0) {
                    propCurve.animationCurveValue = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
                }
                var rc = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight, position.width, EditorGUIUtility.singleLineHeight);
                EditorGUI.PropertyField(rc, propCurve);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            var propEase = property.FindPropertyRelative("ease");
            var isCustom = propEase.enumValueFlag == (int)Ease.INTERNAL_Custom;
            return isCustom ? EditorGUIUtility.singleLineHeight * 2 : EditorGUIUtility.singleLineHeight;
        }
    }
}
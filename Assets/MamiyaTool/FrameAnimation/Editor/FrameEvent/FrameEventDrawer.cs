using JetBrains.Annotations;
using UnityEditorInternal;
using UnityEngine;
using UnityEditor;

namespace MamiyaTool {
    [CustomPropertyDrawer(typeof(CustomFrameEventDrawer))]
    [UsedImplicitly]
    public class FrameEventDrawer : UnityEventDrawer {
        internal const string kInstancePath = "m_Target";
        protected override void DrawEventHeader(Rect headerRect) { }
        protected override void SetupReorderableList(ReorderableList list) {
            base.SetupReorderableList(list);
            list.headerHeight = 4;
        }
        protected override void OnAddEvent(ReorderableList list) {
            base.OnAddEvent(list);
            var listProperty = list.serializedProperty;
            if(listProperty.arraySize > 0) {
                var lastCall = list.serializedProperty.GetArrayElementAtIndex(listProperty.arraySize - 1);
                var targetProperty = lastCall.FindPropertyRelative(kInstancePath);
                targetProperty.objectReferenceValue = FindBoundObject(listProperty);
            }
        }
        private static GameObject FindBoundObject(SerializedProperty property) {
            var component = property.serializedObject.targetObject as Component;
            return component != null ? component.gameObject : null;
        }
    }
}
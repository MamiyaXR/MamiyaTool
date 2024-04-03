using UnityEngine;
using UnityEditor;

namespace MFramework {
    [CanEditMultipleObjects]
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class CustomMonoBehaviourInspector : Editor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            EditorHelper.DrawMenus(target, EditorGUIUtility.currentViewWidth);
        }
    }
}
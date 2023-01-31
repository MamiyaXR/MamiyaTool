using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;

[CustomEditor(typeof(CustomText))]
public class CustomTextInspector : UnityEditor.UI.TextEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CustomText customText = (CustomText)target;
        EditorGUI.BeginChangeCheck();
        customText.needLocalization = EditorGUILayout.Toggle("Need Localization", customText.needLocalization);
        if(customText.needLocalization) {
            GUILayout.Label(new GUIContent("Localization Key"));
            customText.key = EditorGUILayout.TextArea(customText.key);
        }
        if(EditorGUI.EndChangeCheck())
            EditorUtility.SetDirty(target);
    }
}
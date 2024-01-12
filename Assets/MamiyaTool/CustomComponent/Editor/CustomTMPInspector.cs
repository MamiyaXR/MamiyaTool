using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro.EditorUtilities;

[CustomEditor(typeof(CustomTMP), true), CanEditMultipleObjects]
public class CustomTMPInspector : TMP_EditorPanelUI
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CustomTMP customText = (CustomTMP)target;
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

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;

[CustomEditor(typeof(CustomImage))]
public class CustomImageInspector : UnityEditor.UI.ImageEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CustomImage customImage = (CustomImage)target;
        EditorGUI.BeginChangeCheck();
        customImage.needLocalization = EditorGUILayout.Toggle("Need Localization", customImage.needLocalization);
        if(customImage.needLocalization) {
            GUILayout.Label(new GUIContent("Image Name"));
            customImage.imageName = EditorGUILayout.TextArea(customImage.imageName);
        }
        if(EditorGUI.EndChangeCheck())
            EditorUtility.SetDirty(target);
    }
}
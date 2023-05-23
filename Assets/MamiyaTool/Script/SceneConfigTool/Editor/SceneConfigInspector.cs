using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MamiyaTool {
    [CustomEditor(typeof(SceneConfig))]
    public class SceneConfigInspector : Editor {
        private SceneConfig m_Target;

        private void Awake() {
            m_Target = (SceneConfig)target;
        }
        public override void OnInspectorGUI() {
            serializedObject.Update();
            // Area
            EditorGUILayout.BeginHorizontal();
            DrawProperty("Area");
            EditorGUILayout.EndHorizontal();
            // Scene
            EditorGUILayout.BeginHorizontal();
            DrawProperty("Scene.asset", new GUIContent("Scene"));
            if(m_Target.Scene.Asset == null)
                m_Target.Scene.name = "";
            else if(m_Target.Scene.name != m_Target.Scene.Asset.name)
                m_Target.Scene.name = m_Target.Scene.Asset.name;
            EditorGUILayout.EndHorizontal();
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawProperty(string path, GUIContent label = null) {
            SerializedProperty prop = FindProperty(path);
            if(prop == null)
                return;
            if(label == null)
                EditorGUILayout.PropertyField(prop);
            else
                EditorGUILayout.PropertyField(prop, label);
        }
        private SerializedProperty FindProperty(string path) {
            if(!path.Contains('.'))
                return serializedObject.FindProperty(path);
            string[] parts = path.Split('.');
            SerializedProperty prop = serializedObject.FindProperty(parts[0]);
            for(int i = 0; i < parts.Length; ++i) {
                SerializedProperty tempProp = prop.FindPropertyRelative(parts[i]);
                if(tempProp != null)
                    prop = tempProp;
            }
            return prop;
        }
    }
}
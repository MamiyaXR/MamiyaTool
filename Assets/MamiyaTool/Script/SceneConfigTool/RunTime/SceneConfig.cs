using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

[CreateAssetMenu(fileName = "New Scene Config", menuName = "Scene Config")]
public class SceneConfig : ScriptableObject {
    public string Name;
    public Rect Area;
    public SceneAsset Scene;

    public override string ToString() {
        return $"SceneConfig : {Name}";
    }
    //public void InstantiationAreaMark(Scene scene) {
    //    GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Plugins/MamiyaTool/SceneCfgMgr/SceneAreaMark.prefab");
    //    if(prefab == null)
    //        return;
    //    GameObject ins = PrefabUtility.InstantiatePrefab(prefab, scene) as GameObject;
    //    ins.GetComponent<SceneAreaMark>().cfg = this;
    //    EditorSceneManager.MarkSceneDirty(scene);
    //}
    //public void RemoveAreaMark(Scene scene) {
    //    List<GameObject> objs = new List<GameObject>();
    //    scene.GetRootGameObjects(objs);
    //    bool result = false;
    //    foreach(GameObject obj in objs) {
    //        if(obj.name == "SceneAreaMark") {
    //            GameObject.DestroyImmediate(obj);
    //            result = true;
    //        }
    //    }
    //    //if(result)
    //    //    EditorSceneManager.SaveScene(scene);
    //}
}

[CustomEditor(typeof(SceneConfig))]
public class SceneConfigInspector : Editor {
    private SceneConfig m_Target;

    private void Awake() {
        m_Target = (SceneConfig)target;
    }
    public override void OnInspectorGUI() {
        serializedObject.Update();
        // Name
        EditorGUILayout.BeginHorizontal();
        DrawProperty("Name");
        EditorGUILayout.EndHorizontal();
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
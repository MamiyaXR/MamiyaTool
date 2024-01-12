using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace MamiyaTool {
    public class SceneCfgMgrWindow : EditorWindow {
        [MenuItem("Tools/Scene Config Manager window")]
        public static void ShowWindow() {
            GetWindow(typeof(SceneCfgMgrWindow), false, "Scene Config Manager");
        }
        /*****************************************************************
         * 
         *      field
         * 
         *****************************************************************/
        private List<SceneConfig> sceneCfgs;
        private GUISkin skin;
        private Tab tab = Tab.SceneList;
        private ErrorMessage curError;
        private string searchField = "";
        private Vector2 scrollPos = Vector2.zero;
        private BuildScenesMgr buildScenes = new BuildScenesMgr();
        private SceneAsset launchScene;
        private SceneAreaMark areaMark;

        private const string LaunchSceneMissErrMsg = "Error : LaunchScene is missing!";
        private const string LaunchSceneNotInBuildErrMsg = "Error : LaunchScene should be in the build settings!";
        private const string NoProblemMsg = "Everything is working correctly!";

        public static string LaunchScenePath {
            get { return EditorPrefs.GetString("LaunchScenePath", ""); }
            set { EditorPrefs.SetString("LaunchScenePath", value); }
        }
        /*****************************************************************
         * 
         *      lifecycle
         * 
         *****************************************************************/
        private void OnEnable() {
            sceneCfgs = new List<SceneConfig>();
            string[] skinsGUID = AssetDatabase.FindAssets("SCEMAWindowSkin");
            skin = LoadAsset<GUISkin>(skinsGUID[0]);
            Refresh();
        }
        private void OnGUI() {
            string title = "Scene Config Manager";
            switch(tab) {
                case Tab.Tester:
                    title += " - Setup Wizard";
                    break;
            }

            // Header
            GUI.Label(new Rect(0, 0, position.width, 40), title, skin.FindStyle("Header"));
            GUI.Label(new Rect(0, 40, 120, 20), "Launch Scene Path : ", GUI.skin.label);
            LaunchScenePath = GUI.TextField(new Rect(120, 40, position.width - 120, 20), LaunchScenePath);

            if(GUI.Button(new Rect(position.width - 30, 10, 20, 20), new GUIContent("", "Refresh"), skin.FindStyle("Refresh"))) {
                Refresh();
                return;
            }
            if(GUI.Button(new Rect(position.width - 55, 10, 20, 20), new GUIContent("", "View Area"), skin.FindStyle(areaMark == null ? "OFF" : "ON"))) {
                if(areaMark == null)
                    LoadAreaMarkScene();
                else
                    UnLoadAreaMarkScene();
                return;
            }
            if(GUI.Button(new Rect(position.width - 80, 10, 20, 20), new GUIContent("", "Setup Wizard"), skin.FindStyle(tab == Tab.Tester ? "Setup-ON" : curError != ErrorMessage.NoProblem ? "Setup-Alert" : "Setup"))) {
                tab = tab == Tab.Tester ? Tab.SceneList : Tab.Tester;
            }
            // Content
            Rect rect = new Rect(0, 60, position.width, position.height - 50);
            switch(tab) {
                case Tab.SceneList:
                    DrawSearchField(new Rect(rect.x, rect.y, rect.width - 20, 20));
                    DrawList(new Rect(rect.x, rect.y + 20, rect.width, rect.height - 20), sceneCfgs);
                    break;
                case Tab.Tester:
                    SearchForProblems();
                    DrawError(rect);
                    break;
            }
        }
        /*****************************************************************
         * 
         *      private method
         * 
         *****************************************************************/
        private void Refresh() {
            SearchForProblems();
            string[] sceneMarkGUIDs = AssetDatabase.FindAssets("t:SceneConfig");
            sceneCfgs.Clear();
            foreach(var guid in sceneMarkGUIDs) {
                SceneConfig cfg = LoadAsset<SceneConfig>(guid);
                sceneCfgs.Add(cfg);
            }
        }
        private T LoadAsset<T>(string guid) where T : Object {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            return AssetDatabase.LoadAssetAtPath<T>(path);
        }
        private void SearchForProblems() {
            launchScene = GetLaunchScene();

            if(launchScene == null)
                curError = ErrorMessage.LaunchSceneMissing;
            else if(!buildScenes.Contains(launchScene))
                curError = ErrorMessage.LaunchSceneNotInBuildSettings;
            else
                curError = ErrorMessage.NoProblem;
        }
        private SceneAsset GetLaunchScene() {
            //string[] launchGUID = AssetDatabase.FindAssets(LaunchScenePath);
            SceneAsset launch = null;
            //foreach(var guid in launchGUID) {
            //    var asset = LoadAsset<UnityEditor.SceneAsset>(guid);
            //    if(asset != null && (asset is UnityEditor.SceneAsset)) {
            //        launch = new SceneAsset(asset);
            //        break;
            //    }
            //}
            var asset = AssetDatabase.LoadAssetAtPath<UnityEditor.SceneAsset>(LaunchScenePath);
            if(asset != null && (asset is UnityEditor.SceneAsset))
                launch = new SceneAsset(asset);
            return launch;
        }
        private void LoadAreaMarkScene() {
            string[] guid = AssetDatabase.FindAssets("SceneAreaMark t:scene");
            string path = AssetDatabase.GUIDToAssetPath(guid[0]);
            var scene = EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);
            GameObject[] objs = scene.GetRootGameObjects();
            foreach(var obj in objs) {
                if(obj.name == "Mark") {
                    areaMark = obj.GetComponent<SceneAreaMark>();
                    Selection.activeObject = obj;
                    return;
                }
            }
        }
        private void UnLoadAreaMarkScene() {
            string[] guid = AssetDatabase.FindAssets("SceneAreaMark t:scene");
            string path = AssetDatabase.GUIDToAssetPath(guid[0]);
            var scene = EditorSceneManager.GetSceneByPath(path);
            EditorSceneManager.CloseScene(scene, true);
        }

        #region draw method
        /// <summary>
        /// 绘制搜索框
        /// </summary>
        /// <param name="rect">矩形范围</param>
        private void DrawSearchField(Rect rect) {
            searchField = EditorGUI.TextField(rect, "", searchField, (GUIStyle)"SearchTextField");
            rect.x += rect.width;
            rect.width = 20;
            if(GUI.Button(rect, "", (GUIStyle)"SearchCancelButton")) {
                searchField = "";
                GUI.SetNextControlName("");
                GUI.FocusControl("");
            }
        }
        /// <summary>
        /// 绘制列表
        /// </summary>
        /// <param name="rect">矩形范围</param>
        /// <param name="list">数据</param>
        private void DrawList(Rect rect, List<SceneConfig> list) {
            scrollPos = GUI.BeginScrollView(rect, scrollPos, new Rect(0, 0, position.width - 20, sceneCfgs.Count * 20));

            Scene activeScene = EditorSceneManager.GetActiveScene();
            DrawLaunchScene(activeScene);
            for(int i = 0; i < list.Count; i++) {
                SceneConfig mark = sceneCfgs[i];
                if(mark == null)
                    continue;
                if(!mark.name.ToLower().Contains(searchField.ToLower()))
                    continue;
                DrawElement(mark, activeScene);
            }
            GUI.EndScrollView();
        }
        private GUIStyle GetElementStyle(bool active) {
            return active ? new GUIStyle("ObjectPickerBackground") : GUIStyle.none;
        }
        /// <summary>
        /// 绘制Launch场景
        /// </summary>
        private void DrawLaunchScene(Scene activeScene) {
            if(launchScene == null)
                return;

            bool active = activeScene.name == launchScene.Asset.name;
            
            Rect r = EditorGUILayout.BeginHorizontal(GetElementStyle(active));
            GUILayout.Space(5);
            // name
            GUILayout.Label(launchScene.Asset.name);
            GUILayout.FlexibleSpace();
            // search
            if(GUILayout.Button(new GUIContent("", "Find SceneConfig object"), skin.FindStyle("Search"), GUILayout.Width(20), GUILayout.Height(20)))
                EditorGUIUtility.PingObject(launchScene.Asset);
            LoadSceneControl(launchScene);
            EditorGUILayout.EndHorizontal();
            // list item button
            if(GUI.Button(r, "", GUIStyle.none)) {
                Scene scene = new Scene();
                if(IsSceneLoaded(launchScene, ref scene))
                    EditorSceneManager.SetActiveScene(scene);
            }
        }
        /// <summary>
        /// 绘制列表元素
        /// </summary>
        /// <param name="mark">数据</param>
        private void DrawElement(SceneConfig mark, Scene activeScene) {
            if(mark == null || mark.Scene.Asset == null)
                return;

            bool active = activeScene.name == mark.Scene.Asset.name;

            Rect r = EditorGUILayout.BeginHorizontal(GetElementStyle(active));
            GUILayout.Space(5);
            BuildSettingsControl(mark.Scene);
            GUILayout.Label(mark.name);
            //GUILayout.Space(5);
            //GUILayout.Label($"{mark.Area}");
            GUILayout.FlexibleSpace();
            // search
            if(GUILayout.Button(new GUIContent("", "Find SceneConfig object"), skin.FindStyle("Search"), GUILayout.Width(20), GUILayout.Height(20))) {
                EditorGUIUtility.PingObject(mark);
                Selection.activeObject = mark;
            }
            LoadSceneControl(mark.Scene);
            LoadAddSceneControl(mark);

            EditorGUILayout.EndHorizontal();
            // list item button
            if(GUI.Button(r, "", GUIStyle.none)) {
                SceneView.lastActiveSceneView.Frame(mark.Area);
                Scene scene = new Scene();
                if(IsSceneLoaded(mark.Scene, ref scene))
                    EditorSceneManager.SetActiveScene(scene);
            }
        }
        /// <summary>
        /// 检测并绘制buildsetting按钮
        /// </summary>
        /// <param name="scene">数据</param>
        private void BuildSettingsControl(SceneAsset scene) {
            bool inBuildSettings = buildScenes.Contains(scene);
            GUIStyle buildSettingButton = skin.FindStyle(inBuildSettings ? "ON" : "OFF");
            if(DrawLayoutButton(buildSettingButton, scene == null || scene.Asset == null, new GUIContent("", inBuildSettings ? "Remove from build settings" : "Add to build settings"))) {
                if(inBuildSettings)
                    buildScenes.Remove(scene);
                else
                    buildScenes.Add(scene);
            }
        }
        /// <summary>
        /// 检测并绘制以Single模式加载场景的按钮
        /// </summary>
        /// <param name="scene"></param>
        private void LoadSceneControl(SceneAsset scene) {
            bool isLoaded = IsSceneLoaded(scene);
            if(DrawLayoutButton(skin.FindStyle(isLoaded ? "Open" : "Close"), scene == null || scene.Asset == null, new GUIContent("", "Open Scene"))) {
                EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                var scenePath = AssetDatabase.GetAssetPath(scene.Asset);
                //OpenSceneMode mode = Event.current.shift ? OpenSceneMode.Additive : OpenSceneMode.Single;
                EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
            }
        }
        /// <summary>
        /// 检测并绘制以Add模式加载场景的按钮
        /// </summary>
        /// <param name="scene"></param>
        private void LoadAddSceneControl(SceneConfig cfg) {
            SceneAsset scene = cfg.Scene;
            bool isLoaded = IsSceneLoaded(scene);
            if(DrawLayoutButton(skin.FindStyle(isLoaded ? "Settings-ON" : "Settings"), scene == null || scene.Asset == null, new GUIContent("", "Add Scene"))) {
                EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                var scenePath = AssetDatabase.GetAssetPath(scene.Asset);
                if(isLoaded) {
                    var sceneStruct = EditorSceneManager.GetSceneByPath(scenePath);
                    EditorSceneManager.CloseScene(sceneStruct, true);
                } else {
                    EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);
                }
            }
        }
        /// <summary>
        /// 检测场景是否已加载
        /// </summary>
        /// <param name="scene">场景资源数据</param>
        /// <returns></returns>
        private bool IsSceneLoaded(SceneAsset scene) {
            bool result = false;
            if(scene != null) {
                for(int cnt = 0; cnt < EditorSceneManager.sceneCount; cnt++) {
                    var s = EditorSceneManager.GetSceneAt(cnt);
                    if(scene.Asset != null && s.name == scene.Asset.name) {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }
        private bool IsSceneLoaded(SceneAsset asset, ref Scene scene) {
            bool result = false;
            if(asset != null) {
                for(int cnt = 0; cnt < EditorSceneManager.sceneCount; cnt++) {
                    scene = EditorSceneManager.GetSceneAt(cnt);
                    if(asset.Asset != null && scene.name == asset.Asset.name) {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// 绘制按钮
        /// </summary>
        /// <param name="style"></param>
        /// <param name="hide"></param>
        /// <param name="label"></param>
        /// <returns></returns>
        private bool DrawLayoutButton(GUIStyle style, bool hide, GUIContent label = null) {
            if(label == null)
                label = GUIContent.none;

            if(hide) {
                GUILayout.Label(label, skin.label, GUILayout.Width(20));
                return false;
            }
            return GUILayout.Button(label, style, GUILayout.Width(20), GUILayout.Height(20));
        }
        /// <summary>
        /// 绘制错误信息
        /// </summary>
        /// <param name="rect"></param>
        private void DrawError(Rect rect) {
            switch(curError) {
                case ErrorMessage.LaunchSceneMissing:
                    if(DrawTestResult(ref rect, LaunchSceneMissErrMsg)) {
                        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
                        if(EditorSceneManager.SaveScene(scene, LaunchScenePath)) {
                            Debug.Log("Launch created");
                        }
                    }
                    break;
                case ErrorMessage.LaunchSceneNotInBuildSettings:
                    if(DrawTestResult(ref rect, LaunchSceneNotInBuildErrMsg)) {
                        buildScenes.Add(launchScene);
                        Debug.Log("Launch is added to build settings.");
                    }
                    break;
                case ErrorMessage.NoProblem:
                    rect.height = 30f;
                    EditorGUI.HelpBox(rect, NoProblemMsg, MessageType.Info);
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 绘制提示框
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private bool DrawTestResult(ref Rect rect, string msg) {
            rect.height = 30f;
            EditorGUI.HelpBox(rect, msg, MessageType.Error);
            rect.y += rect.height;
            rect.height = 20;

            if(GUI.Button(rect, "Fix this problem")) {
                rect.y += rect.height;
                return true;
            } else
                return false;
        }
        #endregion
        /*****************************************************************
         * 
         *      enum
         * 
         *****************************************************************/
        private enum Tab {
            SceneList,
            Tester,
        }
        private enum ErrorMessage {
            NoProblem,
            LaunchSceneMissing,
            LaunchSceneNotInBuildSettings,
        }
    }
}
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.SceneManagement;

namespace MamiyaTool {
    public class SceneViewTools {
        //右键合法性
        static bool rightLegal;
        static bool RightMouseDown;

        [InitializeOnLoadMethod]
        static void init() {
            rightLegal = false;
            RightMouseDown = false;
            SceneView.duringSceneGui += OnSceneGUI;
        }
        private static void OnSelect(RectTransform rectTransform) {
            Selection.activeTransform = rectTransform;
            EditorGUIUtility.PingObject(rectTransform);
        }

        static void OnSceneGUI(SceneView sceneView) {
            if(Event.current == null || Event.current.button != 1) {
                return;
            }

            if(Event.current.type == EventType.MouseDown) {
                rightLegal = true;
                RightMouseDown = true;
            }

            if(Event.current.type == EventType.MouseDrag && RightMouseDown) {
                rightLegal = false;
            }

            if(Event.current.type == EventType.MouseUp && RightMouseDown && rightLegal) {
                var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();

                List<RectTransform> inSceneObjs = new List<RectTransform>();
                if(prefabStage != null) {

                    RectTransform[] allObjects = prefabStage.prefabContentsRoot.GetComponentsInChildren<RectTransform>();
                    foreach(RectTransform rect in allObjects) {
                        inSceneObjs.Add(rect);
                    }
                } else {
                    RectTransform[] child = GameObject.FindObjectsOfType<RectTransform>();
                    foreach(RectTransform rect in child) {
                        //剔除Canvas物体
                        if(rect.name.Contains("Canvas"))
                            continue;
                        inSceneObjs.Add(rect);
                    }
                }
                if(rightLegal)
                    if(inSceneObjs.Count == 0) {
                        RightMouseDown = false;
                        rightLegal = false;
                        return;
                    }
                GenericMenu menuView = new GenericMenu();
                Dictionary<string, int> nameNumDict = new Dictionary<string, int>();
                Camera camera = SceneView.currentDrawingSceneView.camera; //获取到编辑器模式下的相机，这个相机是看不到的，但是可以拿到
                Vector3 pos = Event.current.mousePosition; //低版本可能要×2
                pos = new Vector3(pos.x, camera.pixelHeight - pos.y);
                for(int index = inSceneObjs.Count - 1; index >= 0; index--) {
                    var rect = inSceneObjs[index];

                    if(RectTransformUtility.RectangleContainsScreenPoint(rect, pos, camera)) {
                        string name = rect.name;
                        bool changeName = false;
                        if(nameNumDict.ContainsKey(name)) {
                            nameNumDict[name]++;
                            changeName = true;
                        } else {
                            nameNumDict.Add(name, 1);
                        }
                        if(changeName) {
                            int num = nameNumDict[name] - 1;
                            name += "[" + num + "]";
                        }


                        menuView.AddItem(new GUIContent(name), false, () => {
                            OnSelect(rect);
                        });
                    }
                }
                RightMouseDown = false;
                rightLegal = false;
                //没有执行出来，不能设置为Used 会有莫名报错
                if(menuView.GetItemCount() > 0) {
                    menuView.ShowAsContext();
                    Event.current.Use();
                }
            }
        }
    }
}
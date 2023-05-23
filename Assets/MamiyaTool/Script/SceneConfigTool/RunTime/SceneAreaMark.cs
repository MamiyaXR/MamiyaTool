using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace MamiyaTool {
    public class SceneAreaMark : MonoBehaviour {
        public Color color = Color.green;
        /*****************************************************************
         * 
         *      lifecycle
         * 
         *****************************************************************/
        private void OnDrawGizmos() {
            Gizmos.color = color;
            string[] sceneMarkGUIDs = AssetDatabase.FindAssets("t:SceneConfig");
            foreach(var guid in sceneMarkGUIDs) {
                SceneConfig cfg = LoadAsset<SceneConfig>(guid);
                if(IsSceneLoaded(cfg.Scene))
                    DrawOneScene(cfg);
            }
        }
        /*****************************************************************
         * 
         *      private method
         * 
         *****************************************************************/
        private void DrawOneScene(SceneConfig cfg) {
            if(cfg == null)
                return;
            Gizmos.DrawWireCube(cfg.Area.center, cfg.Area.size);
        }
        private T LoadAsset<T>(string guid) where T : Object {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            return AssetDatabase.LoadAssetAtPath<T>(path);
        }
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
    }
}
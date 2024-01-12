using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace MamiyaTool {
    public class BuildScenesMgr {
        /// <summary>
        /// 查询目标scene是否在buildSettingsScene列表中
        /// </summary>
        /// <param name="scenePath">目标scene路径</param>
        /// <param name="sceneList">buildSettingScene列表</param>
        /// <returns>结果</returns>
        public bool Contains(string scenePath, List<EditorBuildSettingsScene> sceneList = null) {
            if(sceneList == null)
                sceneList = EditorBuildSettings.scenes.ToList();

            foreach(var s in sceneList) {
                if(s.path == scenePath)
                    return true;
            }

            return false;
        }
        /// <summary>
        /// 查询目标scene是否已被添加到BuildSetting
        /// </summary>
        /// <param name="scene">目标scene</param>
        /// <returns>结果</returns>
        public bool Contains(SceneAsset scene) {
            string scenePath = AssetDatabase.GetAssetPath(scene.Asset);
            return Contains(scenePath);
        }
        public void Add(SceneAsset scene) {
            string scenePath = AssetDatabase.GetAssetPath(scene.Asset);
            var sceneList = EditorBuildSettings.scenes.ToList();

            if(!Contains(scenePath, sceneList)) {
                var buildSettingScene = new EditorBuildSettingsScene(scenePath, true);
                sceneList.Add(buildSettingScene);
                EditorBuildSettings.scenes = sceneList.ToArray();
            }
        }
        /// <summary>
        /// 将目标scene从BuildSetting中移除
        /// </summary>
        /// <param name="scene">目标scene</param>
        public void Remove(SceneAsset scene) {
            string scenePath = AssetDatabase.GetAssetPath(scene.Asset);
            var sceneList = EditorBuildSettings.scenes.ToList();

            int cnt = sceneList.RemoveAll(x => x.path == scenePath);
            if(cnt > 0) {
                EditorBuildSettings.scenes = sceneList.ToArray();
            }
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MamiyaTool {
    [Serializable]
    public class SceneAsset {
        public string name = "";

#if UNITY_EDITOR
        public UnityEditor.SceneAsset Asset {
            get { return asset; }
            set {
                asset = value;
                name = asset != null ? asset.name : "";
            }
        }
        [SerializeField] private UnityEditor.SceneAsset asset;
        public SceneAsset(UnityEditor.SceneAsset scene) {
            Asset = scene;
        }
#endif
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MamiyaTool {
    [CreateAssetMenu(fileName = "New Scene Config", menuName = "Scene Config")]
    public class SceneConfig : ScriptableObject {
        public Bounds Area;
        public SceneAsset Scene;

        public override string ToString() {
            return $"SceneConfig : {name}";
        }
    }
}
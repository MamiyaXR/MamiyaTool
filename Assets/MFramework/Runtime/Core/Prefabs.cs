using System.Collections.Generic;
using UnityEngine;

namespace MFramework {
    public class Prefabs : MonoBehaviour {
        [SerializeField]
        private GameObject[] prefabs;

        public GameObject this[int index] => GetPrefab(index);
        public GameObject this[string name] => GetPrefab(name);

        private Dictionary<string, GameObject> _dict;
        private Dictionary<string, GameObject> Dict {
            get {
                if(_dict == null) {
                    _dict = new Dictionary<string, GameObject>();
                    foreach(var prefab in prefabs) {
                        _dict.Add(prefab.name, prefab);
                    }
                }
                return _dict;
            }
        }

        public GameObject GetPrefab(int index) {
            if(0 <= index && index < prefabs.Length) {
                return prefabs[index];
            }
            return null;
        }

        public GameObject GetPrefab(string name) {
            if(Dict.TryGetValue(name, out GameObject prefab)) {
                return prefab;
            }
            return null;
        }
    }
}
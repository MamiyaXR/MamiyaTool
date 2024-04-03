using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MFramework {
    public class MVCSettings : ScriptableObject {
        public static MVCSettings Inst { get; private set; }
        private static string[] empty = new string[0];
        public static string[] WinGroups => Inst ? Inst.winGroups : empty;
        public string[] winGroups = { "Bottom", "Normal", "Top" };
#if UNITY_EDITOR
        private static string Path = "Assets/MVCSettings.asset";
        [InitializeOnLoadMethod]
        private static void Init() {
            if(!Inst) {
                if(!File.Exists(Path)) {
                    Inst = CreateInstance<MVCSettings>();
                    AssetDatabase.CreateAsset(Inst, Path);
                    AssetDatabase.Refresh();
                } else {
                    Inst = AssetDatabase.LoadAssetAtPath<MVCSettings>(Path);
                }
            }
        }
#endif
    }
}
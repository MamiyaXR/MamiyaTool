#if UNITY_EDITOR
using UnityEditor;

namespace MamiyaTool {
    public class EditorPrefsBoolProperty : BindableProperty<bool> {
        public EditorPrefsBoolProperty(string key, bool initValue = false) {
            mValue = EditorPrefs.GetBool(key, initValue);
            Register(value => { EditorPrefs.SetBool(key, value); });
        }
    }
}
#endif
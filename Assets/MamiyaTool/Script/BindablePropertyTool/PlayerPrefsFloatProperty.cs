using UnityEngine;

namespace MamiyaTool {
    public class PlayerPrefsFloatProperty : BindableProperty<float> {
        public PlayerPrefsFloatProperty(string saveKey, float defaultValue = 0f) {
            mValue = PlayerPrefs.GetFloat(saveKey, defaultValue);
            Register(value => PlayerPrefs.SetFloat(saveKey, value));
        }
    }
}
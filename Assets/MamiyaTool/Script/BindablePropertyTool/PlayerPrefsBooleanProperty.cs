using UnityEngine;

namespace MamiyaTool {
    public class PlayerPrefsBooleanProperty : BindableProperty<bool> {
        public PlayerPrefsBooleanProperty(string saveKey, bool defaultValue = false) {
            mValue = PlayerPrefs.GetInt(saveKey, defaultValue ? 1 : 0) == 1;
            Register(value => PlayerPrefs.SetInt(saveKey, value ? 1 : 0));
        }
    }
}
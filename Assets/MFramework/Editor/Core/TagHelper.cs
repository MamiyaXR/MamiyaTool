using UnityEngine;
using UnityEngine.UI;

namespace MFramework {
    public static class TagHelper {
        public static Component GetCompByTag(Component comp) {
            Component r = null;
            switch(comp.gameObject.tag) {
                case "UI/RectTransform":
                    r = comp.GetComponent<RectTransform>();
                    break;
                case "UI/Button":
                    r = comp.GetComponent<Button>();
                    break;
                case "UI/Toggle":
                    r = comp.GetComponent<Toggle>();
                    break;
                case "UI/InputField":
                    r = comp.GetComponent<InputField>();
                    break;
                case "UI/Image":
                    r = comp.GetComponent<Image>();
                    if(!r)
                        r = comp.GetComponent<RawImage>();
                    break;
                case "UI/RawImage":
                    r = comp.GetComponent<RawImage>();
                    break;
                case "UI/Text":
                    r = comp.GetComponent<Text>();
                    if(!r)
                        r = comp.GetComponent<InputField>();
                    break;

                case "UI/Progress":
                    r = comp.GetComponent<Image>();
                    break;
                case "UI/Slider":
                    r = comp.GetComponent<Slider>();
                    break;
                //case "UI/Area":
                //    r = comp.GetComponent<ViewArea>();
                //    break;
                //case "UI/StateExt":
                //    r = comp.GetComponent<StateExt>();
                //    break;

                case "UI/Anim":
                    r = comp.GetComponent<AnimPlayer>();
                    break;

                case "AudioSource":
                    r = comp.GetComponent<AudioSource>();
                    break;
                case "Prefabs":
                    r = comp.GetComponent<Prefabs>();
                    break;
                default:
                    break;
            }
            return r ? r : comp;
        }
    }
}
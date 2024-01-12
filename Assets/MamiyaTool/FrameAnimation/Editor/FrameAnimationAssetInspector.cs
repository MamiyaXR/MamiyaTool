using UnityEngine;
using UnityEditor;

namespace MamiyaTool {
    [CustomEditor(typeof(FrameAnimationAsset))]
    public class FrameAnimationAssetInspector : Editor {
        private FrameAnimationAsset _target;
        private AnimationSetting animSetting;
        private bool isPlaying = false;
        private bool hasAnimation = false;
        private float speedScale = 1f;
        /***************************************************************
         * 
         *      lifecycle
         * 
         ***************************************************************/
        private void OnEnable() {
            _target = (FrameAnimationAsset)target;
            hasAnimation = false;
            isPlaying = false;
            
            if(_target != null && _target.Tracks != null) {
                foreach(var track in _target.Tracks) {
                    if(!(track is FrameSpriteTrack))
                        continue;
                    var _track = track as FrameSpriteTrack;
                    if(_track.Datas == null || _track.Datas.Count <= 0)
                        continue;

                    animSetting = new AnimationSetting(_track);
                    hasAnimation = true;
                    break;
                }
            }
        }
        public override void OnPreviewGUI(Rect r, GUIStyle background) {
            if(Application.isPlaying || !hasAnimation) {
                base.OnPreviewGUI(r, background);
                return;
            }

            animSetting.delta += ((float)EditorApplication.timeSinceStartup - animSetting.lastTime) * speedScale;
            animSetting.lastTime = (float)EditorApplication.timeSinceStartup;

            if(isPlaying) {
                float rate = 1f / _target.SampleRate;
                if(rate < animSetting.delta) {
                    animSetting.delta = Mathf.Repeat(animSetting.delta, rate);
                    animSetting.index++;
                }
                animSetting.index %= animSetting.anim.Datas.Count;
            }

            Sprite sprite = animSetting.anim.Datas[animSetting.index].Frame;
            EditorGUI.DropShadowLabel(r, sprite.name);
            r.height -= 15;
            Texture2D tex = AssetPreview.GetAssetPreview(sprite);
            if(tex != null)
                GUI.DrawTexture(r, tex, ScaleMode.ScaleToFit);
        }
        public override void OnPreviewSettings() {
            base.OnPreviewSettings();
            if(!hasAnimation)
                return;
            GUIContent playBtn = EditorGUIUtility.IconContent("preAudioPlayOn");
            GUIContent pauseBtn = EditorGUIUtility.IconContent("preAudioPlayOff");

            EditorGUI.BeginChangeCheck();
            isPlaying = GUILayout.Toggle(isPlaying, isPlaying ? playBtn : pauseBtn, "preButton");
            if(EditorGUI.EndChangeCheck()) {
                animSetting.lastTime = (float)EditorApplication.timeSinceStartup;
            }

            GUIContent _speedScale = EditorGUIUtility.IconContent("SpeedScale", "Speed Scale");
            if(GUILayout.Button(_speedScale, "preButton")) {
                speedScale = 1;
            }
            speedScale = GUILayout.HorizontalSlider(speedScale, 0f, 1f, "preSlider", "preSliderThumb");
            GUILayout.Box(speedScale.ToString("0.000"), new GUIStyle("preLabel"));
        }
        public override bool HasPreviewGUI() {
            return hasAnimation;
        }
        public override bool RequiresConstantRepaint() {
            return isPlaying;
        }
        public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height) {
            FrameAnimationAsset asset = AssetDatabase.LoadAssetAtPath<FrameAnimationAsset>(assetPath);
            if(asset != null) {
                foreach(var track in asset.Tracks) {
                    if(!(track is FrameSpriteTrack))
                        continue;
                    var _track = track as FrameSpriteTrack;
                    if(_track.Datas == null || _track.Datas.Count <= 0)
                        continue;

                    foreach(var data in _track.Datas) {
                        if(data != null && data.Frame != null) {
                            Texture2D preview = AssetPreview.GetAssetPreview(data.Frame);
                            if(preview != null) {
                                Texture2D tex = new Texture2D(width, height);
                                EditorUtility.CopySerialized(preview, tex);
                                return tex;
                            }
                            break;
                        }
                    }
                    break;
                }
            }
            return base.RenderStaticPreview(assetPath, subAssets, width, height);
        }
        /***************************************************************
         * 
         *      define
         * 
         ***************************************************************/
        private class AnimationSetting {
            public int index;
            public float delta;
            public float lastTime;
            public FrameSpriteTrack anim;

            public AnimationSetting(FrameSpriteTrack anim) {
                index = 0;
                delta = 0f;
                lastTime = (float)EditorApplication.timeSinceStartup;
                this.anim = anim;
            }
        }
    }
}
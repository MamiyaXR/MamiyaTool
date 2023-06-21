using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace MamiyaTool {
    internal class FrameAnimWinSelectionItem : IEquatable<FrameAnimWinSelectionItem> {
        #region private field
        private int m_Id;
        private GameObject m_GameObject;
        private FrameAnimationAsset m_AnimationAsset;
        private FrameAnimation m_AnimationClip;
        private bool m_Previewing;
        private bool m_Recording;
        private bool m_Playing;
        private AnimationKeyTime m_Time;

        private float m_LastUpdateTime;
        #endregion

        #region public field
        public WindowState state;
        #endregion

        #region accessor
        public int id { get { return m_Id; } set { m_Id = value; } }
        public GameObject gameObject {
            get => m_GameObject;
            set {
                if(m_GameObject == value)
                    return;
                m_GameObject = value;
                animationClip = null;
                animationClip = animationClip;
            }
        }
        public FrameAnimationAsset animationAsset {
            get => m_AnimationAsset;
            set {
                if(m_AnimationAsset == value)
                    return;
                m_AnimationAsset = value;
                animationClip = null;
                animationClip = animationClip;
            }
        }
        public FrameAnimation animationClip {
            get {
                if(m_AnimationClip == null) {
                    if(animationAsset != null && rootGameObject != null)
                        m_AnimationClip = new FrameAnimation(rootGameObject.transform, animationAsset);
                }
                return m_AnimationClip;
            }
            set {
                m_AnimationClip = value;
            }
        }
        public GameObject rootGameObject {
            get {
                Component animationPlayer = this.animationPlayer;
                if(animationPlayer != null) {
                    return animationPlayer.gameObject;
                }
                return null;
            }
        }
        public FrameAnimator animationPlayer {
            get {
                if(gameObject != null)
                    return FrameAnimWinUtility.GetClosestAnimationPlayerComponentInParents(gameObject.transform);
                return null;
            }
        }

        public bool disable {
            get {
                return animationAsset == null;
            }
        }
        public bool animationIsEditable {
            get {
                if(animationAsset == null)
                    return false;
                return true;
            }
        }
        public bool objectIsPrefab {
            get {
                // No gameObject selected
                if(!gameObject)
                    return false;

                if(EditorUtility.IsPersistent(gameObject))
                    return true;

                if((gameObject.hideFlags & HideFlags.NotEditable) != 0)
                    return true;

                return false;
            }
        }
        public bool objectIsOptimized {
            get {
                FrameAnimator animator = animationPlayer as FrameAnimator;
                if(animator == null)
                    return false;

                return true;
            }
        }
        public bool canPreview {
            get {
                if(rootGameObject != null) {
                    return objectIsOptimized;
                }

                return false;
            }
        }
        public bool canRecord {
            get {
                if(!animationIsEditable)
                    return false;

                return canPreview;
            }
        }
        public bool canPlay {
            get {
                if(animationAsset == null)
                    return false;
                return canPreview;
            }
        }
        public bool canChangeAnimationClip {
            get {
                return rootGameObject != null;
            }
        }
        public bool canSyncSceneSelection { get { return true; } }

        public bool playing {
            get => m_Playing;
            set {
                if(!canPlay || recording)
                    return;
                if(m_Playing == value)
                    return;
                if(value) {
                    if(animationClip != null) {
                        m_Playing = value;
                        animationClip.Play();
                    }
                } else {
                    m_Playing = value;
                    if(animationClip != null) {
                        animationClip.Reset();
                        animationClip = null;
                    }
                }
            }
        }
        public bool previewing { get; set; }
        public bool recording { get; set; }
        public float time {
            get => m_Time.time;
            set => SetCurrentFrame(value);
        }
        public int frame {
            get {
                if(animationClip == null)
                    return 0;
                return (int)(curFrameInfo.Value.GetValue(animationClip));
            }
            set {
                if(animationClip == null)
                    return;
                if(!previewing)
                    return;
                curFrameInfo.Value.SetValue(animationClip, value);
                setFrame.Value.Invoke(animationClip, new object[] { value });
            }
        }
        #endregion
        /*****************************************************************
         * 
         *      public method
         * 
         *****************************************************************/
        public bool PlaybackUpdate() {
            float curTime = Time.realtimeSinceStartup;
            m_LastUpdateTime = Mathf.Min(m_LastUpdateTime, curTime);
            float deltaTime = curTime - m_LastUpdateTime;
            m_LastUpdateTime = curTime;

            if(playing) {
                animationClip.Update(deltaTime * animationPlayer.PlaySpeed);
                return true;
            }

            return false;
        }
        public bool Equals(FrameAnimWinSelectionItem other) {
            return id == other.id &&
                animationAsset == other.animationAsset &&
                gameObject == other.gameObject;
        }
        /*****************************************************************
         * 
         *      private method
         * 
         *****************************************************************/
        private void SetCurrentFrame(float value) {
            if(!Mathf.Approximately(value, time)) {
                m_Time = AnimationKeyTime.Time(value, state.frameRate);
                StartPreview();
                ResampleAnimation();
            }
        }
        private void StartPreview() {
            if(previewing || !canPreview)
                return;
            previewing = true;
        }
        private void ResampleAnimation() {
            if(state.disabled)
                return;
            if(!canPreview || !previewing)
                return;
        }
        /*****************************************************************
         * 
         *      static method
         * 
         *****************************************************************/
        static BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
        static Lazy<Type> frameAnimType = new Lazy<Type>(() => {
            var type = typeof(FrameAnimation);
            return type;
        });
        static Lazy<FieldInfo> curFrameInfo = new Lazy<FieldInfo>(() => {
            var type = frameAnimType.Value;
            return type.GetField("curFrame", flags);
        });
        static Lazy<MethodInfo> setFrame = new Lazy<MethodInfo>(() => {
            var type = frameAnimType.Value;
            return type.GetMethod("SetFrame", flags, null, new Type[] { typeof(int) }, null);
        });
        public static FrameAnimWinSelectionItem Create(GameObject gameObject, FrameAnimationAsset asset) {
            var result = new FrameAnimWinSelectionItem();
            result.gameObject = gameObject;
            result.animationAsset = asset;
            return result;
        }
    }
}
using System;
using UnityEngine;
#if DOTWEEN
using DG.Tweening;
#endif

namespace MFramework {
    [ExecuteInEditMode]
    public class AnimPlayer : MonoBehaviour, IUpdateable {
        public string animName;
        public bool autoPlay = true;
        public bool realTime = true;
        [SerializeField, ReadOnly] private float duration = 0f;
        [SerializeReference] public AnimClip[] clips;

        public float Duration => duration;
        public bool Playing { get; private set; }

        public Action onPlay;
        public Action onStop;

        private bool _inited = false;
        /******************************************************************
         * 
         *      lifecycle
         * 
         ******************************************************************/
        private void Awake() {
            InitClips(false);
        }
        private void OnEnable() {
            if(autoPlay && Application.isPlaying)
                Play();
        }
        private void OnDisable() {
            if(autoPlay && Application.isPlaying)
                Stop();
        }
        /******************************************************************
         * 
         *      public method
         * 
         ******************************************************************/
        public void Play() {
            Stop();

            InitClips(false);

            foreach(var clip in clips)
                clip.Reset();

            Playing = true;

            foreach(var clip in clips) {
                if(!clip.enabled)
                    continue;
                clip.Play();
            }

            try {
                onPlay?.Invoke();
            } catch(Exception e) {
                Debug.LogException(e);
            }

            UpdaterService.AddUpdater(this);
        }
        public void Stop() {
            if(!Playing)
                return;

            UpdaterService.RemoveUpdater(this);

            foreach(var clip in clips)
                clip.Stop();

            try {
                onStop?.Invoke();
            } catch(Exception e) {
                Debug.LogException(e);
            }

            Playing = false;
        }
        void IUpdateable.Update() {
            if(!Playing)
                return;
            var playing = false;
            foreach(var clip in clips) {
                if(!clip.enabled)
                    continue;
                clip.Update();
                playing |= clip.Playing;
            }
            if(!playing)
                Stop();
        }
        public static AnimPlayer GetPlayer(GameObject target) {
            if(!target)
                return null;
            return target.GetComponent<AnimPlayer>();
        }
        public static AnimPlayer GetPlayer(GameObject target, string animName, bool includeChildren = false) {
            if(!target)
                return null;
            var aps = includeChildren ? target.GetComponentsInChildren<AnimPlayer>() : target.GetComponents<AnimPlayer>();
            return Array.Find(aps, x => x.animName == animName);
        }
        /******************************************************************
         * 
         *      private method
         * 
         ******************************************************************/
        private void InitClips(bool absolutely) {
            if(!absolutely && _inited)
                return;

            _inited = true;
            clips ??= new AnimClip[0];

            duration = 0f;
            foreach(var clip in clips) {
                if(clip != null) {
                    clip.Init(this);
                    duration = Mathf.Max(duration, clip.time.TotalTime);
                }
            }
        }
        /******************************************************************
         * 
         *      editor method
         * 
         ******************************************************************/
#if UNITY_EDITOR
#if DOTWEEN
        public static Action<Tween> OnAddTween;
#endif
        [ContextMenu("Play")]
        private void PlayInEditor() {
            gameObject.SetActive(true);
            InitClips(true);
            Play();
        }
        [ContextMenu("Stop")]
        private void StopInEditor() {
            Stop();
            if(clips != null) {
                foreach(var clip in clips)
                    clip.Reset();
            }
        }
#endif
    }
}
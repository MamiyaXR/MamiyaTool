using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MamiyaTool {
    [DisallowMultipleComponent]
    public class FrameAnimator : MonoBehaviour {
        public List<FrameAnimationAsset> animations;
        public string defaultAnim;
        [SerializeField]
        [Range(0f, 1f)]
        private float playSpeed = 1f;
        public float PlaySpeed { get => playSpeed; set => playSpeed = value; }

        public FrameAnimation CurAnim => curAnim;
        private FrameAnimation curAnim;

        public bool IsPlaying => isPlaying;
        private bool isPlaying;
        private bool CanPlay {
            get {
                bool result = curAnim != null;
                if(!result && isPlaying)
                    isPlaying = false;
                return result;
            }
        }
        /******************************************************************
         *
         *      lifecycle
         *
         ******************************************************************/
        private void Awake() {
            isPlaying = false;

            if(animations == null)
                return;
            SetAnimation(defaultAnim);
            if(curAnim != null && curAnim.PlayOnAwake)
                Play();
        }
        private void Update() {
            if(!CanPlay)
                return;
            if(!isPlaying)
                return;

            curAnim.Update(Time.deltaTime * PlaySpeed);
        }
        /******************************************************************
         *
         *      public method
         *
         ******************************************************************/
        public FrameAnimationAsset GetAnimation(string name) {
            foreach(var anim in animations) {
                if(anim.name == name)
                    return anim;
            }
            return null;
        }
        public void SetAnimation(string name) {
            var anim = GetAnimation(name);
            SetAnimation(anim);
        }
        public void SetAnimation(FrameAnimationAsset asset) {
            if(asset == null)
                return;
            if(curAnim != null && curAnim.Asset == asset)
                return;
            if(curAnim != null)
                curAnim.Stop();
            curAnim = new FrameAnimation(transform, asset);
            if(isPlaying)
                curAnim.Play(StopInner);
        }
        public void Play() {
            if(isPlaying)
                return;
            isPlaying = true;
            curAnim.Play(StopInner);
        }
        public void Play(string name) {
            SetAnimation(name);
            Play();
        }
        public void Play(FrameAnimationAsset asset) {
            SetAnimation(asset);
            Play();
        }
        public void Pause() {
            isPlaying = false;
        }
        public void Stop() {
            isPlaying = false;
            if(curAnim == null)
                return;
            curAnim.Stop();
        }
        /******************************************************************
         *
         *      private method
         *
         ******************************************************************/
        private void StopInner() {
            curAnim = null;
        }
    }
}
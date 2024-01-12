using System.Collections.Generic;
using UnityEngine;

namespace MamiyaTool {
    [DisallowMultipleComponent]
    public class FrameAnimator : FrameAnimationCtrlBase {
        public List<FrameAnimationAsset> animations;
        public string defaultAnim;
        /******************************************************************
         *
         *      override
         *
         ******************************************************************/
        public override void Play() {
            PlayInner();
        }
        public override void Stop() {
            isPlaying = false;
            curAnim.Reset();
            StopInner();
        }
        protected override bool Initialize() {
            if(animations != null) {
                SetAnimation(defaultAnim);
            }
            return true;
        }
        protected override void AwakeInner() {
            if(CanPlay && curAnim.PlayOnAwake)
                Play();
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
        public void Play(string name) {
            SetAnimation(name);
            Play();
        }
    }
}
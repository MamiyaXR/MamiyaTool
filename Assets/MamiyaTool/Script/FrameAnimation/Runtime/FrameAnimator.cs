using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

namespace MamiyaTool {
    [DisallowMultipleComponent]
    public class FrameAnimator : MonoBehaviour {
        public List<FrameAnimationAsset> animations;
        public string defaultAnim;
        public float PlaySpeed { get; set; }

        public FrameAnimationAsset CurAnim => curAnim;
        private FrameAnimationAsset curAnim;

        private float curFrame;
        private float curTimer;
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
            curFrame = 0f;
            curTimer = 0f;
            PlaySpeed = 1f;
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

            curTimer += Time.deltaTime * PlaySpeed;
            curFrame += PlaySpeed;
            if(curAnim.UseTime) {
                SetFrame(curTimer);
            } else
                SetFrame(Mathf.FloorToInt(curFrame));
        }
        /******************************************************************
         *
         *      public method
         *
         ******************************************************************/
        public void SetFrame(int frame) {
            if(curAnim.Tracks == null)
                return;
            for(int i = 0; i < curAnim.Tracks.Count; ++i)
                curAnim.Tracks[i].SetFrame(frame, curAnim.Loop);
        }
        public void SetFrame(float time) {
            if(curAnim.Tracks == null)
                return;
            for(int i = 0; i < curAnim.Tracks.Count; ++i)
                curAnim.Tracks[i].SetFrame(time, curAnim.Loop);
        }
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
            curAnim = asset;
            curFrame = curAnim.Offset;
            SetRoot(transform);
            SetFrame(curFrame);
        }
        public void Play() {
            if(isPlaying)
                return;
            isPlaying = true;
        }
        public void Play(string name) {
            var anim = GetAnimation(name);
            if(curAnim != anim)
                SetAnimation(name);
            Play();
        }
        public void Play(FrameAnimationAsset asset) {
            if(curAnim != asset)
                SetAnimation(asset);
            Play();
        }
        public void Pause() {
            isPlaying = false;
        }
        public void Stop(bool reset = false) {
            isPlaying = false;
            if(curAnim == null)
                return;
            curFrame = curAnim.Offset;
            if(reset)
                SetFrame(curFrame);
        }
        /******************************************************************
         *
         *      private method
         *
         ******************************************************************/
        private void SetRoot(Transform root) {
            if(curAnim.Tracks == null)
                return;
            for(int i = 0; i < curAnim.Tracks.Count; ++i)
                curAnim.Tracks[i].SetRoot(root);
        }
    }
}
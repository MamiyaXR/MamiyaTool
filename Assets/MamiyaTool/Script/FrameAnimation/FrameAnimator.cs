using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MamiyaTool {
    [DisallowMultipleComponent]
    public class FrameAnimator : MonoBehaviour {
        public List<FrameAnimationAsset> animations;
        public string defaultAnim;

        public FrameAnimationAsset CurAnim => curAnim;
        private FrameAnimationAsset curAnim;

        private int curFrame;
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
            curFrame = 0;
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
            SetFrame(++curFrame);
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
                SetTrackFrame(curAnim.Tracks[i], frame, curAnim.Loop);
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
        private void SetTrackFrame(FrameTrack track, int frame, bool loop = false) {
            if(track.Frames == null)
                return;
            Transform renderTrans = string.IsNullOrEmpty(track.RenderPath) ?
                                    transform : transform.Find(track.RenderPath);
            if(renderTrans == null)
                return;
            SpriteRenderer render = renderTrans.GetComponent<SpriteRenderer>();
            if(render == null)
                return;

            FrameData frameData = new FrameData();
            if(track.TryGetFrameData(frame, ref frameData, curAnim.Loop)) {
                renderTrans.localPosition = frameData.LocalPosition;
                renderTrans.localRotation = Quaternion.Euler(frameData.LocalRotation);
                renderTrans.localScale = frameData.LocalScale;
                render.sprite = frameData.Sprite;
            }
        }
    }
}
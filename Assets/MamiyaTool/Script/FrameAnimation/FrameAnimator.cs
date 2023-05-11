using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

namespace MamiyaTool {
    [DisallowMultipleComponent]
    [RequireComponent(typeof(SpriteRenderer))]
    public class FrameAnimator : MonoBehaviour {
        public List<FrameAnimation> animations;
        public string defaultAnim;
        private SpriteRenderer render;
        private int curSprite;

        public FrameAnimation CurAnim => curAnim;
        private FrameAnimation curAnim;

        private int frameCounter;
        private bool isPlaying;
        /******************************************************************
         *
         *      lifecycle
         *
         ******************************************************************/
        private void Awake() {
            curSprite = 0;
            frameCounter = 0;
            isPlaying = false;

            if(animations == null)
                return;
            SetAnimation(defaultAnim);
            if(curAnim != null && curAnim.PlayOnAwake)
                Play();
        }
        private void Update() {
            if(!isPlaying)
                return;
            if(curAnim == null)
                return;

            NextSprite(curAnim);
        }
        private void OnValidate() {
            render = GetComponent<SpriteRenderer>();
        }
        /******************************************************************
         *
         *      public method
         *
         ******************************************************************/
        public FrameAnimation GetAnimation(string name) {
            foreach(var anim in animations) {
                if(anim.name == name)
                    return anim;
            }
            return null;
        }
        public void SetAnimation(string name) {
            var anim = GetAnimation(name);
            if(anim == null)
                return;
            curAnim = anim;
            curSprite = curAnim.Offset;
            UpdateRender(curAnim);
        }
        public void Play() {
            if(curAnim == null)
                return;
            isPlaying = true;
        }
        public void Play(string name) {
            var anim = GetAnimation(name);
            if(curAnim != anim)
                SetAnimation(name);
            Play();
        }
        public void Pause() {
            isPlaying = false;
        }
        public void Stop(bool reset = false) {
            isPlaying = false;
            if(curAnim == null)
                return;
            frameCounter = 0;
            curSprite = curAnim.Offset;
            if(reset)
                UpdateRender(curAnim);
        }
        /******************************************************************
         *
         *      private method
         *
         ******************************************************************/
        private void NextSprite(FrameAnimation anim) {
            frameCounter++;
            if(frameCounter >= anim.Frame) {
                frameCounter = 0;
                int spriteCount = anim.Sprites.Count;
                if(spriteCount <= 0)
                    return;
                if(!anim.Loop && curSprite == spriteCount - 1)
                    isPlaying = false;
                else {
                    curSprite = (curSprite + 1) % spriteCount;
                    UpdateRender(anim);
                }
            }
        }
        private void UpdateRender(FrameAnimation anim) {
            render.sprite = anim.GetSprite(curSprite);
        }
    }
}
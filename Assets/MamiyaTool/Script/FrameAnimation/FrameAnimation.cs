using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MamiyaTool {
    #region frameData
    [Serializable]
    public struct FrameData {
        [SerializeField] private Sprite sprite;
        [SerializeField] private int frameCount;
        [SerializeField] private Vector3 localPositionOffset;
        [SerializeField] private Vector3 localRotationOffset;

        public Sprite Sprite => sprite;
        public int FrameCount => frameCount;
        public Vector3 LocalPositionOffset => localPositionOffset;
        public Vector3 LocalRotationOffset => localRotationOffset;
    }
    [Serializable]
    public struct FrameTrack {
        [SerializeField] private string renderPath;
        [SerializeField] private List<FrameData> frames;

        public string RenderPath => renderPath;
        public List<FrameData> Frames => frames;
        public int FrameCount {
            get {
                if(Frames == null)
                    return 0;
                int sum = 0;
                for(int i = 0; i < frames.Count; ++i)
                    sum += frames[i].FrameCount;
                return sum;
            }
        }
        public bool TryGetFrameData(int frame, ref FrameData result, bool loop) {
            if(frames == null)
                return false;
            int frameCount = FrameCount;
            frame = loop ? FrameCount % frame : frame;
            return false;
        }
    }
    #endregion

    #region frameAnimation
    [CreateAssetMenu(fileName = "Frame Animation")]
    public class FrameAnimationAsset : ScriptableObject {
        [SerializeField] private List<FrameTrack> tracks;
        [SerializeField] private int offset = 0;
        [SerializeField] private bool loop = false;
        [SerializeField] private bool playOnAwake = true;

        public List<FrameTrack> Tracks => tracks;
        public int Offset => offset;
        public bool Loop => loop;
        public bool PlayOnAwake => playOnAwake;
    }
    #endregion

    #region frameAnimator
    [DisallowMultipleComponent]
    public class FrameAnimator : MonoBehaviour {
        public List<FrameAnimationAsset> animations;
        public string defaultAnim;

        public FrameAnimationAsset CurAnim => curAnim;
        private FrameAnimationAsset curAnim;

        private int frameCounter;
        private bool isPlaying;

        private List<int> CurFrames {
            get {
                if(curFrames == null)
                    curFrames = new List<int>();
                if(curAnim == null) {
                    curFrames.Clear();
                } else if(curAnim.Tracks.Count != curFrames.Count) {
                    curFrames.Clear();
                    for(int i = 0; i < curAnim.Tracks.Count; i++)
                        curFrames.Add(0);
                }
                return curFrames;
            }
        }
        private List<int> curFrames;
        /******************************************************************
         *
         *      lifecycle
         *
         ******************************************************************/
        private void Awake() {
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
        /******************************************************************
         *
         *      public method
         *
         ******************************************************************/
        public void SetFrame(int frame) {
            //for(int i = 0; i < curAnim.Tracks.Count; ++i)
                
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
            if(anim == null)
                return;
            curAnim = anim;
            //curSprite = curAnim.Offset;
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
            //curSprite = curAnim.Offset;
            if(reset)
                UpdateRender(curAnim);
        }
        /******************************************************************
         *
         *      private method
         *
         ******************************************************************/
        private void SetTrackFrame(FrameTrack track, int frame, bool loop = false) {
            if(track.Frames == null)
                return;
            Transform renderTrans = transform.Find(track.RenderPath);
            if(renderTrans == null)
                return;
            SpriteRenderer render = renderTrans.GetComponent<SpriteRenderer>();
            if(render == null)
                return;


            if(loop) {
                //frame = 
            } else {
                frame = Mathf.Clamp(frame, 0, track.FrameCount - 1);
            }
        }
        private void NextSprite(FrameAnimationAsset anim) {
            //frameCounter++;
            //if(frameCounter >= anim.Frame) {
            //    frameCounter = 0;
            //    int spriteCount = anim.Sprites.Count;
            //    if(spriteCount <= 0)
            //        return;
            //    if(!anim.Loop && curSprite == spriteCount - 1)
            //        isPlaying = false;
            //    else {
            //        curSprite = (curSprite + 1) % spriteCount;
            //        UpdateRender(anim);
            //    }
            //}
        }
        private void UpdateRender(FrameAnimationAsset anim) {
            //render.sprite = anim.GetSprite(curSprite);
        }
    }
    #endregion
}
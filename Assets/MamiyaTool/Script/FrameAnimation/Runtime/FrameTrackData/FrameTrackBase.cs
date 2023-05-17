using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MamiyaTool {
    [Serializable]
    public abstract class FrameTrackBase {
        public abstract void SetRoot(Transform root);
        public abstract void SetFrame(int frame, bool loop);
        public abstract void SetFrame(float time, bool loop);
        public abstract FrameTrackBase Clone();
    }

    [Serializable]
    public abstract class FrameTrackBase<TComponent, T> : FrameTrackBase where TComponent : UnityEngine.Object where T : FrameDataBase {
        [SerializeField] protected string componentPath;
        [SerializeField] protected bool enable;
        [SerializeField] protected List<T> frames;

        protected Transform root;
        protected virtual TComponent Component {
            get {
                if(component == null) {
                    if(root != null) {
                        if(string.IsNullOrEmpty(componentPath))
                            component = root.GetComponent<TComponent>();
                        else {
                            Transform trans = root.Find(componentPath);
                            if(trans != null)
                                component = trans.GetComponent<TComponent>();
                        }
                    }
                }
                return component;
            }
        }
        protected TComponent component;

        public int FrameCount {
            get {
                if(frameCount == -1) {
                    frameCount = 0;
                    if(frames != null) {
                        for(int i = 0; i < frames.Count; ++i)
                            frameCount += frames[i].FrameCount;
                    }
                }
                return frameCount;
            }
        }
        protected int frameCount;

        public float Duration {
            get {
                if(duration == -1f) {
                    duration = 0f;
                    if(frames != null) {
                        for(int i = 0; i < frames.Count; ++i)
                            duration += frames[i].Duration;
                    }
                }
                return duration;
            }
        }
        protected float duration;

        protected int curFrameIndex;
        protected bool Enable => enable && Component != null && frames != null;
        /******************************************************************
         *
         *      public method
         *
         ******************************************************************/
        public FrameTrackBase() {
            enable = true;
            frameCount = -1;
            duration = -1f;
            curFrameIndex = -1;
        }
        public override void SetRoot(Transform root) {
            this.root = root;
            component = null;
        }
        public override void SetFrame(int frame, bool loop) {
            if(!Enable)
                return;
            if(FrameCount <= 0)
                return;
            frame = frame < 0 ? 0 : frame;
            frame = loop ? frame % frameCount : Mathf.Clamp(frame, 0, frameCount - 1);
            int sum = 0;
            for(int i = 0; i < frames.Count; ++i) {
                sum += frames[i].FrameCount;
                if(frame < sum) {
                    if(curFrameIndex != i) {
                        curFrameIndex = i;
                        Invoke();
                    }
                    return;
                }
            }
        }
        public override void SetFrame(float time, bool loop) {
            if(!Enable)
                return;
            if(Duration <= 0f)
                return;
            time = time < 0f ? 0f : time;
            time = loop ? time % duration : Mathf.Clamp(time, 0f, duration);
            float sum = 0f;
            for(int i = 0; i < frames.Count; ++i) {
                sum += frames[i].Duration;
                if(time < sum) {
                    if(curFrameIndex != i) {
                        curFrameIndex = i;
                        Invoke();
                    }
                    return;
                }
            }
        }
        /******************************************************************
         *
         *      
         *
         ******************************************************************/
        protected abstract void Invoke();
        protected void CopyTo(FrameTrackBase<TComponent, T> target) {
            target.componentPath = componentPath;
            target.enable = enable;
            if(frames != null) {
                target.frames = new List<T>();
                foreach(var frame in frames)
                    target.frames.Add(frame.Clone() as T);
            }
        }
    }
}
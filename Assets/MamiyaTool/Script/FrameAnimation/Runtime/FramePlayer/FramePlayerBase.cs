using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MamiyaTool {
    public abstract class FramePlayerBase<TObject, TData> : IFramePlayer where TObject : Object where TData : FrameDataBase {
        protected bool enable;
        protected int beginFrame;
        protected int endFrame;
        protected TObject m_Object;
        protected List<TData> datas;

        public bool Enable => enable;
        public int BeginFrame => beginFrame;
        public int EndFrame => endFrame;

        private int curFrame;
        /******************************************************************
         *
         *      public method
         *
         ******************************************************************/
        public void SetFrame(int frameCount) {
            frameCount = Mathf.Clamp(frameCount, beginFrame, endFrame);
            if(curFrame == frameCount)
                return;
            curFrame = frameCount;
            foreach(var data in datas) {
                if(data.FrameIndex == curFrame) {
                    Invoke(data);
                    return;
                }
            }
        }
        public void Init(Transform root, IFrameTrack<TData> track) {
            enable = track.Enable;
            m_Object = GetObjectInner(root, track.ComponentPath);
            datas = new List<TData>();
            datas.AddRange(track.Datas);

            beginFrame = datas.Count > 0 ? datas.Select(i => i.FrameIndex).Min() : 0;
            endFrame = datas.Count > 0 ? datas.Select(i => i.FrameIndex).Max() : 0;

            curFrame = -1;
        }
        public abstract void Reset();
        /******************************************************************
         *
         *      protected method
         *
         ******************************************************************/
        protected virtual TObject GetObjectInner(Transform root, string path) {
            if(root == null)
                return null;
            if(string.IsNullOrEmpty(path))
                return root.GetComponent<TObject>();
            Transform trans = root.Find(path);
            if(trans == null)
                return null;
            return trans.GetComponent<TObject>();
        }
        protected abstract void Invoke(TData data);
    }
}
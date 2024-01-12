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

        protected int curFrame;
        protected bool ObjUsable => m_Object != null;
        /******************************************************************
         *
         *      public method
         *
         ******************************************************************/
        public virtual void SetFrame(int frameCount) {
            frameCount = Mathf.Clamp(frameCount, beginFrame, endFrame);
            if(curFrame == frameCount)
                return;
            curFrame = frameCount;
            foreach(var data in datas) {
                if(data.FrameIndex == curFrame) {
                    if(ObjUsable)
                        Invoke(data);
                    return;
                }
            }
        }
        public void Init(Transform root, IFrameTrack<TData> track) {
            enable = track.Enable;
            GetObject(root, track.ComponentPath);
            datas = new List<TData>();
            datas.AddRange(track.Datas);

            beginFrame = datas.Count > 0 ? datas.Select(i => i.FrameIndex).Min() : 0;
            endFrame = datas.Count > 0 ? datas.Select(i => i.FrameIndex).Max() : 0;

            curFrame = -1;
        }
        public virtual void Reset() {
            if(ObjUsable)
                ResetInner();
        }
        /******************************************************************
         *
         *      protected method
         *
         ******************************************************************/
        protected virtual void GetObject(Transform root, string path) {
            m_Object = GetObjectInner(root, path);
            if(ObjUsable)
                Cache();
        }
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
        protected virtual void Cache() { }
        protected virtual void ResetInner() { }
        protected abstract void Invoke(TData data);
    }
}
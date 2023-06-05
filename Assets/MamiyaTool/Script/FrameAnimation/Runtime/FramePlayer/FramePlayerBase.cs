using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MamiyaTool {
    public abstract class FramePlayerBase<TObject, TData> : IFramePlayer where TObject : Object where TData : FrameDataBase {
        protected TObject m_Object;
        protected List<TData> datas;
        /******************************************************************
         *
         *      public method
         *
         ******************************************************************/
        public void SetFrame(int frameCount) {
            foreach(var data in datas) {
                if(data.FrameIndex == frameCount) {
                    Invoke(data);
                    return;
                }
            }
        }
        /******************************************************************
         *
         *      protected method
         *
         ******************************************************************/
        protected virtual TObject GetObject(Transform root, string path) {
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
        /******************************************************************
         *
         *      static method
         *
         ******************************************************************/
        public static T Create<T>(Transform root, string path, List<TData> datas) where T : FramePlayerBase<TObject, TData>, new() {
            T result = new T();
            result.m_Object = result.GetObject(root, path);
            result.datas = new List<TData>();
            result.datas.AddRange(datas);
            return result;
        }
    }
}
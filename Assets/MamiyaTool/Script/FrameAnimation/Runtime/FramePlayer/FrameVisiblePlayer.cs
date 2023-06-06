using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MamiyaTool {
    public class FrameVisiblePlayer : FramePlayerBase<GameObject, FrameVisibleData> {
        private bool cache;
        /******************************************************************
         *
         *      override
         *
         ******************************************************************/
        protected override GameObject GetObjectInner(Transform root, string path) {
            if(root == null)
                return null;
            if(string.IsNullOrEmpty(path)) {
                cache = root.gameObject.activeInHierarchy;
                return root.gameObject;
            }
            Transform trans = root.Find(path);
            if(trans == null)
                return null;
            cache = trans.gameObject.activeInHierarchy;
            return trans.gameObject;
        }
        protected override void Invoke(FrameVisibleData data) {
            m_Object.SetActive(data.Visible);
        }
        public override void Reset() {
            m_Object.SetActive(cache);
        }
    }
}
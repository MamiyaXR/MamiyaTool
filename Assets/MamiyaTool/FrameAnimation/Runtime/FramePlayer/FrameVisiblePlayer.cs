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
            if(string.IsNullOrEmpty(path))
                return root.gameObject;
            Transform trans = root.Find(path);
            if(trans == null)
                return null;
            return trans.gameObject;
        }
        protected override void Cache() {
            cache = m_Object.activeInHierarchy;
        }
        protected override void Invoke(FrameVisibleData data) {
            m_Object.SetActive(data.Visible);
        }
        protected override void ResetInner() {
            m_Object.SetActive(cache);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MamiyaTool {
    public class FrameScalePlayer : FramePlayerBase<Transform, FrameScaleData> {
        private Vector3 cache;
        /******************************************************************
         *
         *      override
         *
         ******************************************************************/
        protected override Transform GetObjectInner(Transform root, string path) {
            var result = base.GetObjectInner(root, path);
            cache = result.localScale;
            return result;
        }
        protected override void Invoke(FrameScaleData data) {
            m_Object.localScale = data.LocalScale;
        }
        public override void Reset() {
            m_Object.localScale = cache;
        }
    }
}
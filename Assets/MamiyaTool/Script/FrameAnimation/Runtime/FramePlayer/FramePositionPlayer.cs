using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MamiyaTool {
    public class FramePositionPlayer : FramePlayerBase<Transform, FramePositionData> {
        private Vector3 cache;
        /******************************************************************
         *
         *      override
         *
         ******************************************************************/
        protected override Transform GetObjectInner(Transform root, string path) {
            var result = base.GetObjectInner(root, path);
            cache = result.localPosition;
            return result;
        }
        protected override void Invoke(FramePositionData data) {
            m_Object.localPosition = data.LocalPosition;
        }
        public override void Reset() {
            m_Object.localPosition = cache;
        }
    }
}
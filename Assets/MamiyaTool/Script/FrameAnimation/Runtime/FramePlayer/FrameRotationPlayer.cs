using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MamiyaTool {
    public class FrameRotationPlayer : FramePlayerBase<Transform, FrameRotationData> {
        private Quaternion cache;
        /******************************************************************
         *
         *      override
         *
         ******************************************************************/
        protected override Transform GetObjectInner(Transform root, string path) {
            var result = base.GetObjectInner(root, path);
            cache = result.localRotation;
            return result;
        }
        protected override void Invoke(FrameRotationData data) {
            m_Object.localRotation = data.LocalRotation;
        }
        public override void Reset() {
            m_Object.localRotation = cache;
        }
    }
}
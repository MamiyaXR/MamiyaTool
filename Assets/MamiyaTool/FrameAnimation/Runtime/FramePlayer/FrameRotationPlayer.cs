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
        protected override void Cache() {
            cache = m_Object.localRotation;
        }
        protected override void Invoke(FrameRotationData data) {
            m_Object.localRotation = data.LocalRotation;
        }
        protected override void ResetInner() {
            m_Object.localRotation = cache;
        }
    }
}
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
        protected override void Cache() {
            cache = m_Object.localPosition;
        }
        protected override void Invoke(FramePositionData data) {
            m_Object.localPosition = data.LocalPosition;
        }
        protected override void ResetInner() {
            m_Object.localPosition = cache;
        }
    }
}
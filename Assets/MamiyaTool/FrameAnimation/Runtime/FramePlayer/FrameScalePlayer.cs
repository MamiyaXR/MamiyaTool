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
        protected override void Cache() {
            cache = m_Object.localScale;
        }
        protected override void Invoke(FrameScaleData data) {
            m_Object.localScale = data.LocalScale;
        }
        protected override void ResetInner() {
            m_Object.localScale = cache;
        }
    }
}
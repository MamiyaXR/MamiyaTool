using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MamiyaTool {
    public class FrameSpriteFlipXPlayer : FramePlayerBase<SpriteRenderer, FrameSpriteFlipXData> {
        private bool cache;
        /******************************************************************
         *
         *      override
         *
         ******************************************************************/
        protected override void Cache() {
            cache = m_Object.flipX;
        }
        protected override void Invoke(FrameSpriteFlipXData data) {
            m_Object.flipX = data.FilpX;
        }
        protected override void ResetInner() {
            m_Object.flipX = cache;
        }
    }
}
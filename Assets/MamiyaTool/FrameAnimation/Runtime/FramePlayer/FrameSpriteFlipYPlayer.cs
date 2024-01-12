using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MamiyaTool {
    public class FrameSpriteFlipYPlayer : FramePlayerBase<SpriteRenderer, FrameSpriteFlipYData> {
        private bool cache;
        /******************************************************************
         *
         *      override
         *
         ******************************************************************/
        protected override void Cache() {
            cache = m_Object.flipY;
        }
        protected override void Invoke(FrameSpriteFlipYData data) {
            m_Object.flipY = data.FilpY;
        }
        protected override void ResetInner() {
            m_Object.flipY = cache;
        }
    }
}
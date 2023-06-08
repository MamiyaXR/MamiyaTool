using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MamiyaTool {
    public class FrameSpritePlayer : FramePlayerBase<SpriteRenderer, FrameSpriteData> {
        private Sprite cache;
        /******************************************************************
         *
         *      override
         *
         ******************************************************************/
        protected override void Cache() {
            cache = m_Object.sprite;
        }
        protected override void Invoke(FrameSpriteData data) {
            m_Object.sprite = data.Frame;
        }
        protected override void ResetInner() {
            m_Object.sprite = cache;
        }
    }
}
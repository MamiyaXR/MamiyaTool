using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MamiyaTool {
    public class FrameSpriteOrderPlayer : FramePlayerBase<SpriteRenderer, FrameSpriteOrderData>{
        private int cache;
        /******************************************************************
         *
         *      override
         *
         ******************************************************************/
        protected override void Cache() {
            cache = m_Object.sortingOrder;
        }
        protected override void Invoke(FrameSpriteOrderData data) {
            m_Object.sortingOrder = cache + data.OrderInLayer;
        }
        protected override void ResetInner() {
            m_Object.sortingOrder = cache;
        }
    }
}
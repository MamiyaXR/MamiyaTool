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
        protected override SpriteRenderer GetObjectInner(Transform root, string path) {
            var result = base.GetObjectInner(root, path);
            cache = result.sortingOrder;
            return result;
        }
        protected override void Invoke(FrameSpriteOrderData data) {
            m_Object.sortingOrder = cache + data.OrderInLayer;
        }
        public override void Reset() {
            m_Object.sortingOrder = cache;
        }
    }
}
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
        protected override SpriteRenderer GetObjectInner(Transform root, string path) {
            var result = base.GetObjectInner(root, path);
            cache = result.flipX;
            return result;
        }
        protected override void Invoke(FrameSpriteFlipXData data) {
            m_Object.flipX = data.FilpX;
        }
        public override void Reset() {
            m_Object.flipX = cache;
        }
    }
}
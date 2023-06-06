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
        protected override SpriteRenderer GetObjectInner(Transform root, string path) {
            var result = base.GetObjectInner(root, path);
            cache = result.flipY;
            return result;
        }
        protected override void Invoke(FrameSpriteFlipYData data) {
            m_Object.flipY = data.FilpY;
        }
        public override void Reset() {
            m_Object.flipY = cache;
        }
    }
}
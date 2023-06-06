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
        protected override SpriteRenderer GetObjectInner(Transform root, string path) {
            var result = base.GetObjectInner(root, path);
            cache = result.sprite;
            return result;
        }
        protected override void Invoke(FrameSpriteData data) {
            m_Object.sprite = data.Frame;
        }
        public override void Reset() {
            m_Object.sprite = cache;
        }
    }
}
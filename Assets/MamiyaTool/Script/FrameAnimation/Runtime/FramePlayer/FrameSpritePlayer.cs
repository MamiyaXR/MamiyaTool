using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MamiyaTool {
    public class FrameSpritePlayer : FramePlayerBase<SpriteRenderer, FrameSpriteData> {
        protected override void Invoke(FrameSpriteData data) {
            m_Object.sprite = data.Frame;
        }
    }
}
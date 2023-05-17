using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MamiyaTool {
    [Serializable]
    public class FrameSpriteTrack : FrameTrackBase<SpriteRenderer, FrameSpriteData> {
        protected override void Invoke() {
            Component.sprite = frames[curFrameIndex].Frame;
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MamiyaTool {
    [Serializable]
    public class FrameSpriteOrderTrack : FrameTrackBase<SpriteRenderer, FrameSpriteOrderData> {
        protected override void Invoke() {
            Component.sortingOrder = frames[curFrameIndex].OrderInLayer;
        }
        public override FrameTrackBase Clone() {
            FrameSpriteOrderTrack result = new FrameSpriteOrderTrack();
            CopyTo(result);
            return result;
        }
    }
}
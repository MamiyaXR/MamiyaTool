using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MamiyaTool {
    [Serializable]
    public class FrameScaleTrack : FrameTrackBase<Transform, FrameScaleData> {
        protected override void Invoke() {
            Component.localScale = frames[curFrameIndex].LocalScale;
        }
        public override FrameTrackBase Clone() {
            FrameScaleTrack result = new FrameScaleTrack();
            CopyTo(result);
            return result;
        }
    }
}
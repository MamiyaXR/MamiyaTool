using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MamiyaTool {
    [Serializable]
    public class FramePositionTrack : FrameTrackBase<Transform, FramePositionData> {
        protected override void Invoke() {
            Component.localPosition = frames[curFrameIndex].LocalPosition;
        }
    }
}
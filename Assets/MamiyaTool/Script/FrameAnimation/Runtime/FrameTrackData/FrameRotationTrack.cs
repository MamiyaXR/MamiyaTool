using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MamiyaTool {
    [Serializable]
    public class FrameRotationTrack : FrameTrackBase<Transform, FrameRotationData> {
        protected override void Invoke() {
            Component.localRotation = frames[curFrameIndex].LocalRotation;
        }
    }
}
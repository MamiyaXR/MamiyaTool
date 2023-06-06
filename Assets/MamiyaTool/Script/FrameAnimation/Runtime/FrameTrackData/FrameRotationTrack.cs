using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MamiyaTool {
    public class FrameRotationTrack : FrameTrackBase<FrameRotationData> {
        public override IFramePlayer CreatePlayer(Transform root) {
            FrameRotationPlayer result = new FrameRotationPlayer();
            result.Init(root, this);
            return result;
        }
    }
}
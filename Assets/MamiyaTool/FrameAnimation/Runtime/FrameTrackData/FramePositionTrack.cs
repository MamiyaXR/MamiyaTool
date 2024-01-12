using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MamiyaTool {
    public class FramePositionTrack : FrameTrackBase<FramePositionData> {
        public override IFramePlayer CreatePlayer(Transform root) {
            FramePositionPlayer result = new FramePositionPlayer();
            result.Init(root, this);
            return result;
        }
    }
}
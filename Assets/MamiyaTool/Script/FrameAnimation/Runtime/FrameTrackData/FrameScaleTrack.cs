using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MamiyaTool {
    public class FrameScaleTrack : FrameTrackBase<FrameScaleData> {
        public override IFramePlayer CreatePlayer(Transform root) {
            FrameScalePlayer result = new FrameScalePlayer();
            result.Init(root, this);
            return result;
        }
    }
}
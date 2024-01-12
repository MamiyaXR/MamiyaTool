using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MamiyaTool {
    public class FrameVisibleTrack : FrameTrackBase<FrameVisibleData> {
        public override IFramePlayer CreatePlayer(Transform root) {
            FrameVisiblePlayer result = new FrameVisiblePlayer();
            result.Init(root, this);
            return result;
        }
    }
}
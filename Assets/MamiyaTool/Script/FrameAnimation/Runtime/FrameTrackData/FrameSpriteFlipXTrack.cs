using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MamiyaTool {
    public class FrameSpriteFlipXTrack : FrameTrackBase<FrameSpriteFlipXData> {
        public override IFramePlayer CreatePlayer(Transform root) {
            FrameSpriteFlipXPlayer result = new FrameSpriteFlipXPlayer();
            result.Init(root, this);
            return result;
        }
    }
}
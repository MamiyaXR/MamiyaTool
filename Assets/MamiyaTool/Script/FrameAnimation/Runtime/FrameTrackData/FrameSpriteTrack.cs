using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MamiyaTool {
    //[Serializable]
    //public class FrameSpriteTrack : FrameTrackBase<SpriteRenderer, FrameSpriteData> {
    //    protected override void Invoke() {
    //        Component.sprite = frames[curFrameIndex].Frame;
    //    }
    //    public override FrameTrackBase Clone() {
    //        FrameSpriteTrack result = new FrameSpriteTrack();
    //        CopyTo(result);
    //        return result;
    //    }
    //}
    public class FrameSpriteTrack : FrameTrackBase<FrameSpriteData> {
        public override IFramePlayer CreatePlayer(Transform root) {
            return FramePlayerBase.Create<FrameSpritePlayer>(root, componentPath, datas);
        }
    }
}
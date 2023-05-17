using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MamiyaTool {
    [Serializable]
    public class FrameVisibleTrack : FrameTrackBase<GameObject, FrameVisibleData> {
        protected override GameObject Component {
            get {
                if(component == null) {
                    if(root != null) {
                        if(string.IsNullOrEmpty(componentPath))
                            component = root.gameObject;
                        else {
                            Transform trans = root.Find(componentPath);
                            if(trans != null)
                                component = trans.gameObject;
                        }
                    }
                }
                return component;
            }
        }
        protected override void Invoke() {
            Component.SetActive(frames[curFrameIndex].Visible);
        }
        public override FrameTrackBase Clone() {
            FrameVisibleTrack result = new FrameVisibleTrack();
            CopyTo(result);
            return result;
        }
    }
}
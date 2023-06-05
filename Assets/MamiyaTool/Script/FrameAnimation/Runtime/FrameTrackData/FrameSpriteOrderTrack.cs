using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MamiyaTool {
    //[Serializable]
    //public class FrameSpriteOrderTrack : FrameTrackBase<SpriteRenderer, FrameSpriteOrderData> {
    //    private int sortingOrderCatch;
    //    protected override SpriteRenderer Component {
    //        get {
    //            if(component == null) {
    //                if(root != null) {
    //                    if(string.IsNullOrEmpty(componentPath)) {
    //                        component = root.GetComponent<SpriteRenderer>();
    //                        sortingOrderCatch = component.sortingOrder;
    //                    } else {
    //                        Transform trans = root.Find(componentPath);
    //                        if(trans != null) {
    //                            component = trans.GetComponent<SpriteRenderer>();
    //                            sortingOrderCatch = component.sortingOrder;
    //                        }
    //                    }
    //                }
    //            }
    //            return component;
    //        }
    //    }
    //    protected override void Invoke() {
    //        Component.sortingOrder = sortingOrderCatch + frames[curFrameIndex].OrderInLayer;
    //    }
    //    public override FrameTrackBase Clone() {
    //        FrameSpriteOrderTrack result = new FrameSpriteOrderTrack();
    //        CopyTo(result);
    //        return result;
    //    }
    //    public override void Reset() {
    //        Component.sortingOrder = sortingOrderCatch;
    //    }
    //}
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MamiyaTool {
    [Serializable]
    public class FrameSpriteOrderData : FrameDataBase {
        [SerializeField] private int orderInLayer;

        public int OrderInLayer => orderInLayer;
        public override FrameDataBase Clone() {
            FrameSpriteOrderData result = new FrameSpriteOrderData();
            CopyTo(result);
            result.orderInLayer = orderInLayer;
            return result;
        }
    }
}
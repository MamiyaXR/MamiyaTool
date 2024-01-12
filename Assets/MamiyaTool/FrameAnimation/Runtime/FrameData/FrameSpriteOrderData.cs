using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MamiyaTool {
    [Serializable]
    public class FrameSpriteOrderData : FrameDataBase {
        [SerializeField] private int orderInLayer = 0;

        public int OrderInLayer => orderInLayer;
    }
}
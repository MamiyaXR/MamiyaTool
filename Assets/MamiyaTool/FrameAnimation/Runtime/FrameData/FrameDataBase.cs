using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MamiyaTool {
    [Serializable]
    public abstract class FrameDataBase {
        [SerializeField] protected int frameIndex;

        public int FrameIndex => frameIndex;
    }
}
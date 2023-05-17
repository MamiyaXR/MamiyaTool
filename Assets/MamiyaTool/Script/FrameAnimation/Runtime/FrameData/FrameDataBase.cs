using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MamiyaTool {
    [Serializable]
    public abstract class FrameDataBase {
        [SerializeField] protected int frameCount;
        [SerializeField] protected float duration;
        public int FrameCount => frameCount;
        public float Duration => duration;
    }
}
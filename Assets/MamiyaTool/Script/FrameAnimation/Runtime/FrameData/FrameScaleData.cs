using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MamiyaTool {
    [Serializable]
    public class FrameScaleData : FrameDataBase {
        [SerializeField] private Vector3 localScale;

        public Vector3 LocalScale => localScale;

        public FrameScaleData() {
            localScale = Vector3.one;
        }
    }
}
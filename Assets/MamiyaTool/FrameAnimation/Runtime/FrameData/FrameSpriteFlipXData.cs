using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MamiyaTool {
    [Serializable]
    public class FrameSpriteFlipXData : FrameDataBase {
        [SerializeField] private bool flipX = false;

        public bool FilpX => flipX;
    }
}
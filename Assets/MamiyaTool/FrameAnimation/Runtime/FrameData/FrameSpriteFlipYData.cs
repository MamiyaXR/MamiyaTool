using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MamiyaTool {
    [Serializable]
    public class FrameSpriteFlipYData : FrameDataBase {
        [SerializeField] private bool flipY = false;

        public bool FilpY => flipY;
    }
}
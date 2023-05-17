using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MamiyaTool {
    [Serializable]
    public class FrameSpriteData : FrameDataBase {
        [SerializeField] private Sprite frame;

        public Sprite Frame => frame;
    }
}
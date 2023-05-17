﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MamiyaTool {
    [Serializable]
    public class FramePositionData : FrameDataBase {
        [SerializeField] private Vector3 localPosition;

        public Vector3 LocalPosition => localPosition;

        public FramePositionData() {
            localPosition = Vector3.zero;
        }
    }
}
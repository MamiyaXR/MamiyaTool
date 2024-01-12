using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MamiyaTool {
    [Serializable]
    public class FrameRotationData : FrameDataBase {
        [SerializeField] private Quaternion localRotation = Quaternion.identity;

        public Quaternion LocalRotation => localRotation;
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MamiyaTool {
    [Serializable]
    public class FrameRotationData : FrameDataBase {
        [SerializeField] private Quaternion localRotation;

        public Quaternion LocalRotation => localRotation;

        public FrameRotationData() {
            localRotation = Quaternion.identity;
        }
    }
}
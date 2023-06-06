using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MamiyaTool {
    [Serializable]
    public abstract class FrameTrackBase {
        public abstract IFramePlayer CreatePlayer(Transform root);
    }

    [Serializable]
    public abstract class FrameTrackBase<T> : FrameTrackBase, IFrameTrack<T> where T : FrameDataBase {
        [SerializeField] protected bool enable = true;
        [SerializeField] protected string componentPath;
        [SerializeField] protected List<T> frames;

        public bool Enable => enable;
        public string ComponentPath => componentPath;
        public List<T> Datas => frames;
    }
}
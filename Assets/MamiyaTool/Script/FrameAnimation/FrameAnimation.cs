using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MamiyaTool {
    #region frameData
    [Serializable]
    public struct FrameData {
        [SerializeField] private Sprite sprite;
        [SerializeField] private int frameCount;
        [SerializeField] private Vector3 localPosition;
        [SerializeField] private Vector3 localRotation;
        [SerializeField] private Vector3 localScale;

        public Sprite Sprite => sprite;
        public int FrameCount => frameCount;
        public Vector3 LocalPosition => localPosition;
        public Vector3 LocalRotation => localRotation;
        public Vector3 LocalScale => localScale;
    }
    [Serializable]
    public struct FrameTrack {
        [SerializeField] private string renderPath;
        [SerializeField] private List<FrameData> frames;

        public string RenderPath => renderPath;
        public List<FrameData> Frames => frames;
        public int FrameCount {
            get {
                if(Frames == null)
                    return 0;
                int sum = 0;
                for(int i = 0; i < frames.Count; ++i)
                    sum += frames[i].FrameCount;
                return sum;
            }
        }
        public bool TryGetFrameData(int frame, ref FrameData result, bool loop) {
            if(frames == null)
                return false;
            int frameCount = FrameCount;
            if(frameCount <= 0)
                return false;
            frame = frame < 0 ? 0 : frame;
            frame = loop ? frame % frameCount : frame;
            frame = Mathf.Clamp(frame, 0, frameCount - 1);
            int sum = 0;
            for(int i = 0; i < frames.Count; ++i) {
                sum += frames[i].FrameCount;
                if(frame < sum) {
                    result = frames[i];
                    return true;
                }
            }
            return false;
        }
    }
    #endregion

    #region frameAnimationAsset
    [CreateAssetMenu(fileName = "Frame Animation")]
    public class FrameAnimationAsset : ScriptableObject {
        [SerializeField] private List<FrameTrack> tracks;
        [SerializeField] private int offset = 0;
        [SerializeField] private bool loop = false;
        [SerializeField] private bool playOnAwake = true;

        public List<FrameTrack> Tracks => tracks;
        public int Offset => offset;
        public bool Loop => loop;
        public bool PlayOnAwake => playOnAwake;
    }
    #endregion
}
using System.Collections.Generic;
using UnityEngine;

namespace MamiyaTool {
    public class FrameAnimationAsset : ScriptableObject {
        [SerializeReference] private List<FrameTrackBase> tracks;
        [SerializeField] private int sampleRate = 24;
        [SerializeField] private bool loop = false;
        [SerializeField] private bool playOnAwake = true;

        public List<FrameTrackBase> Tracks => tracks;
        public int SampleRate => sampleRate;
        public bool Loop => loop;
        public bool PlayOnAwake => playOnAwake;
    }
}
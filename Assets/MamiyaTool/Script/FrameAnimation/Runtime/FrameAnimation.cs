using System;
using System.Collections;
using System.Collections.Generic;

namespace MamiyaTool {
    public class FrameAnimation : IDisposable {
        //private List<FrameTrackBase> tracks;
        //private int offset = 0;
        //private bool loop = false;
        //private bool playOnAwake = true;
        //private bool useTime = false;

        //public List<FrameTrackBase> Tracks => tracks;
        //public int Offset => offset;
        //public bool Loop => loop;
        //public bool PlayOnAwake => playOnAwake;
        //public bool UseTime => useTime;
        //public FrameAnimationAsset Asset { get; private set; }

        //public FrameAnimation(FrameAnimationAsset asset) {
        //    if(asset.Tracks != null) {
        //        tracks = new List<FrameTrackBase>();
        //        foreach(var track in asset.Tracks)
        //            tracks.Add(track.Clone());
        //    }
        //    offset = asset.Offset;
        //    loop = asset.Loop;
        //    playOnAwake = asset.PlayOnAwake;
        //    useTime = asset.UseTime;
        //    Asset = asset;
        //}
        //public void Reset() {
        //    if(tracks != null) {
        //        foreach(var track in tracks)
        //            track.Reset();
        //    }
        //}
        public void Dispose() {
            GC.SuppressFinalize(this);
        }
    }
}
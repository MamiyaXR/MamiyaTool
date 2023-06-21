using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace MamiyaTool {
    public class FrameAnimation : IDisposable {
        private FrameAnimationAsset asset;

        private List<IFramePlayer> players;
        private int sampleRate = 24;
        private bool loop = false;
        private bool playOnAwake;

        private float frameLength;
        private int beginFrame;
        private int endFrame;
        private int frameCount;
        private float length;

        private bool usable;
        private float frameTimer;
        private int curFrame;

        private UnityEvent onComplete;

        public FrameAnimationAsset Asset => asset;
        public bool PlayOnAwake => playOnAwake;
        public float startTime => beginFrame;
        public float stopTime => endFrame;
        /******************************************************************
         *
         *      public method
         *
         ******************************************************************/
        public FrameAnimation(Transform root, FrameAnimationAsset asset, bool isEdit = false) {
            this.asset = asset;

            players = new List<IFramePlayer>();
            if(asset.Tracks != null) {
                foreach(var track in asset.Tracks)
                    players.Add(track.CreatePlayer(root));
            }
            sampleRate = asset.SampleRate;
            loop = asset.Loop;
            playOnAwake = asset.PlayOnAwake;

            frameLength = sampleRate <= 0 ? 0f : 1f / sampleRate;
            beginFrame = isEdit ? players.Select(i => i.BeginFrame).Min() : 0;
            endFrame = players.Select(i => i.EndFrame).Max();
            frameCount = endFrame - beginFrame;
            length = frameCount * frameLength;

            usable = length > 0f;

            onComplete = new UnityEvent();
        }
        public void Play(UnityAction complete = null) {
            frameTimer = 0f;
            curFrame = beginFrame;
            SetFrame(curFrame);
            onComplete.AddListener(complete);
        }
        public void Update(float time) {
            if(!usable)
                return;

            frameTimer += time;
            if(frameTimer >= frameLength) {
                int step = (int)(frameTimer / frameLength);
                frameTimer = frameTimer % frameLength;
                curFrame += step;
                if(curFrame > endFrame) {
                    if(loop) {
                        curFrame = beginFrame;
                    } else {
                        Stop();
                        return;
                    }
                }
                SetFrame(curFrame);
            }
        }
        public void Stop() {
            Reset();
            onComplete.Invoke();
            onComplete.RemoveAllListeners();
        }
        public void Reset() {
            foreach(var player in players)
                player.Reset();
        }
        public void Dispose() {
            GC.SuppressFinalize(this);
        }
        /******************************************************************
         *
         *      private method
         *
         ******************************************************************/
        private void SetFrame(int frameIndex) {
            foreach(var player in players) {
                if(player.Enable)
                    player.SetFrame(frameIndex);
            }
        }
    }
}
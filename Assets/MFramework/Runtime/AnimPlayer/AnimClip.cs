using System;
using System.Collections.Generic;
using UnityEngine;
#if DOTWEEN
using DG.Tweening;
#endif

namespace MFramework {
    #region AnimTime
    [Serializable]
    public class AnimTime {
        public float delay;
        public float duration = 1f;
#if DOTWEEN
        public Ease ease = Ease.OutQuad;
#endif
        public AnimationCurve easeCurve;

        public float TotalTime => delay + duration;
    }
    #endregion

    #region AnimLoop
    [Serializable]
    public class AnimLoop {
        public int loops = 1;
#if DOTWEEN
        public LoopType loopType;
#endif
    }
    #endregion

    #region AnimClip
    public enum AnimClipState {
        Idle = 0,
        Playing,
        Stopped,
    }
    public enum AnimClipResetType {
        Restart = 0,
        FromCurrent,
    }
    [Serializable]
    public class AnimClip {
        public bool enabled = true;
        public AnimClipResetType resetType = AnimClipResetType.FromCurrent;
        public AnimTime time;
        public AnimLoop loop;

        public AnimPlayer Player { get; private set; }
        public AnimClipState State { get; private set; }
        public bool Playing => State == AnimClipState.Playing;
        public bool Played { get; private set; }

        public virtual bool IsRelative => false;

        [HideInInspector] public bool isFrom;
        /******************************************************************
         * 
         *      public method
         * 
         ******************************************************************/
        public void Update() {
            if(Playing) {
#if DOTWEEN
                if(_tweens != null) {
                    foreach(var tween in _tweens) {
                        if(tween.active && tween.IsPlaying())
                            return;
                    }
                }
#endif
                Stop();
            }
        }
        public void Play() {
            if(Playing)
                return;

            State = AnimClipState.Playing;
            if(time.delay <= 0) {
                Played = true;
                DoPlay();
            } else {
                DoDelay();
            }
        }
        public void Stop() {
            if(!Playing)
                return;

            Played = false;
            State = AnimClipState.Stopped;
#if DOTWEEN
            if(_tweens != null) {
                foreach(var tween in _tweens)
                    tween.Kill();
                _tweens.Clear();
            }
#endif
            DoStop();
        }
        /******************************************************************
         * 
         *      private method
         * 
         ******************************************************************/
        protected virtual void DoInit() { }
        protected virtual void DoReset() { }
        protected virtual void DoPlay() { }
        protected virtual void DoStop() { }
        internal void Init(AnimPlayer player) {
            Player = player;
            DoInit();
        }
        private void OnCompleted() {
            Played = true;
            DoPlay();
        }
        private void DoDelay() {
#if DOTWEEN
            var tw = AddDelay();
            tw.SetEase(Ease.Linear);
            tw.onComplete = OnCompleted;
#endif
        }
        internal void Reset() {
            Stop();
            State = AnimClipState.Idle;
            switch(resetType) {
                case AnimClipResetType.Restart:
                    DoReset();
                    break;
                case AnimClipResetType.FromCurrent:
                    break;
            }
        }
#if DOTWEEN
        private List<Tween> _tweens;
        protected Tween AddDelay() {
            float temp = 0;
            var tween = DOTween.To(() => temp, (x) => temp = x, 1, time.delay);
            _tweens ??= new List<Tween>();
            _tweens.Add(tween);
            tween.SetUpdate(Player.realTime);

#if UNITY_EDITOR
            AnimPlayer.OnAddTween?.Invoke(tween);
#endif
            return tween;
        }
        protected Tween AddTween(Tween tween) {
            _tweens ??= new List<Tween>();
            _tweens.Add(tween);
            tween.SetUpdate(Player.realTime);
            tween.SetEase(time.ease);
            if(time.ease == Ease.INTERNAL_Custom)
                tween.SetEase(time.easeCurve);
            tween.SetLoops(loop.loops, loop.loopType);
            if(isFrom) {
                ((Tweener)tween).From(IsRelative);
            } else {
                tween.SetRelative(IsRelative);
            }
#if UNITY_EDITOR
            AnimPlayer.OnAddTween?.Invoke(tween);
#endif
            return tween;
        }
#endif
    }
    #endregion
}
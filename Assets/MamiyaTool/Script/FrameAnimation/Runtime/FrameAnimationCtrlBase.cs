using System;
using System.Collections.Generic;
using UnityEngine;

namespace MamiyaTool {
    public abstract class FrameAnimationCtrlBase : MonoBehaviour, IFrameAnimationCtrl {
        [SerializeField, Range(0f, 1f)] protected float playSpeed = 1f;
        public float PlaySpeed { get => playSpeed; set => playSpeed = value; }
        public FrameAnimation CurAnim => curAnim;
        protected FrameAnimation curAnim;

        public bool IsPlaying => isPlaying;
        protected bool isPlaying;
        protected bool CanPlay {
            get {
                bool result = curAnim != null;
                if(!result && isPlaying)
                    isPlaying = false;
                return result;
            }
        }

        protected Dictionary<string, object> m_Params = new Dictionary<string, object>();
        protected bool init;
        /******************************************************************
         *
         *      lifecycle
         *
         ******************************************************************/
        protected void Awake() {
            isPlaying = false;
            init = Initialize();
            AwakeInner();
        }
        protected void OnEnable() {
            OnEnableInner();
        }
        protected void Start() {
            StartInner();
        }
        protected void Update() {
            UpdateInner();
            UpdateAnim();
        }
        protected void OnDisable() {
            OnDisableInner();
        }
        protected void nDestroy() {
            OnDestroyInner();
        }
        /******************************************************************
         *
         *      public method
         *
         ******************************************************************/
        public abstract void Play();
        public abstract void Stop();
        protected abstract bool Initialize();
        public bool GetBool(string param) {
            return GetParam<bool>(param);
        }
        public float GetFloat(string param) {
            return GetParam<float>(param);
        }
        public int GetInt(string param) {
            return GetParam<int>(param);
        }
        public void SetBool(string param, bool value) {
            SetParam(param, value);
        }
        public void SetFloat(string param, float value) {
            SetParam(param, value);
        }
        public void SetInt(string param, int value) {
            SetParam(param, value);
        }
        public void SetTrigger(string param) {
            var trigger = GetParam<Action>(param);
            trigger?.Invoke();
        }
        public void SetAnimation(FrameAnimationAsset asset) {
            if(asset == null)
                return;
            if(curAnim != null && curAnim.Asset == asset)
                return;
            if(curAnim != null)
                curAnim.Reset();
            curAnim = new FrameAnimation(transform, asset);
            if(isPlaying)
                curAnim.Play();
        }
        public void Play(FrameAnimationAsset asset) {
            SetAnimation(asset);
            PlayInner();
        }
        public void Pause() {
            isPlaying = false;
        }
        public void Resume() {
            if(CanPlay)
                isPlaying = true;
        }
        /******************************************************************
         *
         *      protected method
         *
         ******************************************************************/
        protected virtual T GetParam<T>(string key) {
            if(m_Params.TryGetValue(key, out object value))
                return (T)value;
            return default;
        }
        protected virtual void SetParam<T>(string key, T value) {
            if(m_Params.ContainsKey(key))
                m_Params[key] = value;
        }
        protected virtual void AwakeInner() { }
        protected virtual void OnEnableInner() { }
        protected virtual void StartInner() { }
        protected virtual void UpdateInner() { }
        protected virtual void OnDisableInner() { }
        protected virtual void OnDestroyInner() { }
        protected virtual void UpdateAnim() {
            if(!CanPlay)
                return;
            if(!isPlaying)
                return;
            isPlaying = curAnim.Update(Time.deltaTime * PlaySpeed);
        }
        protected virtual void PlayInner() {
            if(isPlaying)
                return;
            if(!CanPlay)
                return;
            isPlaying = true;
            curAnim.Play();
        }
        protected virtual void StopInner() {
            curAnim = null;
        }
    }
}
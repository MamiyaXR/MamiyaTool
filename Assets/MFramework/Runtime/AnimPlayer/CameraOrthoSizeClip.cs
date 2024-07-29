using System;
using UnityEngine;
#if DOTWEEN
using DG.Tweening;
#endif

namespace MFramework {
    [Serializable, RefCompMenu("Camera/OrthoSize")]
    public class CameraOrthoSizeClip : AnimClip {
        public Camera target;
        [AnimClipToFrom]
        public float to = 5f;

        [SerializeField, HideInInspector]
        private float _from;
        /******************************************************************
         * 
         *      override
         * 
         ******************************************************************/
        protected override void DoInit() {
            base.DoInit();
            _from = target.orthographicSize;
        }
        protected override void DoReset() {
            base.DoReset();
            target.orthographicSize = _from;
        }
        protected override void DoPlay() {
#if DOTWEEN
            AddTween(target.DOOrthoSize(to, time.duration));
#endif
        }
    }
}
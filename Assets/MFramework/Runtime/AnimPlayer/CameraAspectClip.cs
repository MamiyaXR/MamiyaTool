using System;
using UnityEngine;
#if DOTWEEN
using DG.Tweening;
#endif

namespace MFramework {
    [Serializable, RefCompMenu("Camera/Aspect")]
    public class CameraAspectClip : AnimClip {
        public Camera target;
        [AnimClipToFrom]
        public float to = 1.777778f;

        [SerializeField, HideInInspector]
        private float _from;
        /******************************************************************
         * 
         *      override
         * 
         ******************************************************************/
        protected override void DoInit() {
            base.DoInit();
            _from = target.aspect;
        }
        protected override void DoReset() {
            base.DoReset();
            target.aspect = _from;
        }
        protected override void DoPlay() {
#if DOTWEEN
            AddTween(target.DOAspect(to, time.duration));
#endif
        }
    }
}
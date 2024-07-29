using System;
using UnityEngine;
#if DOTWEEN
using DG.Tweening;
#endif

namespace MFramework {
    [Serializable, RefCompMenu("Camera/FOV")]
    public class CameraFOVClip : AnimClip {
        public Camera target;
        [AnimClipToFrom]
        public float to = 60f;

        [SerializeField, HideInInspector]
        private float _from;
        /******************************************************************
         * 
         *      override
         * 
         ******************************************************************/
        protected override void DoInit() {
            base.DoInit();
            _from = target.fieldOfView;
        }
        protected override void DoReset() {
            base.DoReset();
            target.fieldOfView = _from;
        }
        protected override void DoPlay() {
#if DOTWEEN
            AddTween(target.DOFieldOfView(to, time.duration));
#endif
        }
    }
}
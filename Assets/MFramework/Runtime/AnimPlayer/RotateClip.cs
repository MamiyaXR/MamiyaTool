using System;
using UnityEngine;
#if DOTWEEN
using DG.Tweening;
#endif

namespace MFramework {
    [Serializable, RefCompMenu("Transfrom/Rotate")]
    public class RotateClip : AnimClip {
        public Transform target;
        [AnimClipToFrom]
        public Vector3 to;
        public bool local = true;
        public bool relative;
        public RotateMode rotate;

        public override bool IsRelative => relative;

        [SerializeField, HideInInspector]
        private Vector3 _from;
        /******************************************************************
         * 
         *      override
         * 
         ******************************************************************/
        protected override void DoInit() {
            base.DoInit();
            if(local) {
                _from = target.localEulerAngles;
            } else {
                _from = target.eulerAngles;
            }
        }
        protected override void DoReset() {
            base.DoReset();
            if(local) {
                target.localEulerAngles = _from;
            } else {
                target.eulerAngles = _from;
            }
        }
        protected override void DoPlay() {
#if DOTWEEN
            if(local) {
                AddTween(target.DOLocalRotate(to, time.duration, rotate).SetRelative(relative));
            } else {
                AddTween(target.DORotate(to, time.duration, rotate).SetRelative(relative));
            }
#endif
        }
    }
}
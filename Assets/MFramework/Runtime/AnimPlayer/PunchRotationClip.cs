using System;
using UnityEngine;
#if DOTWEEN
using DG.Tweening;
#endif

namespace MFramework {
    [Serializable, RefCompMenu("Punch/Rotation")]
    public class PunchRotationClip : AnimClip {
        public Transform target;
        [AnimClipToFrom]
        public Vector3 to;

        [Range(0, 50)]
        public int vibrato = 10;
        [Range(0f, 1f)]
        public float elasticity = 1f;

        [SerializeField, HideInInspector]
        private Vector3 _from;
        /******************************************************************
         * 
         *      override
         * 
         ******************************************************************/
        protected override void DoInit() {
            base.DoInit();
            _from = target.localEulerAngles;
        }
        protected override void DoReset() {
            base.DoReset();
            target.localEulerAngles = _from;
        }
        protected override void DoPlay() {
#if DOTWEEN
            AddTween(target.DOPunchRotation(to, time.duration, vibrato, elasticity));
#endif
        }
    }
}
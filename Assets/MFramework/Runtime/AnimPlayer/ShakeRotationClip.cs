using System;
using UnityEngine;
#if DOTWEEN
using DG.Tweening;
#endif

namespace MFramework {
    [Serializable, RefCompMenu("Shake/Rotation")]
    public class ShakeRotationClip : AnimClip {
        public Transform target;
        [AnimClipToFrom]
        public Vector3 strength = new Vector3(0f, 0f, 90f);
        [Range(0, 50)]
        public int vibrato = 10;
        [Range(0f, 90f)]
        public float randomness = 90f;
        public bool fadeOut = true;

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
            AddTween(target.DOShakeRotation(time.duration, strength, vibrato, randomness, fadeOut));
#endif
        }
    }
}
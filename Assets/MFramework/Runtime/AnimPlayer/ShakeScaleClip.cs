using System;
using UnityEngine;
#if DOTWEEN
using DG.Tweening;
#endif

namespace MFramework {
    [Serializable, RefCompMenu("Shake/Scale")]
    public class ShakeScaleClip : AnimClip {
        public Transform target;
        [AnimClipToFrom]
        public Vector3 strength = Vector3.one;
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
            _from = target.localScale;
        }
        protected override void DoReset() {
            base.DoReset();
            target.localScale = _from;
        }
        protected override void DoPlay() {
#if DOTWEEN
            AddTween(target.DOShakeScale(time.duration, strength, vibrato, randomness, fadeOut));
#endif
        }
    }
}
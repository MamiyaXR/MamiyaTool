using System;
using UnityEngine;
#if DOTWEEN
using DG.Tweening;
#endif

namespace MFramework {
    [Serializable, RefCompMenu("Shake/Position")]
    public class ShakePositionClip : AnimClip {
        [CompSelector(typeof(RectTransform), typeof(Transform))]
        public UnityEngine.Object target;
        [AnimClipToFrom]
        public Vector3 strength = Vector3.one;
        [Range(0, 50)]
        public int vibrato = 10;
        [Range(0f, 90f)]
        public float randomness = 90f;
        public bool snapping = false;
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
            switch(target) {
                case RectTransform rtf:
                    _from = rtf.anchoredPosition3D;
                    break;
                case Transform tf:
                    _from = tf.position;
                    break;
                default:
                    Debug.LogError("Invalid target");
                    break;
            }
        }
        protected override void DoReset() {
            base.DoReset();
            switch(target) {
                case RectTransform rtf:
                    rtf.anchoredPosition3D = _from;
                    break;
                case Transform tf:
                    tf.position = _from;
                    break;
                default:
                    Debug.LogError("Invalid target");
                    break;
            }
        }
        protected override void DoPlay() {
#if DOTWEEN
            switch(target) {
                case RectTransform rtf:
                    AddTween(rtf.DOShakeAnchorPos(time.duration, strength, vibrato, randomness, snapping, fadeOut));
                    break;
                case Transform tf:
                    AddTween(tf.DOShakePosition(time.duration, strength, vibrato, randomness, snapping, fadeOut));
                    break;
                default:
                    Debug.LogError("Invalid target");
                    break;
            }
#endif
        }
    }
}
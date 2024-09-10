using System;
using UnityEngine;
#if DOTWEEN
using DG.Tweening;
#endif

namespace MFramework {
    [Serializable, RefCompMenu("Punch/Position")]
    public class PunchPositionClip : AnimClip {
        [CompSelector(typeof(RectTransform), typeof(Transform))]
        public UnityEngine.Object target;
        [AnimClipToFrom]
        public Vector3 to;
        [Range(0, 50)]
        public int vibrato = 10;
        [Range(0f, 1f)]
        public float elasticity = 1f;
        public bool snapping;
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
                    AddTween(rtf.DOPunchAnchorPos(to, time.duration, vibrato, elasticity, snapping));
                    break;
                case Transform tf:
                    AddTween(tf.DOPunchPosition(to, time.duration, vibrato, elasticity, snapping));
                    break;
                default:
                    Debug.LogError("Invalid target");
                    break;
            }
#endif
        }
    }
}
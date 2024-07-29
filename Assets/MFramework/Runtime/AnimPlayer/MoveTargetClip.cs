using System;
using UnityEngine;
#if DOTWEEN
using DG.Tweening;
#endif

namespace MFramework {
    [Serializable, RefCompMenu("Transform/MoveTarget")]
    public class MoveTargetClip : AnimClip {
        [CompSelector(typeof(RectTransform), typeof(Transform))]
        public UnityEngine.Object target;
        [AnimClipToFrom]
        public Transform to;
        public bool snapping;
        public bool relative;

        public override bool IsRelative => relative;

        [SerializeField, HideInInspector] private Vector3 _from;
        /******************************************************************
         * 
         *      override
         * 
         ******************************************************************/
        protected override void DoInit() {
            base.DoInit();
            switch(target) {
                case RectTransform rtf:
                    _from = rtf.position;
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
                    rtf.position = _from;
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
            Vector3 to;
            switch(this.to) {
                case RectTransform rtf:
                    var rTarget = target as RectTransform;
                    to = DOTweenModuleUI.Utils.SwitchToRectTransform(rtf, rTarget);
                    break;
                default:
                    to = this.to.position;
                    break;
            }

            Tween t = null;
            switch(target) {
                case RectTransform rtf:
                    t = rtf.DOMove(to, time.duration, snapping);
                    break;
                case Transform tf:
                    t = tf.DOMove(to, time.duration, snapping);
                    break;
                default:
                    Debug.LogError("Invalid target");
                    break;
            }
            Debug.Assert(t != null);
            t.SetRelative(relative);
            AddTween(t);
#endif
        }
    }
}
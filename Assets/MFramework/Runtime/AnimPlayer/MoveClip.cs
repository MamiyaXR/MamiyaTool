using System;
using UnityEngine;
#if DOTWEEN
using DG.Tweening;
#endif

namespace MFramework {
    [Serializable, RefCompMenu("Transform/Move")]
    public class MoveClip : AnimClip {
        [CompSelector(typeof(RectTransform), typeof(Transform))]
        public UnityEngine.Object target;
        [AnimClipToFrom]
        public Vector3 to;
        public bool local = true;
        public bool self;
        public bool snapping;
        public bool relative;

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
            switch(target) {
                case RectTransform rtf:
                    if(self) {
                        _from = rtf.localPosition;
                    } else if(local) {
                        _from = rtf.anchoredPosition;
                    } else {
                        _from = rtf.position;
                    }
                    break;
                case Transform tf:
                    if(self) {
                        _from = tf.localPosition;
                    } else if(local) {
                        _from = tf.localPosition;
                    } else {
                        _from = tf.position;
                    }
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
                    if(self) {
                        rtf.localPosition = _from;
                    } else if(local) {
                        rtf.anchoredPosition = _from;
                    } else {
                        rtf.position = _from;
                    }
                    break;
                case Transform tf:
                    if(self) {
                        tf.localPosition = _from;
                    } else if(local) {
                        tf.localPosition = _from;
                    } else {
                        tf.position = _from;
                    }
                    break;
                default:
                    Debug.LogError("Invalid target");
                    break;
            }
        }
        protected override void DoPlay() {
#if DOTWEEN
            Tween t = null;
            switch(target) {
                case RectTransform rtf:
                    if(self) {
                        t = rtf.DOLocalMove(rtf.localPosition + to, time.duration, snapping);
                    } else if(local) {
                        t = rtf.DOLocalMove(AnchoredToLocalPosition(rtf, to), time.duration, snapping);
                    } else {
                        t = rtf.DOMove(to, time.duration, snapping);
                    }
                    break;
                case Transform tf:
                    if(self) {
                        t = tf.DOMove(tf.position + to, time.duration, snapping);
                    } else if(local) {
                        t = tf.DOLocalMove(to, time.duration, snapping);
                    } else {
                        t = tf.DOMove(to, time.duration, snapping);
                    }
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
        /******************************************************************
         * 
         *      
         * 
         ******************************************************************/
        public static Vector2 LocalToAnchoredPosition(RectTransform transform, Vector2 localPosition2D) {
            if(transform == null || transform.Equals(null)) return Vector2.zero;
            RectTransform parent = transform.parent?.GetComponent<RectTransform>();
            if(parent == null) return Vector2.zero;

            // 在传入 localPosition2D 的情况下，就不能使用刚才化简的结果了，使用最早一版的公式
            Rect parentRect = parent.rect;
            Vector2 pivot = transform.pivot;
            Vector2 anchorMin = transform.anchorMin;
            Vector2 anchorMax = transform.anchorMax;
            Vector2 anchorMinPos = parentRect.min + anchorMin * parentRect.size;
            Vector2 anchorMaxPos = parentRect.min + anchorMax * parentRect.size;

            return localPosition2D - (anchorMinPos + (anchorMaxPos - anchorMinPos) * pivot);
        }
        public static Vector2 AnchoredToLocalPosition(RectTransform transform, Vector2 anchoredPosition) {
            if(transform == null || transform.Equals(null)) return Vector2.zero;
            RectTransform parent = transform.parent?.GetComponent<RectTransform>();
            if(parent == null) return Vector2.zero;

            Rect parentRect = parent.rect;
            Vector2 pivot = transform.pivot;
            Vector2 anchorMin = transform.anchorMin;
            Vector2 anchorMax = transform.anchorMax;
            Vector2 anchorMinPos = parentRect.min + anchorMin * parentRect.size;
            Vector2 anchorMaxPos = parentRect.min + anchorMax * parentRect.size;

            return anchoredPosition + (anchorMinPos + (anchorMaxPos - anchorMinPos) * pivot);
        }
    }
}
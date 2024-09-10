using System;
using UnityEngine;
#if DOTWEEN
using DG.Tweening;
#endif

namespace MFramework {
    [Serializable, RefCompMenu("UI/Size")]
    public class UISizeClip : AnimClip {
        public RectTransform target;
        [AnimClipToFrom]
        public Vector2 to;
        public bool relative;

        public override bool IsRelative => relative;

        [SerializeField, HideInInspector]
        private Vector2 _from;
        /******************************************************************
         * 
         *      override
         * 
         ******************************************************************/
        protected override void DoInit() {
            base.DoInit();
            _from = target.sizeDelta;
        }
        protected override void DoReset() {
            base.DoReset();
            target.sizeDelta = _from;
        }
        protected override void DoPlay() {
#if DOTWEEN
            AddTween(target.DOSizeDelta(to, time.duration));
#endif
        }
    }
}
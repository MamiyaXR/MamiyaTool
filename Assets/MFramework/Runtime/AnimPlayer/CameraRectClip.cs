using System;
using UnityEngine;
#if DOTWEEN
using DG.Tweening;
#endif

namespace MFramework {
    [Serializable, RefCompMenu("Camera/Rect")]
    public class CameraRectClip : AnimClip {
        public Camera target;
        [AnimClipToFrom]
        public Rect to;

        [SerializeField, HideInInspector]
        private Rect _from;
        /******************************************************************
         * 
         *      override
         * 
         ******************************************************************/
        protected override void DoInit() {
            base.DoInit();
            _from = target.rect;
        }
        protected override void DoReset() {
            base.DoReset();
            target.rect = _from;
        }
        protected override void DoPlay() {
#if DOTWEEN
            AddTween(target.DORect(to, time.duration));
#endif
        }
    }
}
using System;
using UnityEngine;
#if DOTWEEN
using DG.Tweening;
#endif

namespace MFramework {
    [Serializable, RefCompMenu("Camera/PixelRect")]
    public class CameraPixelRectClip : AnimClip {
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
            _from = target.pixelRect;
        }
        protected override void DoReset() {
            base.DoReset();
            target.pixelRect = _from;
        }
        protected override void DoPlay() {
#if DOTWEEN
            AddTween(target.DOPixelRect(to, time.duration));
#endif
        }
    }
}
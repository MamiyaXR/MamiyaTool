using System;
using UnityEngine;
#if DOTWEEN
using DG.Tweening;
#endif

namespace MFramework {
    [Serializable, RefCompMenu("Camera/BGColor")]
    public class CameraBGColorClip : AnimClip {
        public Camera target;
        [AnimClipToFrom]
        public Color to;

        [SerializeField, HideInInspector]
        private Color _from;
        /******************************************************************
         * 
         *      override
         * 
         ******************************************************************/
        protected override void DoInit() {
            base.DoInit();
            _from = target.backgroundColor;
        }
        protected override void DoReset() {
            base.DoReset();
            target.backgroundColor = _from;
        }
        protected override void DoPlay() {
#if DOTWEEN
            AddTween(target.DOColor(to, time.duration));
#endif
        }
    }
}
using System;
using UnityEngine;
#if DOTWEEN
using DG.Tweening;
#endif

namespace MFramework {
    [Serializable, RefCompMenu("Transform/Scale")]
    public class ScaleClip : AnimClip {
        public Transform target;
        [AnimClipToFrom]
        public Vector3 to = Vector3.one;
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
            _from = target.localScale;
        }
        protected override void DoReset() {
            base.DoReset();
            target.localScale = _from;
        }
        protected override void DoPlay() {
#if DOTWEEN
            AddTween(target.DOScale(to, time.duration).SetRelative(relative));
#endif
        }
    }
}
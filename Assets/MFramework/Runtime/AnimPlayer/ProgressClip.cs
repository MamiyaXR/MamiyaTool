using System;
using UnityEngine;
using UnityEngine.UI;
#if DOTWEEN
using DG.Tweening;
#endif

namespace MFramework {
    [Serializable, RefCompMenu("UI/Progress")]
    public class ProgressClip : AnimClip {
        public Image target;
        [AnimClipToFrom, Range(0f, 1f)]
        public float to = 1f;

        [SerializeField, HideInInspector]
        private float _from;
        /******************************************************************
         * 
         *      override
         * 
         ******************************************************************/
        protected override void DoInit() {
            base.DoInit();
            _from = target.fillAmount;
        }
        protected override void DoReset() {
            base.DoReset();
            target.fillAmount = _from;
        }
        protected override void DoPlay() {
#if DOTWEEN
            AddTween(target.DOFillAmount(to, time.duration));
#endif
        }
    }
}
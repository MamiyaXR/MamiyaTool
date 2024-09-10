using System;
using UnityEngine;
using UnityEngine.UI;
#if DOTWEEN
using DG.Tweening;
#endif

namespace MFramework {
    [Serializable, RefCompMenu("UI/Slider")]
    public class SliderValueClip : AnimClip {
        public Slider target;
        [AnimClipToFrom]
        public float to = 100;

        [SerializeField, HideInInspector]
        private float _from;
        /******************************************************************
         * 
         *      override
         * 
         ******************************************************************/
        protected override void DoInit() {
            base.DoInit();
            _from = target.value;
        }
        protected override void DoReset() {
            base.DoReset();
            target.value = _from;
        }
        protected override void DoPlay() {
#if DOTWEEN
            AddTween(target.DOValue(to, time.duration));
#endif
        }
    }
}
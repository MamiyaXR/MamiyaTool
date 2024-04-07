using System;
using UnityEngine;

namespace MFramework {
    [Serializable, RefCompMenu("GameObject/Enable")]
    public class EnableClip : AnimClip {
        public Behaviour target;
        public bool enable = true;

        [SerializeField, HideInInspector]
        private bool _orgEnable;
        /******************************************************************
         * 
         *      override
         * 
         ******************************************************************/
        protected override void DoInit() {
            base.DoInit();
            _orgEnable = target.enabled;
        }
        protected override void DoReset() {
            base.DoReset();
            target.enabled = _orgEnable;
        }
        protected override void DoPlay() {
            target.enabled = enable;
        }
    }
}
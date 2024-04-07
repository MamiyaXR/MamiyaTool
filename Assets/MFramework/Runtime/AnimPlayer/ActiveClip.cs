using System;
using UnityEngine;

namespace MFramework {
    [Serializable, RefCompMenu("GameObject/Active")]
    public class ActiveClip : AnimClip {
        public GameObject target;
        public bool active = true;

        [SerializeField, HideInInspector]
        private bool _orgActive;
        /******************************************************************
         * 
         *      override
         * 
         ******************************************************************/
        protected override void DoInit() {
            base.DoInit();
            _orgActive = target.activeSelf;
        }
        protected override void DoReset() {
            base.DoReset();
            target.SetActive(_orgActive);
        }
        protected override void DoPlay() {
            target.SetActive(active);
        }
    }
}
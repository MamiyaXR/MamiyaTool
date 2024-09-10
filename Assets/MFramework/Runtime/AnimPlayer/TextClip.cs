using System;
using UnityEngine;
using UnityEngine.UI;
#if DOTWEEN
using DG.Tweening;
#endif

namespace MFramework {
    [Serializable, RefCompMenu("UI/Text")]
    public class TextClip : AnimClip {
        public Text target;
        [AnimClipToFrom]
        public string to;
        public bool richText;
        public ScrambleMode scrambleMode = ScrambleMode.None;
        public string scrambleChars;
        public bool relative;

        public override bool IsRelative => relative;

        [SerializeField, HideInInspector]
        private string _from;
        /******************************************************************
         * 
         *      override
         * 
         ******************************************************************/
        protected override void DoInit() {
            base.DoInit();
            _from = target.text;
        }
        protected override void DoReset() {
            base.DoReset();
            target.text = _from;
        }
        protected override void DoPlay() {
#if DOTWEEN
            AddTween(target.DOText(to, time.duration, richText, scrambleMode, scrambleChars).SetRelative(relative));
#endif
        }
    }
}
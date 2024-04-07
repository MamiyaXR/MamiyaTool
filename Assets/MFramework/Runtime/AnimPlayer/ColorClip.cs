using System;
using UnityEngine;
using UnityEngine.UI;
#if DOTWEEN
using DG.Tweening;
#endif

namespace MFramework {
    [Serializable, RefCompMenu("Color/Color")]
    public class ColorClip : AnimClip {
        [CompSelector(typeof(SpriteRenderer),
            typeof(Renderer),
            typeof(Light),
            typeof(Text),
            typeof(Graphic))]
        public UnityEngine.Object target;
        [AnimClipToFrom]
        public Color to = Color.white;
        [SerializeField, HideInInspector]
        private Color _from;
        /******************************************************************
         * 
         *      override
         * 
         ******************************************************************/
        protected override void DoInit() {
            base.DoInit();
            switch(target) {
                case SpriteRenderer sr:
                    _from = sr.color;
                    break;
                case Renderer r:
                    _from = r.material.color;
                    break;
                case Light l:
                    _from = l.color;
                    break;
                case Text txt:
                    _from = txt.color;
                    break;
                case Graphic g:
                    _from = g.color;
                    break;
            }
        }
        protected override void DoReset() {
            base.DoReset();
            switch(target) {
                case SpriteRenderer sr:
                    sr.color = _from;
                    break;
                case Renderer r:
                    r.material.color = _from;
                    break;
                case Light l:
                    l.color = _from;
                    break;
                case Text txt:
                    txt.color = _from;
                    break;
                case Graphic g:
                    g.color = _from;
                    break;
            }
        }
        protected override void DoPlay() {
#if DOTWEEN
            switch(target) {
                case SpriteRenderer sr:
                    AddTween(sr.DOColor(to, time.duration));
                    break;
                case Renderer r:
                    AddTween(r.material.DOColor(to, time.duration));
                    break;
                case Light l:
                    AddTween(l.DOColor(to, time.duration));
                    break;
                case Text txt:
                    AddTween(txt.DOColor(to, time.duration));
                    break;
                case Graphic g:
                    AddTween(g.DOColor(to, time.duration));
                    break;
            }
#endif
        }
    }
}
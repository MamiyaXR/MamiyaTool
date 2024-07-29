using System;
using UnityEngine;
using UnityEngine.UI;
#if DOTWEEN
using DG.Tweening;
#endif

namespace MFramework {
    [Serializable, RefCompMenu("Color/Fade")]
    public class FadeClip : AnimClip {
        [CompSelector(typeof(SpriteRenderer),
            typeof(Renderer),
            typeof(Light),
            typeof(Text),
            typeof(Graphic),
            typeof(CanvasGroup))]
        public UnityEngine.Object target;
        [AnimClipToFrom(), Range(0f, 1f)]
        public float to = 0f;

        [SerializeField, HideInInspector]
        private float _from;
        /******************************************************************
         * 
         *      override
         * 
         ******************************************************************/
        protected override void DoInit() {
            base.DoInit();
            switch(target) {
                case SpriteRenderer sr:
                    _from = sr.color.a;
                    break;
                case Renderer r:
                    _from = r.material.color.a;
                    break;
                case Light l:
                    _from = l.intensity;
                    break;
                case Text txt:
                    _from = txt.color.a;
                    break;
                case Graphic g:
                    _from = g.color.a;
                    break;
                case CanvasGroup grp:
                    _from = grp.alpha;
                    break;
            }
        }
        protected override void DoReset() {
            base.DoReset();
            switch(target) {
                case SpriteRenderer sr:
                    sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, _from);
                    break;
                case Renderer r:
                    r.material.color = new Color(r.material.color.r, r.material.color.g, r.material.color.b, _from);
                    break;
                case Light l:
                    l.intensity = _from;
                    break;
                case Text txt:
                    txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, _from);
                    break;
                case Graphic g:
                    g.color = new Color(g.color.r, g.color.g, g.color.b, _from);
                    break;
                case CanvasGroup grp:
                    grp.alpha = _from;
                    break;
            }
        }
        protected override void DoPlay() {
#if DOTWEEN
            switch(target) {
                case SpriteRenderer sr:
                    AddTween(sr.DOFade(to, time.duration));
                    break;
                case Renderer r:
                    AddTween(r.material.DOFade(to, time.duration));
                    break;
                case Light l:
                    AddTween(l.DOIntensity(to, time.duration));
                    break;
                case Text txt:
                    AddTween(txt.DOFade(to, time.duration));
                    break;
                case Graphic g:
                    AddTween(g.DOFade(to, time.duration));
                    break;
                case CanvasGroup grp:
                    AddTween(grp.DOFade(to, time.duration));
                    break;
            }
#endif
        }
    }
}
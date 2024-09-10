using UnityEngine;
using UnityEditor;
using DG.DOTweenEditor;

namespace MFramework {
    [CustomEditor(typeof(AnimPlayer))]
    public class AnimPlayerEditor : CustomMonoBehaviourInspector {
        private static int _PlayCounter = 0;

        private static void UpdatePreview(int offset) {
            _PlayCounter += offset;
            //Debug.Log($"AnimPlayerEditor.UpdatePreview {_PlayCounter}");

            if(_PlayCounter == 1) {
                DOTweenEditorPreview.Start();
                AnimPlayer.OnAddTween = (t) => {
                    DOTweenEditorPreview.PrepareTweenForPreview(t);
                };
            } else if(_PlayCounter <= 0) {
                AnimPlayer.OnAddTween = null;
                DOTweenEditorPreview.Stop();
            }
        }

        private void OnEnable() {
            if(Application.isPlaying)
                return;

            //Debug.Log($"AnimPlayerEditor.OnEnable", target);
            UpdatePreview(1);
        }

        private void OnDisable() {
            if(Application.isPlaying)
                return;

            //Debug.Log($"AnimPlayerEditor.OnDisable", target);
            UpdatePreview(-1);
        }
    }
}
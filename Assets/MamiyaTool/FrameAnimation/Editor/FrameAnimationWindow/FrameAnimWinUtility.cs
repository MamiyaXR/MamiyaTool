using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace MamiyaTool {
    public static class FrameAnimWinUtility {
        public static bool CompareAssetList(List<FrameAnimationAsset> listA, List<FrameAnimationAsset> listB) {
            if(listA == null || listB == null)
                return false;
            if(listA.Count != listB.Count)
                return false;
            for(int i = 0; i < listA.Count; ++i) {
                if(listA[i].GetHashCode() != listB[i].GetHashCode())
                    return false;
            }
            return true;
        }
        // What is the first animation player component (Animator or Animation) when recursing parent tree toward root
        public static FrameAnimator GetClosestAnimationPlayerComponentInParents(Transform tr) {
            while(true) {
                if(tr.TryGetComponent(out FrameAnimator animator)) {
                    return animator;
                }

                if(tr == tr.root)
                    break;

                tr = tr.parent;
            }
            return null;
        }

        public static void DrawInRangeOverlay(Rect rect, Color color, float startOfClipPixel, float endOfClipPixel) {
            // Rect shaded shape drawn inside range
            if(endOfClipPixel >= rect.xMin) {
                if(color.a > 0f) {
                    Rect inRect = Rect.MinMaxRect(Mathf.Max(startOfClipPixel, rect.xMin), rect.yMin, Mathf.Min(endOfClipPixel, rect.xMax), rect.yMax);
                    DrawRect(inRect, color);
                }
            }
        }

        public static void DrawOutOfRangeOverlay(Rect rect, Color color, float startOfClipPixel, float endOfClipPixel) {
            Color lineColor = Color.white.RGBMultiplied(0.4f);

            // Rect shaded shape drawn before range
            if(startOfClipPixel > rect.xMin) {
                Rect startRect = Rect.MinMaxRect(rect.xMin, rect.yMin, Mathf.Min(startOfClipPixel, rect.xMax), rect.yMax);
                DrawRect(startRect, color);
                TimeArea.DrawVerticalLine(startRect.xMax, startRect.yMin, startRect.yMax, lineColor);
            }

            // Rect shaded shape drawn after range
            Rect endRect = Rect.MinMaxRect(Mathf.Max(endOfClipPixel, rect.xMin), rect.yMin, rect.xMax, rect.yMax);
            DrawRect(endRect, color);
            TimeArea.DrawVerticalLine(endRect.xMin, endRect.yMin, endRect.yMax, lineColor);
        }

        public static void DrawRect(Rect rect, Color color) {
            if(Event.current.type != EventType.Repaint)
                return;

            //HandleUtility.ApplyWireMaterial();
            ReflectionUtility.InvokeStaticMethod<HandleUtility>("ApplyWireMaterial");
            GL.PushMatrix();
            GL.MultMatrix(Handles.matrix);
            GL.Begin(GL.QUADS);
            GL.Color(color);
            GL.Vertex(rect.min);
            GL.Vertex(new Vector2(rect.xMax, rect.yMin));
            GL.Vertex(rect.max);
            GL.Vertex(new Vector2(rect.xMin, rect.yMax));
            GL.End();
            GL.PopMatrix();
        }
    }
}
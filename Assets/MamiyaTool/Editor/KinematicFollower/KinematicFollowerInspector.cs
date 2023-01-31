using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace MamiyaTool
{
    [CustomEditor(typeof(KinematicFollower))]
    public class KinematicFollowerInspector : Editor
    {
        private List<Vector3> xList = new List<Vector3>();
        private List<Vector3> yList = new List<Vector3>();
        private float k1;
        private float k2;
        private float k3;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            KinematicFollower ctrl = (KinematicFollower)target;

            k1 = ctrl.z / Mathf.PI / ctrl.f;
            k2 = 1 / Mathf.Pow(2f * Mathf.PI * ctrl.f, 2);
            k3 = ctrl.r * ctrl.z / (2 * Mathf.PI * ctrl.f);

            EditorGUILayout.Space(200);
            DrawArrow(new Vector2(20, 300), new Vector2(340, 300), Color.white);
            DrawArrow(new Vector2(20, 300), new Vector2(20, 130), Color.white);
            DrawCurval(new Vector3(20, 300, 0));
        }

        private void DrawArrow(Vector2 from, Vector2 to, Color color)
        {
            Handles.BeginGUI();
            Handles.color = color;
            Handles.DrawAAPolyLine(3, from, to);
            Vector2 v0 = from - to;
            v0 *= 10 / v0.magnitude;
            Vector2 v1 = new Vector2(v0.x * 0.866f - v0.y * 0.5f, v0.x * 0.5f + v0.y * 0.866f);
            Vector2 v2 = new Vector2(v0.x * 0.866f + v0.y * 0.5f, v0.x * -0.5f + v0.y * 0.866f);
            ;
            Handles.DrawAAPolyLine(3, to + v1, to, to + v2);
            Handles.EndGUI();
        }
        private void DrawCurval(Vector3 from)
        {
            float scale = 150f;
            float step = 1 / scale;
            float k2_stable = Mathf.Max(k2, step * step / 2f + step * k1 / 2, step * k1);
            Handles.BeginGUI();
            float dx = 0;
            float y = 0;
            float dy = 0;
            for(float i = 0; i < 1; i += step) {
                xList.Add(from + new Vector3(i * 2, -GetX(i), 0) * scale);
                y = y + step * dy;
                yList.Add(from + new Vector3(i * 2, -y, 0) * scale);
                dx = (GetX(i) - GetX(i - step)) / step;
                dy = dy + step * (GetX(i) + k3 * dx - y - k1 * dy) / k2_stable;
            }
            Handles.color = Color.green;
            Handles.DrawAAPolyLine(1.5f, xList.ToArray());
            Handles.color = Color.red;
            Handles.DrawAAPolyLine(1.5f, yList.ToArray());
            Handles.EndGUI();

            xList.Clear();
            yList.Clear();
        }
        private float GetX(float i)
        {
            if(i <= 0)
                return 0;
            else
                return 1;
        }
    }
}
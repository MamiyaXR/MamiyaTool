using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MamiyaTool {
    [CustomPropertyDrawer(typeof(KinematicHandle))]
    public class KinematicHandleDrawer : PropertyDrawer {
        private const string F = "f";
        private const string Z = "z";
        private const string R = "r";
        private const float scale = 150f;
        private const float lineWidth = 1.5f;
        private const float fieldWidth = 50f;
        private const float labelWidth = 10f;
        private const float space = 10f;
        private const float graphHeigt = 400f;
        private const float padding = 20f;
        private float height => EditorGUIUtility.singleLineHeight;
        private float fullHeight => height + 2f;
        private bool upfold = false;
        /******************************************************************
         *
         *      lifecycle
         *
         ******************************************************************/
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginProperty(position, label, property);
            Rect posCache = position;
            // 绘制foldout
            Rect posFoldout = new Rect(posCache.x, posCache.y, posCache.width, height);
            upfold = EditorGUI.Foldout(posFoldout, upfold, GUIContent.none);
            var propertyF = property.FindPropertyRelative(F);
            var propertyZ = property.FindPropertyRelative(Z);
            var propertyR = property.FindPropertyRelative(R);
            if(upfold) {
                // 绘制标签
                Rect rectL = new Rect(position.x, position.y, position.width, height);
                EditorGUI.LabelField(rectL, label);
                position = new Rect(position.x, position.y + fullHeight, position.width, position.height - fullHeight);
                // 绘制属性
                position = DrawPropertyV(position, propertyF, F);
                position = DrawPropertyV(position, propertyZ, Z);
                position = DrawPropertyV(position, propertyR, R);
                // 绘制曲线
                DrawGraph(position, propertyF.floatValue, propertyZ.floatValue, propertyR.floatValue);
            } else {
                // 绘制标签
                position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
                // 绘制属性
                position = DrawPropertyH(position, propertyF, F);
                position = DrawPropertyH(position, propertyZ, Z);
                position = DrawPropertyH(position, propertyR, R);
            }
            EditorGUI.EndProperty();
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return upfold ? fullHeight * 4f + graphHeigt : height;
        }
        /******************************************************************
         *
         *      private method
         *
         ******************************************************************/
        private Rect DrawPropertyH(Rect position, SerializedProperty property, string label) {
            float x = position.x;
            float y = position.y;
            float width = 0f;
            Rect rectLabel = new Rect(x, y, labelWidth, height);
            x += labelWidth;
            width += labelWidth;
            Rect rectField = new Rect(x, y, fieldWidth, height);
            x += fieldWidth + space;
            width += fieldWidth + space;
            EditorGUI.LabelField(rectLabel, label);
            EditorGUI.PropertyField(rectField, property, GUIContent.none);
            return new Rect(x, y, position.width - width, position.height);
        }
        private Rect DrawPropertyV(Rect position, SerializedProperty property, string label) {
            Rect rect = new Rect(position.x, position.y, position.width, height);
            property.floatValue = EditorGUI.FloatField(rect, label, property.floatValue);
            return new Rect(position.x, position.y + fullHeight, position.width, position.height - fullHeight);
        }
        private void DrawGraph(Rect position, float f, float z, float r) {
            // axle x
            Vector2 from = new Vector2(position.x + padding, position.yMax - padding);
            Vector2 to = new Vector2(position.xMax - padding, position.yMax - padding);
            DrawArrow(from, to, Color.white);
            // axle y
            to = new Vector2(position.x + padding, position.y + padding);
            DrawArrow(from, to, Color.white);
            // curve
            to = new Vector2(position.xMax - padding, position.y + padding);
            DrawCurve(from, to, f, z, r);
        }
        private void DrawArrow(Vector2 from, Vector2 to, Color color) {
            Color colOrg = Handles.color;
            Handles.BeginGUI();
            Handles.color = color;
            Handles.DrawAAPolyLine(3, from, to);
            Vector2 v0 = from - to;
            v0 *= 10 / v0.magnitude;
            Vector2 v1 = new Vector2(v0.x * 0.866f - v0.y * 0.5f, v0.x * 0.5f + v0.y * 0.866f);
            Vector2 v2 = new Vector2(v0.x * 0.866f + v0.y * 0.5f, v0.x * -0.5f + v0.y * 0.866f);
            Handles.DrawAAPolyLine(3, to + v1, to, to + v2);
            Handles.EndGUI();
            Handles.color = colOrg;
        }
        private void DrawCurve(Vector2 from, Vector2 to, float f, float z, float r) {
            List<Vector3> xList = new List<Vector3>();
            List<Vector3> yList = new List<Vector3>();
            float step = 1f / scale;
            KinematicHandle handle = new KinematicHandle(f, z, r);
            float y = 0f;
            float dy = 0f;
            float xPre = 0f;
            Handles.BeginGUI();
            Color colOrg = Handles.color;
            for(float i = 0f; i < 1f; i += step) {
                float x = GetX(i);
                (y, dy) = handle.Invoke(y, dy, x, xPre, step);
                xPre = x;

                float posX = Mathf.LerpUnclamped(from.x, to.x, i);
                float posY = Mathf.LerpUnclamped(from.y, to.y, x);
                xList.Add(new Vector3(posX, posY, 0f));
                posY = Mathf.LerpUnclamped(from.y, to.y, y);
                yList.Add(new Vector3(posX, posY, 0f));
            }
            Handles.color = Color.green;
            Handles.DrawAAPolyLine(lineWidth, xList.ToArray());
            Handles.color = Color.red;
            Handles.DrawAAPolyLine(lineWidth, yList.ToArray());
            Handles.color = colOrg;
            Handles.EndGUI();
            xList.Clear();
            yList.Clear();
        }
        private float GetX(float x) {
            return x > 0.1f ? 0.5f : 0f;
        }
    }
}
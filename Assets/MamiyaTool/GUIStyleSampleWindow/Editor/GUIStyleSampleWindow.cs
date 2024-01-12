using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;
using System;

namespace MamiyaTool {
    public class GUIStyleSampleWindow : EditorWindow {
        private Vector2 m_ScrollViewPosition = Vector2.zero;
        private List<GUIStyle> m_AllGUIStyleList;
        private List<GUIStyle> m_ShowingGUIStyleList;
        private bool m_IsShowText = true;
        private Vector2 m_CellSize = new Vector2(200, 100);
        private string m_SearchString;
        private bool m_HasInit;
        [MenuItem("Tools/GUIStyleSamples")]
        private static void ShowWindow() {
            var window = GetWindow<GUIStyleSampleWindow>();
            window.titleContent = new GUIContent("GUIStyleSamples");
            window.Show();
        }

        private void TryInit() {
            if(m_HasInit)
                return;

            m_AllGUIStyleList = new List<GUIStyle>(700);
            m_ShowingGUIStyleList = new List<GUIStyle>(700);

            var pp = GUI.skin.GetType().GetProperties();
            for(int i = 0; i < pp.Length; i++) {
                if(pp[i].GetValue(GUI.skin) is GUIStyle style) {
                    m_AllGUIStyleList.Add(style);
                    m_ShowingGUIStyleList.Add(style);
                }
            }

            var styles = GUI.skin.GetType().GetField("m_CustomStyles", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).GetValue(GUI.skin) as GUIStyle[];
            if(styles != null) {
                for(int i = 0; i < styles.Length; i++) {
                    var style = styles[i];
                    m_AllGUIStyleList.Add(style);
                    m_ShowingGUIStyleList.Add(style);
                }
            }

            m_HasInit = true;
        }

        private void UpdateShowingGUIStyleList() {
            m_ShowingGUIStyleList.Clear();
            var isShowAll = string.IsNullOrEmpty(m_SearchString);
            var lowerSearchString = isShowAll ? "" : m_SearchString.ToLower();
            for(int i = 0; i < m_AllGUIStyleList.Count; i++) {
                var style = m_AllGUIStyleList[i];
                if(style.name.ToLower().Contains(lowerSearchString)) {
                    m_ShowingGUIStyleList.Add(style);
                }
            }

        }

        private void OnGUI() {
            TryInit();

            DrawHeader();

            var column = Math.Max((int)(position.width / m_CellSize.x), 6);
            m_ScrollViewPosition = GUILayout.BeginScrollView(m_ScrollViewPosition);
            DrawStyles(column);
            GUILayout.EndScrollView();
        }

        private void DrawHeader() {
            GUILayout.BeginHorizontal(GUI.skin.GetStyle("PreBackground"), GUILayout.Height(24));
            m_IsShowText = GUILayout.Toggle(m_IsShowText, "Show text", GUILayout.Width(120));
            GUILayout.Label("Search", GUILayout.Width(50));
            var newSearchString = GUILayout.TextField(m_SearchString, GUILayout.Width(200));
            if(newSearchString != m_SearchString) {
                m_SearchString = newSearchString;
                UpdateShowingGUIStyleList();
            }
            GUILayout.EndHorizontal();
        }

        private void DrawStyles(int column) {
            GUILayout.BeginVertical();
            for(int i = 0; i < m_ShowingGUIStyleList.Count; i++) {
                if(i % column == 0) {
                    GUILayout.Space(2);
                    GUILayout.BeginHorizontal();
                }

                GUILayout.Space(4);
                GUILayout.BeginVertical(GUI.skin.window, GUILayout.Width(m_CellSize.x), GUILayout.Height(m_CellSize.y));
                var style = m_ShowingGUIStyleList[i];
                GUILayout.Space(-18);
                GUILayout.TextField($"{ (i + 1).ToString("D3")}:{ style.name}");
                if(GUILayout.Button(m_IsShowText ? "Text" : "    ", style)) {
                    GUIUtility.systemCopyBuffer = $"GUI.skin.GetStyle(\"{style.name}\")";
                    Debug.Log($"复制到剪切板:{ GUIUtility.systemCopyBuffer}");
                }
                GUILayout.EndVertical();

                if(i % column == column - 1 || i == m_ShowingGUIStyleList.Count - 1) {
                    GUILayout.EndHorizontal();
                    GUILayout.Space(2);
                }
            }
            GUILayout.EndVertical();
        }
    }
}
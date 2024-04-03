using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Diagnostics;

using Debug = UnityEngine.Debug;

namespace MFramework {
    public static class EditorHelper {
        public static void DrawProperty(SerializedObject so, string propName) {
            var property = so.FindProperty(propName);
            so.Update();
            EditorGUILayout.PropertyField(property, true);
            so.ApplyModifiedProperties();
        }

        #region DrawMenu
        public class Func {
            public string menu;
            public MethodInfo method;
            public object[] GetDefaultArgs() {
                var mparams = method.GetParameters();
                if(mparams.Length == 0)
                    return null;
                var args = new object[mparams.Length];
                for(int i = 0; i < mparams.Length; i++) {
                    args[i] = mparams[i].ParameterType.IsValueType ? Activator.CreateInstance(mparams[i].ParameterType) : null;
                }
                return args;
            }
        }

        static void GetMenus(Type type, List<Func> funs) {
            if(type == typeof(System.Object))
                return;

            if(type.BaseType != null) {
                GetMenus(type.BaseType, funs);
            }

            BindingFlags bf = BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
            MethodInfo[] methods = type.GetMethods(bf);
            foreach(var m in methods) {
                var ma = m.GetCustomAttributes<ContextMenu>(true).ToArray();
                if(ma == null || ma.Length == 0)
                    continue;
                if(funs.Exists(x => x.method.Name == m.Name))
                    continue;

                funs.Add(new Func() { menu = ma[0].menuItem, method = m });
            }
        }
        static void SetGODirty(MonoBehaviour target) {
            EditorUtility.SetDirty(target);
            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if(prefabStage != null) {
                EditorSceneManager.MarkSceneDirty(prefabStage.scene);
            }
        }

        public static void DrawMenus(UnityEngine.Object target, float width) {
            List<Func> menuFuns = new();
            GetMenus(target.GetType(), menuFuns);
            if(menuFuns.Count <= 0)
                return;

            DrawButtons(menuFuns.Select(x => x.menu).ToArray(), 5, width - 30, (i) => {
                var func = menuFuns[i];
                func.method.Invoke(target, func.GetDefaultArgs());

                if(!Application.isPlaying) {
                    EditorUtility.SetDirty(target);
                }
            });
        }
        #endregion

        public static void DrawPadding() {
            GUILayout.Space(18f);
        }
        public static void DrawProperty(string label, SerializedProperty sp, bool padding, params GUILayoutOption[] options) {
            if(sp != null) {
                if(padding)
                    EditorGUILayout.BeginHorizontal();
                if(label != null)
                    EditorGUILayout.PropertyField(sp, new GUIContent(label), options);
                else
                    EditorGUILayout.PropertyField(sp, options);
                if(padding) {
                    DrawPadding();
                    EditorGUILayout.EndHorizontal();
                }
            }
        }
        public static void DrawProperty(string label, SerializedProperty sp, params GUILayoutOption[] options) {
            DrawProperty(label, sp, true, options);
        }
        public static void DrawText(SerializedObject so, string propName, float defWidth = 70f) {
            SerializedProperty sp = so.FindProperty(propName);
            so.Update();
            DrawText(sp, propName, defWidth);
        }
        public static void DrawTextArray(SerializedObject so, string propName, string itemName, float defWidth = 70f) {
            var myArrayProperty = so.FindProperty(propName);
            myArrayProperty.arraySize = EditorGUILayout.IntField("Sqls Count", myArrayProperty.arraySize);
            for(int i = 0; i < myArrayProperty.arraySize; ++i) {
                var myElement = myArrayProperty.GetArrayElementAtIndex(i);
                DrawText(myElement, itemName + i, defWidth);
            }
        }
        public static void DrawButtons(string[] btns, int rowMax, float size, Action<int> onClick) {
            var col = btns.Length % rowMax == 0 ? btns.Length / rowMax : btns.Length / rowMax + 1;
            var rcol = col > 1 ? rowMax : btns.Length;
            var lo = GUILayout.Width((size - 4.0f - (rcol - 1) * 4.0f) / rcol);
            for(int i = 0; i < col; ++i) {
                GUILayout.BeginHorizontal();
                for(int j = 0; j < rowMax; ++j) {
                    int index = i * rowMax + j;
                    if(index >= btns.Length)
                        break;
                    if(GUILayout.Button(btns[index], lo)) {
                        onClick(index);
                    }
                }
                GUILayout.EndHorizontal();
            }
        }
        public static void RunBat(string batFile) {
            using(Process p = new()) {
                p.StartInfo.FileName = batFile;
                p.StartInfo.Verb = "runas";
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.WorkingDirectory = ".";
                p.StartInfo.RedirectStandardInput = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.StandardOutputEncoding = System.Text.Encoding.UTF8;
                p.StartInfo.StandardErrorEncoding = System.Text.Encoding.UTF8;
                p.EnableRaisingEvents = true;
                p.OutputDataReceived += (sender, e) => {
                    Debug.Log(e.Data);
                };
                p.ErrorDataReceived += (sender, e) => {
                    Debug.LogError(e.Data);
                };

                p.Start();
                p.BeginOutputReadLine();
                p.BeginErrorReadLine();
                p.WaitForExit();

                AssetDatabase.Refresh();
            }
        }
        /******************************************************************
         * 
         *      private method
         * 
         ******************************************************************/
        private static void DrawText(SerializedProperty sp, string propName, float defWidth) {
            bool ww = GUI.skin.textField.wordWrap;
            GUI.skin.textField.wordWrap = true;
            if(sp.hasMultipleDifferentValues) {
                DrawProperty("", sp, GUILayout.Height(128f));
            } else {
                GUIStyle style = new GUIStyle(EditorStyles.textField);
                style.wordWrap = true;

                float height = style.CalcHeight(new GUIContent(sp.stringValue), Screen.width - 100f);
                bool offset = true;

                if(height > 90f) {
                    offset = false;
                    height = style.CalcHeight(new GUIContent(sp.stringValue), Screen.width - 20f);
                } else {
                    GUILayout.BeginHorizontal();
                    GUILayout.BeginVertical(GUILayout.Width(defWidth));
                    GUILayout.Space(3f);
                    GUILayout.Label(propName);
                    GUILayout.EndVertical();
                    GUILayout.BeginVertical();
                }
                Rect rect = EditorGUILayout.GetControlRect(GUILayout.Height(height));

                GUI.changed = false;
                string text = EditorGUI.TextArea(rect, sp.stringValue, style);
                if(GUI.changed)
                    sp.stringValue = text;

                if(offset) {
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                }
            }

            GUI.skin.textField.wordWrap = ww;
        }
    }
}
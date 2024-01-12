using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;

namespace MamiyaTool {
    public class RenameTool : EditorWindow {
        [MenuItem("Tools/Rename Tool")]
        public static void ShowWindow() {
            GetWindow(typeof(RenameTool), false, "Rename Tool");
        }
        /*****************************************************************
         * 
         *      field
         * 
         *****************************************************************/
        private SerializedObject _serializedObject;
        [SerializeField] private List<Param> paramList = new List<Param>();
        [SerializeField] private List<string> wordList = new List<string>();
        private DefaultAsset dir;
        private string filter;
        private bool curDir;
        /*****************************************************************
         * 
         *      lifecycle
         * 
         *****************************************************************/
        private void OnEnable() {
            _serializedObject = new SerializedObject(this);
            filter = @"t:Texture2D";
            curDir = true;
        }
        private void OnGUI() {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Folder :");
            dir = EditorGUILayout.ObjectField(dir, typeof(DefaultAsset), false) as DefaultAsset;
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Filter :");
            filter = GUILayout.TextField(filter);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Only current folder :");
            curDir = GUILayout.Toggle(curDir, GUIContent.none);
            EditorGUILayout.EndHorizontal();
            DrawList(nameof(paramList));
            DrawList(nameof(wordList));
            if(GUILayout.Button("Rename")) {
                Rename();
            }
            EditorGUILayout.EndVertical();
        }
        /*****************************************************************
         * 
         *      private method
         * 
         *****************************************************************/
        private void DrawList(string listName) {
            SerializedProperty property = _serializedObject.FindProperty(listName);
            if(property == null)
                return;
            _serializedObject.Update();
            EditorGUILayout.PropertyField(property);
            _serializedObject.ApplyModifiedProperties();
        }
        private void Rename() {
            string dirPath = AssetDatabase.GetAssetPath(dir);
            if(!AssetDatabase.IsValidFolder(dirPath))
                return;
            string[] guids = AssetDatabase.FindAssets(filter, new string[] { dirPath });
            foreach(string guid in guids) {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                if(curDir) {
                    string r = Path.GetFileName(Path.GetDirectoryName(path));
                    if(dir.name != r)
                        continue;
                }
                string oldName = Path.GetFileName(path);
                AssetDatabase.RenameAsset(path, GetNewName(oldName));
            }
            AssetDatabase.Refresh();
        }
        private string GetNewName(string oldName) {
            StringBuilder newNameBuilder = new StringBuilder();
            foreach(string word in wordList) {
                newNameBuilder.Append(word);
            }
            string newName = newNameBuilder.ToString();
            foreach(Param param in paramList) {
                Match match = Regex.Match(oldName, param.pattern);
                if(match.Success) {
                    newName = Regex.Replace(newName, param.key, match.Value);
                }
            }
            return newName;
        }
        /*****************************************************************
         * 
         *      define
         * 
         *****************************************************************/
        [Serializable]
        private struct Param {
            public string key;
            public string pattern;
        }
    }
}
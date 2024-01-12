using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace MamiyaTool {
    internal class AssetEditorPanel : IMGUIContainer {
        private EditorWindow m_Owner;
        private AnimEditor m_AnimEditor;
        /*****************************************************************
         * 
         *      public method
         * 
         *****************************************************************/
        public AssetEditorPanel(EditorWindow owner, AnimEditor editor) {
            m_Owner = owner;
            m_AnimEditor = editor;
            onGUIHandler += OnAssetEditorGUI;
        }
        /*****************************************************************
         * 
         *      private method
         * 
         *****************************************************************/
        private void OnAssetEditorGUI() {
            m_AnimEditor.OnAnimEditorGUI(m_Owner, contentRect);
        }
    }
}
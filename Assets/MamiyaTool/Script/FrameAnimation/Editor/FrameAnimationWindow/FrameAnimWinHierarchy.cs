﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace MamiyaTool {
    [Serializable]
    internal class FrameAnimWinHierarchyState : TreeViewState {
        private List<int> m_TallInstanceIDs = new List<int>();

        public bool GetTallMode(FrameAnimWinHierarchyNode node) {
            return m_TallInstanceIDs.Contains(node.id);
        }

        public void SetTallMode(FrameAnimWinHierarchyNode node, bool tallMode) {
            if(tallMode)
                m_TallInstanceIDs.Add(node.id);
            else
                m_TallInstanceIDs.Remove(node.id);
        }

        public int GetTallInstancesCount() {
            return m_TallInstanceIDs.Count;
        }

        public void AddTallInstance(int id) {
            if(!m_TallInstanceIDs.Contains(id))
                m_TallInstanceIDs.Add(id);
        }
    }

    internal class FrameAnimWinHierarchy {
        // Animation window shared state
        private WindowState m_State;
        private TreeViewController m_TreeView;

        public Vector2 GetContentSize() {
            return m_TreeView.GetContentSize();
        }

        public Rect GetTotalRect() {
            return m_TreeView.GetTotalRect();
        }

        public FrameAnimWinHierarchy(WindowState state, EditorWindow owner, Rect position) {
            m_State = state;
            Init(owner, position);
        }

        public void OnGUI(Rect position) {
            m_TreeView.OnEvent();
            m_TreeView.OnGUI(position, GUIUtility.GetControlID(FocusType.Keyboard));
        }

        public void Init(EditorWindow owner, Rect rect) {
            if(m_State.hierarchyState == null)
                m_State.hierarchyState = new FrameAnimWinHierarchyState();

            m_TreeView = new TreeViewController(owner, m_State.hierarchyState);
            //m_State.hierarchyData = new AnimationWindowHierarchyDataSource(m_TreeView, m_State);
            //m_TreeView.Init(rect,
            //    m_State.hierarchyData,
            //    new AnimationWindowHierarchyGUI(m_TreeView, m_State),
            //    null
            //);

            //m_TreeView.deselectOnUnhandledMouseDown = true;
            //m_TreeView.selectionChangedCallback += m_State.OnHierarchySelectionChanged;

            //m_TreeView.ReloadData();
        }

        virtual internal bool IsRenamingNodeAllowed(TreeViewItem node) { return true; }

        public bool IsIDVisible(int id) {
            //if(m_TreeView == null)
            //    return false;

            //var rows = m_TreeView.data.GetRows();
            //return TreeViewController.GetIndexOfID(rows, id) >= 0;
            return default;
        }

        public void EndNameEditing(bool acceptChanges) {
            //m_TreeView.EndNameEditing(acceptChanges);
        }
    }
}
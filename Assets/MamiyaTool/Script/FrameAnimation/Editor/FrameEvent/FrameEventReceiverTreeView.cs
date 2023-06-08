using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace MamiyaTool {
    public class FrameEventReceiverTreeView : TreeView {
        public bool dirty { private get; set; }

        private SerializedProperty EventNames { get; set; }
        private SerializedProperty Events { get; set; }
        private string EventName { get; set; }
        public bool readonlyEvent { get; set; }

        private readonly FrameEventReceiver m_Target;

        private const float k_VerticalPadding = 5;
        private const float k_HorizontalPadding = 5;
        /******************************************************************
         *
         *      lifecycle
         *
         ******************************************************************/
        public FrameEventReceiverTreeView(TreeViewState state, MultiColumnHeader multiColumnHeader, FrameEventReceiver receiver, SerializedObject serializedObject) : base(state, multiColumnHeader) {
            m_Target = receiver;
            useScrollView = true;
            SetSerializedProperties(serializedObject);
            getNewSelectionOverride = (item, selection, shift) => new List<int>();
        }
        protected override TreeViewItem BuildRoot() {
            var root = new TreeViewItem(-1, -1) {
                children = new List<TreeViewItem>()
            };
            var matchingId = EventName != null && readonlyEvent
                ? FindIdForEventName(EventNames, EventName) : -1;
            if(matchingId >= 0)
                AddItem(root, matchingId);

            for(int i = 0; i < EventNames.arraySize; ++i) {
                if(i == matchingId)
                    continue;
                AddItem(root, i, !readonlyEvent);
            }
            return root;
        }
        protected override void RowGUI(RowGUIArgs args) {
            var item = (FrameEventReceiverItem)args.item;
            for(int i = 0; i < args.GetNumVisibleColumns(); ++i) {
                var rect = args.GetCellRect(i);
                rect.y += k_VerticalPadding;
                item.Draw(rect, args.GetColumn(i), args.row, k_HorizontalPadding, m_Target);
            }
        }
        protected override float GetCustomRowHeight(int row, TreeViewItem treeItem) {
            var item = treeItem as FrameEventReceiverItem;
            return item.GetHeight() + k_VerticalPadding;
        }
        /******************************************************************
         *
         *      public method
         *
         ******************************************************************/
        public void Draw() {
            var rect = EditorGUILayout.GetControlRect(true, GetTotalHeight());
            OnGUI(rect);
        }
        public void RefreshIfDirty() {
            var eventNamesListSizeHasChanged = EventNames.arraySize != GetRows().Count;
            if(dirty || eventNamesListSizeHasChanged)
                Reload();
            dirty = false;
        }
        public static MultiColumnHeaderState.Column[] GetColumns() {
            return new[] {
                new MultiColumnHeaderState.Column {
                    headerContent = L10n.TextContent("Key"),
                    contextMenuText = "",
                    headerTextAlignment = TextAlignment.Center,
                    width = 20, minWidth = 20,
                    autoResize = true,
                    allowToggleVisibility = false,
                    canSort = false,
                },
                new MultiColumnHeaderState.Column {
                    headerContent = L10n.TextContent("Event"),
                    contextMenuText = "",
                    headerTextAlignment = TextAlignment.Center,
                    width = 50, minWidth = 120,
                    autoResize = true,
                    allowToggleVisibility = false,
                    canSort = false,
                }
            };
        }
        /******************************************************************
         *
         *      private method
         *
         ******************************************************************/
        private void SetSerializedProperties(SerializedObject serializedObject) {
            EventNames = FrameEventReceiverUtility.FindEventNameProperty(serializedObject);
            Events = FrameEventReceiverUtility.FindEventsProperty(serializedObject);
            Reload();
        }
        private float GetTotalHeight() {
            float height = 0f;
            foreach(var item in GetRows()) {
                var listItem = item as FrameEventReceiverItem;
                height += listItem.GetHeight() + k_VerticalPadding;
            }

            var scrollbarPadding = showingHorizontalScrollBar ? GUI.skin.horizontalScrollbar.fixedHeight : k_VerticalPadding;
            return height + multiColumnHeader.height + scrollbarPadding;
        }
        private void AddItem(TreeViewItem root, int id, bool enabled = true) {
            var evtName = EventNames.GetArrayElementAtIndex(id);
            var evt = Events.GetArrayElementAtIndex(id);
            root.children.Add(new FrameEventReceiverItem(evtName, evt, id, readonlyEvent, enabled, this));
        }
        /******************************************************************
         *
         *      static method
         *
         ******************************************************************/
        private static int FindIdForEventName(SerializedProperty eventNames, string eventToFind) {
            for(int i = 0; i < eventNames.arraySize; ++i) {
                var serlizedProperty = eventNames.GetArrayElementAtIndex(i);
                var evtNameReferenceValue = serlizedProperty.stringValue;
                if(evtNameReferenceValue == eventToFind)
                    return i;
            }
            return -1;
        }
    }
}
using System;
using System.Reflection;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace MamiyaTool {
    public class FrameEventReceiverItem : TreeViewItem {
        private static FrameEventDrawer k_EvtDrawer = new FrameEventDrawer();

        private readonly SerializedProperty m_EvtName;
        private readonly SerializedProperty m_Evt;
        private readonly FrameEventReceiverTreeView m_TreeView;

        private int m_CurrentRowIdx;
        private FrameEventReceiver m_CurrentReceiver;

        private static GUIContent TitleSettingsIcon {
            get {
                if(titleSettingsIcon == null) {
                    Type type = typeof(EditorGUI).GetNestedType("GUIContents", BindingFlags.Static | BindingFlags.NonPublic);
                    FieldInfo field = type.GetField("<titleSettingsIcon>k__BackingField", BindingFlags.Static | BindingFlags.NonPublic);

                    titleSettingsIcon = field.GetValue(null) as GUIContent;
                }
                return titleSettingsIcon;
            }
        }
        private static GUIContent titleSettingsIcon;

        internal readonly bool enabled;
        internal readonly bool readonlyEvent;

        internal const string EventName = "EventName";
        internal const string EventOptions = "EventOptions";
        /******************************************************************
         *
         *      public method
         *
         ******************************************************************/
        public FrameEventReceiverItem(SerializedProperty eventName, SerializedProperty eventListEntry, int id, bool readonlyEvent, bool enabled, FrameEventReceiverTreeView treeView) : base(id, 0) {
            m_EvtName = eventName;
            m_Evt = eventListEntry;
            this.enabled = enabled;
            this.readonlyEvent = readonlyEvent;
            m_TreeView = treeView;
        }
        public float GetHeight() {
            return k_EvtDrawer.GetPropertyHeight(m_Evt, GUIContent.none);
        }
        public void Draw(Rect rect, int colIdx, int rowIdx, float padding, FrameEventReceiver target) {
            switch(colIdx) {
                case 0:
                    DrawEventNameColumn(rect, padding, target, rowIdx);
                    break;
                case 1:
                    DrawEventColumn(rect, rowIdx);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        /******************************************************************
         *
         *      private method
         *
         ******************************************************************/
        private void DrawEventNameColumn(Rect rect, float padding, FrameEventReceiver target, int rowIdx) {
            using(new EditorGUI.DisabledScope(!enabled)) {
                m_CurrentRowIdx = rowIdx;
                m_CurrentReceiver = target;

                var eventName = m_EvtName.stringValue;
                rect.x += padding;
                rect.width -= padding;
                rect.height = EditorGUIUtility.singleLineHeight;
                GUI.SetNextControlName(EventName);
                m_EvtName.stringValue = EditorGUI.TextField(rect, eventName);
            }
        }
        private void DrawEventColumn(Rect rect, int rowIdx) {
            using(new EditorGUI.DisabledScope(!enabled)) {
                if(!readonlyEvent) {
                    var optionButtonSize = GetOptionButtonSize();
                    rect.width -= optionButtonSize.x;

                    var optionButtonRect = new Rect {
                        x = rect.xMax,
                        y = rect.y,
                        width = optionButtonSize.x,
                        height = optionButtonSize.y,
                    };
                    DrawOptionsButton(optionButtonRect, rowIdx, m_CurrentReceiver);
                }

                using(new EditorGUI.DisabledScope(!enabled)) {
                    var nameAsString = m_EvtName.stringValue == null ? "Null" : m_EvtName.stringValue;
                    using(var change = new EditorGUI.ChangeCheckScope()) {
                        EditorGUI.PropertyField(rect, m_Evt, EditorGUIUtility.TrTempContent(nameAsString));
                        if(change.changed)
                            m_TreeView.dirty = true;
                    }
                }
            }
        }
        private void DrawOptionsButton(Rect rect, int rowIdx, FrameEventReceiver target) {
            GUI.SetNextControlName(EventOptions);
            if(EditorGUI.DropdownButton(rect, TitleSettingsIcon, FocusType.Passive, EditorStyles.iconButton)) {
                var menu = new GenericMenu();
                menu.AddItem(new GUIContent(Styles.EventListDuplicateOption), false, () => {
                    Undo.RecordObject(target, Styles.UndoDuplicateRow);
                    var evtCloner = ScriptableObject.CreateInstance<UnityEventCloner>();
                    evtCloner.evt = target.GetEventAtIndex(rowIdx);
                    var clone = Object.Instantiate(evtCloner);
                    target.AddEmptyEvent(clone.evt);
                    m_TreeView.dirty = true;
                    PrefabUtility.RecordPrefabInstancePropertyModifications(target);
                });
                menu.AddItem(new GUIContent(Styles.EventListDeleteOption), false, () => {
                    Undo.RecordObject(target, Styles.UndoDeleteRow);
                    target.RemoveAtIndex(rowIdx);
                    m_TreeView.dirty = true;
                    PrefabUtility.RecordPrefabInstancePropertyModifications(target);
                });
                menu.ShowAsContext();
            }
        }
        /******************************************************************
         *
         *      static method
         *
         ******************************************************************/
        private static Vector2 GetOptionButtonSize() {
            EditorGUIUtility.SetIconSize(Vector2.zero);
            return EditorStyles.iconButton.CalcSize(TitleSettingsIcon);
        }
        /******************************************************************
         *
         *      class define
         *
         ******************************************************************/
        private class UnityEventCloner : ScriptableObject {
            public UnityEvent evt;
        }
    }
}
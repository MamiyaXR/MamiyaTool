using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine.Events;

namespace MamiyaTool {
    [CustomEditor(typeof(FrameEventReceiver))]
    public class FrameEventReceiverInspector : Editor{
        private FrameEventReceiver m_Target;
        [SerializeField] private TreeViewState m_TreeState;
        [SerializeField] private MultiColumnHeaderState m_MultiColumnHeaderState;
        internal FrameEventReceiverTreeView m_TreeView;
        /******************************************************************
         *
         *      lifecycle
         *
         ******************************************************************/
        private void OnEnable() {
            m_Target = target as FrameEventReceiver;
            InitTreeView(serializedObject);
            Undo.undoRedoPerformed += OnUndoRedo;
        }
        private void OnDisable() {
            Undo.undoRedoPerformed -= OnUndoRedo;
        }
        public override void OnInspectorGUI() {
            serializedObject.Update();

            using(var changeCheck = new EditorGUI.ChangeCheckScope()) {
                m_TreeView.RefreshIfDirty();

                EditorGUILayout.Space();
                m_TreeView.Draw();
                DrawAddRemoveButtons();

                if(changeCheck.changed) {
                    serializedObject.ApplyModifiedProperties();
                    m_TreeView.dirty = true;
                }
            }
        }
        /******************************************************************
         *
         *      private method
         *
         ******************************************************************/
        private void OnUndoRedo() {
            m_TreeView.dirty = true;
        }
        private void InitTreeView(SerializedObject so) {
            m_TreeState = FrameEventListFactory.CreateViewState();
            m_MultiColumnHeaderState = FrameEventListFactory.CreateHeaderState();
            var header = FrameEventListFactory.CreateHeader(m_MultiColumnHeaderState, FrameEventReceiverUtility.headerHeight);

            m_TreeView = FrameEventListFactory.CreateFrameInspectorList(m_TreeState, header, m_Target, so);
        }
        private void DrawAddRemoveButtons() {
            using(new GUILayout.HorizontalScope()) {
                GUILayout.FlexibleSpace();
                if(GUILayout.Button(Styles.AddEventButton)) {
                    Undo.RecordObject(m_Target, Styles.UndoAddEvent);
                    m_Target.AddEmptyEvent(new UnityEvent());
                    PrefabUtility.RecordPrefabInstancePropertyModifications(m_Target);
                }
                GUILayout.Space(18f);
            }
        }
    }
}
using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace MamiyaTool {
    public static class FrameEventListFactory {
        public static FrameEventReceiverTreeView CreateFrameInspectorList(TreeViewState state, FrameEventReceiverHeader header, FrameEventReceiver target, SerializedObject so) {
            return new FrameEventReceiverTreeView(state, header, target, so);
        }
        public static FrameEventReceiverHeader CreateHeader(MultiColumnHeaderState state, int columnHeight) {
            var header = new FrameEventReceiverHeader(state) { height = columnHeight };
            header.ResizeToFit();
            return header;
        }
        public static MultiColumnHeaderState CreateHeaderState() {
            return new MultiColumnHeaderState(FrameEventReceiverTreeView.GetColumns());
        }
        public static TreeViewState CreateViewState() {
            return new TreeViewState();
        }
    }
}
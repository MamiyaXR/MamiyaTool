using UnityEditor.IMGUI.Controls;
using UnityEditor;

namespace MamiyaTool {
    public class FrameEventReceiverHeader : MultiColumnHeader {
        public FrameEventReceiverHeader(MultiColumnHeaderState state) : base(state) { }
        protected override void AddColumnHeaderContextMenuItems(GenericMenu menu) {
            menu.AddItem(L10n.TextContent("Resize to Fit"), false, ResizeToFit);
        }
    }
}
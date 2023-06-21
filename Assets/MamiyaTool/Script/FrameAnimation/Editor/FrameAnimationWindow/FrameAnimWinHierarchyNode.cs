using System;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace MamiyaTool {
    internal class FrameAnimWinHierarchyNode : TreeViewItem {
        public string path;
        public Type trackType;

        public EditorCurveBinding? binding;

        public float? topPixel = null;
        public int indent = 0;

        public FrameAnimWinHierarchyNode(int instanceID, int depth, Type trackType, string path, string displayName)
            : base(instanceID, depth, displayName) {
            this.displayName = displayName;
            this.trackType = trackType;
            this.path = path;
        }
    }
}
using System;
using System.Reflection;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace MamiyaTool {
    internal class TreeViewController {
        #region reflection info
        static BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        static Lazy<Type> treeViewCtrlType = new Lazy<Type>(() => {
            var type = typeof(EditorWindow).Assembly.GetTypes().First(x => x.FullName == "UnityEditor.IMGUI.Controls.TreeViewController");
            return type;
        });
        static Lazy<ConstructorInfo> ctor = new Lazy<ConstructorInfo>(() => {
            var type = treeViewCtrlType.Value;
            return type.GetConstructor(flags, null, new Type[] { typeof(EditorWindow), typeof(TreeViewState) }, null);
        });
        static Lazy<MethodInfo> getContentSize = new Lazy<MethodInfo>(() => {
            var type = treeViewCtrlType.Value;
            return type.GetMethod("GetContentSize", flags, null, Type.EmptyTypes, null);
        });
        static Lazy<MethodInfo> getTotalRect = new Lazy<MethodInfo>(() => {
            var type = treeViewCtrlType.Value;
            return type.GetMethod("GetTotalRect", flags, null, Type.EmptyTypes, null);
        });
        static Lazy<MethodInfo> setTotalRect = new Lazy<MethodInfo>(() => {
            var type = treeViewCtrlType.Value;
            return type.GetMethod("SetTotalRect", flags, null, new Type[] { typeof(Rect) }, null);
        });
        static Lazy<MethodInfo> onEvent = new Lazy<MethodInfo>(() => {
            var type = treeViewCtrlType.Value;
            return type.GetMethod("OnEvent", flags, null, Type.EmptyTypes, null);
        });
        static Lazy<MethodInfo> onGUI = new Lazy<MethodInfo>(() => {
            var type = treeViewCtrlType.Value;
            return type.GetMethod("OnGUI", flags, null, new Type[] { typeof(Rect), typeof(int) }, null);
        });
        #endregion

        public object Value => value;
        private object value;

        public TreeViewController(EditorWindow editorWindow, TreeViewState treeViewState) {
            value = ctor.Value.Invoke(new object[] { editorWindow, treeViewState });
        }
        public Vector2 GetContentSize() {
            return (Vector2)(getContentSize.Value.Invoke(value, null));
        }
        public Rect GetTotalRect() {
            return (Rect)(getTotalRect.Value.Invoke(value, null));
        }
        public void SetTotalRect(Rect rect) {
            setTotalRect.Value.Invoke(value, new object[] { rect });
        }
        public void OnEvent() {
            onEvent.Value.Invoke(value, null);
        }
        public void OnGUI(Rect rect, int keyboardControlID) {
            if(value != null)
                onGUI.Value.Invoke(value, new object[] { rect, keyboardControlID });
        }
    }
}
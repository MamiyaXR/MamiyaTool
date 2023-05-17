using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace MamiyaTool {
    [CustomPropertyDrawer(typeof(FrameTrackBase))]
    public class FrameTrackDrawer : PropertyDrawer {
        private static Type[] types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .Where(x => x.IsSubclassOf(typeof(FrameTrackBase)) && !x.IsAbstract)
            .ToArray();
        private static string[] typenames = types.Select(x => x.Name).ToArray();

        private static GUIContent cempty = new GUIContent(string.Empty);

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginProperty(position, label, property);
            var type = property.managedReferenceValue?.GetType();
            // 获取当前类型的索引
            var index = Array.IndexOf(types, type);
            // 获取下拉框的位置
            var rect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            // 绘制下拉框
            index = EditorGUI.Popup(rect, property.displayName, index, typenames);
            // 设置当前类型
            if(index >= 0) {
                var serType = types[index];
                if(property.managedReferenceValue?.GetType() != serType) {
                    property.managedReferenceValue = Activator.CreateInstance(serType);
                }
            }
            EditorGUI.PropertyField(position, property, cempty, true);
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return EditorGUI.GetPropertyHeight(property, true);
        }
    }
}
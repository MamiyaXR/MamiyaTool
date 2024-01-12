using System;
using System.Reflection;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace MamiyaTool {
    internal static class Extensions {
        static BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
        static Lazy<Type> editorGUIType = new Lazy<Type>(() => {
            var type = typeof(EditorWindow).Assembly.GetTypes().First(x => x.FullName == "UnityEditor.EditorGUI");
            return type;
        });
        static Lazy<FieldInfo> kWindowToolbarHeightInfo = new Lazy<FieldInfo>(() => {
            var type = editorGUIType.Value;
            return type.GetField("kWindowToolbarHeight", flags);
        });
        static Lazy<Type> guiContentType = new Lazy<Type>(() => {
            var type = typeof(GUIContent);
            return type;
        });
        static Lazy<MethodInfo> tempInfo = new Lazy<MethodInfo>(() => {
            var type = guiContentType.Value;
            return type.GetMethod("Temp", flags, null, new Type[] { typeof(string) }, null);
        });

        public static Color RGBMultiplied(this Color self, float multiplier) {
            return new Color(self.r * multiplier, self.g * multiplier, self.b * multiplier, self.a);
        }
        public static Color AlphaMultiplied(this Color self, float multiplier) {
            return new Color(self.r, self.g, self.b, self.a * multiplier);
        }
        public static GUIContent Temp(string t) {
            return tempInfo.Value.Invoke(null, new object[] { t }) as GUIContent;
        }
        public static float kWindowToolbarHeight {
            get {
                object obj = kWindowToolbarHeightInfo.Value.GetValue(null);
                Type type = obj.GetType();
                PropertyInfo info = type.GetProperty("value", flags);
                MethodInfo method = info.GetGetMethod();
                return (float)(method.Invoke(obj, null));
            }
        }
    }
}
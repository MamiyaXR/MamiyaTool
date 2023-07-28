using System;
using System.Reflection;

namespace MamiyaTool {
    public static class ReflectionUtility {
        private static BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;
        public static object InvokeStaticMethod(Type type, string methodName, Type[] types = null, object[] args = null) {
            types = types == null ? Type.EmptyTypes : types;
            MethodInfo method = type.GetMethod(methodName, flags, null, types, null);
            return method.Invoke(null, args);
        }
        public static object InvokeStaticMethod<T>(string methodName, Type[] types = null, object[] args = null) {
            Type type = typeof(T);
            return InvokeStaticMethod(type, methodName, types, args);
        }
        public static object InvokeMethod(Type type, string methodName, object obj, Type[] types = null, object[] args = null) {
            types = types == null ? Type.EmptyTypes : types;
            MethodInfo method = type.GetMethod(methodName, flags, null, types, null);
            return method.Invoke(obj, args);
        }
        public static object InvokeMethod<T>(string methodName, object obj, Type[] types = null, object[] args = null) {
            Type type = typeof(T);
            return InvokeMethod(type, methodName, obj, types, args);
        }
        public static object GetField(Type type, string fieldName, object obj) {
            FieldInfo field = type.GetField(fieldName, flags);
            return field.GetValue(obj);
        }
        public static object GetField<T>(string fieldName, T obj) {
            Type type = typeof(T);
            return GetField(type, fieldName, obj);
        }
        public static void SetField(Type type, string fieldName, object obj, object value) {
            FieldInfo field = type.GetField(fieldName, flags);
            field.SetValue(obj, value);
        }
        public static void SetField<T>(string fieldName, T obj, object value) {
            Type type = typeof(T);
            SetField(type, fieldName, obj, value);
        }
    }
}
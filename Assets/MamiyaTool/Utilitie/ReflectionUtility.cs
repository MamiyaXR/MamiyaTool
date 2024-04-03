using System;
using System.Reflection;

namespace MamiyaTool {
    public static class ReflectionUtility {
        private const BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;
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
        public static FieldInfo GetFieldInTree(this Type type, string fieldName, BindingFlags bindingAttr = flags) {
            var field = type.GetField(fieldName, bindingAttr);
            if(field != null)
                return field;
            if(type.BaseType != null)
                return GetFieldInTree(type.BaseType, fieldName, bindingAttr);
            return null;
        }
        public static PropertyInfo GetPropertyInTree(this Type type, string propertyName, BindingFlags bindingAttr = flags) {
            var property = type.GetProperty(propertyName, bindingAttr);
            if(property != null)
                return property;
            if(type.BaseType != null)
                return GetPropertyInTree(type.BaseType, propertyName, bindingAttr);
            return null;
        }
        public static MethodInfo GetMethodInTree(this Type type, string methodName, BindingFlags bindingAttr = flags) {
            var method = type.GetMethod(methodName, bindingAttr);
            if(method != null)
                return method;
            if(type.BaseType != null)
                return GetMethodInTree(type.BaseType, methodName, bindingAttr);
            return null;
        }
        public static MethodInfo GetMethodInTree(string typeFullName, string methodName, BindingFlags bindingAttr = flags) {
            var type = Type.GetType(typeFullName);
            if(type == null)
                return null;
            return GetMethodInTree(type, methodName, bindingAttr);
        }
    }
}
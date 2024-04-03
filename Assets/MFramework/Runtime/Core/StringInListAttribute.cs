using System;
using System.Reflection;
using UnityEngine;

namespace MFramework {
    public class StringInListAttribute : PropertyAttribute {
        public delegate string[] GetStringList();

        private string _methodOrPropertyOrField;
        private string[] _list;
        private MethodInfo _method;
        private PropertyInfo _property;
        private FieldInfo _field;

        private static Type _tnullable = typeof(Nullable);
        private static BindingFlags _bindingFlags = BindingFlags.Instance | BindingFlags.Public
                                                    | BindingFlags.NonPublic | BindingFlags.Static;
        /******************************************************************
         *
         *      lifecycle
         *
         ******************************************************************/
        public StringInListAttribute(params string[] list) {
            _list = list;
        }
        public StringInListAttribute(Type type, string methodOrPropertyOrField) {
            _methodOrPropertyOrField = methodOrPropertyOrField;
            if(type == null || type == _tnullable)
                return;
            _method = type.GetMethod(methodOrPropertyOrField, _bindingFlags);

            if(_method == null)
                _property = type.GetProperty(methodOrPropertyOrField, _bindingFlags);
            if(_property == null)
                _field = type.GetField(methodOrPropertyOrField, _bindingFlags);
            if(_method == null && _property == null && _field == null)
                Debug.LogError($"No such method or property:{methodOrPropertyOrField} for type:{type}");
        }
        /******************************************************************
         *
         *      public method
         *
         ******************************************************************/
        public string[] GetList(object obj) {
            if(_list != null)
                return _list;

            string[] r = null;
            if(_method != null)
                r = _method.Invoke(obj, null) as string[];
            if(r == null && _property != null)
                r = _property.GetValue(obj, null) as string[];
            if(r == null && _field != null)
                r = _field.GetValue(obj) as string[];
            if(r == null && obj is Component c) {
                var cps = c.GetComponents<Component>();
                for(int i = cps.Length - 1; i >= 0; --i) {
                    var cp = cps[i];
                    r = GetListInner(cp);
                    if(r != null)
                        break;
                }
            }
            return r;
        }
        /******************************************************************
         *
         *      private method
         *
         ******************************************************************/
        private string[] GetListInner(Component c) {
            if(c == null)
                return null;
            var type = c.GetType();
            var method = type.GetMethod(_methodOrPropertyOrField, _bindingFlags);
            if(method != null)
                return method.Invoke(c, null) as string[];
            var property = type.GetProperty(_methodOrPropertyOrField, _bindingFlags);
            if(property != null)
                return property.GetValue(c, null) as string[];
            var field = type.GetField(_methodOrPropertyOrField, _bindingFlags);
            if(field != null)
                return field.GetValue(c) as string[];
            return null;
        }
    }
}
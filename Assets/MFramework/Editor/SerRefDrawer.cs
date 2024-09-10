using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace MFramework {
    public class SerRefDrawer<T> : PropertyDrawer {
        private static Type _tTarget = typeof(T);
        private static IEnumerable<Type> _allTypes = null;
        private static Type[] _validTypes = null;
        private static string[] _validTypeNames = null;

        private static readonly string[] ExcludePlugins = {
            "MTE.dll",
            "MET_Script.dll",
        };

        private static IEnumerable<Assembly> GetAllAssembly() {
            var r = AppDomain.CurrentDomain.GetAssemblies();
            return r.Where(x => !ExcludePlugins.Contains(x.ManifestModule.Name));
        }
        private static IEnumerable<Type> GetAllTypes() => _allTypes ??= GetAllAssembly().SelectMany(x => x.GetTypes());
        private static bool CheckType(Type x) => _tTarget.IsAssignableFrom(x)
            && x != _tTarget
            && !x.IsAbstract
            && !x.ContainsGenericParameters;

        private static string GetMenuPath(Type x) {
            var attr = x.GetCustomAttribute<RefCompMenuAttribute>();
            return attr?.MenuPath ?? x.Name;
        }
        private static string[] NoneArray = new string[] { "(None)", "" };
        public static Type[] ValidTypes => _validTypes ??= GetAllTypes()
            .Where(x => CheckType(x))
            .OrderBy(x => GetMenuPath(x))
            .ToArray();
        public static readonly string[] ValidTypeNames = _validTypeNames ??= NoneArray.Concat(ValidTypes.Select(x => GetMenuPath(x))).ToArray();
        protected static readonly GUIContent CEmpty = new(string.Empty);

        protected void DrawProperty(Rect position, SerializedProperty property, GUIContent label, Type[] types, string[] typeNames) {
            if(property.propertyType != SerializedPropertyType.ManagedReference) {
                EditorGUI.PropertyField(position, property, label, true);
                return;
            }
            EditorGUI.BeginProperty(position, label, property);
            var type = property.managedReferenceValue?.GetType() ?? null;
            var index = Array.IndexOf(types, type);
            var rect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            index = EditorGUI.Popup(rect, property.displayName, index >= 0 ? index + 2 : 0, typeNames);
            if(index > 1) {
                var serType = types[index - 2];
                if(property.managedReferenceValue?.GetType() != serType) {
                    property.managedReferenceValue = Activator.CreateInstance(serType);
                }
            } else {
                property.managedReferenceValue = null;
            }
            EditorGUI.PropertyField(position, property, CEmpty, true);
            EditorGUI.EndProperty();
        }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            DrawProperty(position, property, label, ValidTypes, ValidTypeNames);
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return EditorGUI.GetPropertyHeight(property, true);
        }
    }
}
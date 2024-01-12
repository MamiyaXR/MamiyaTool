using System;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;

namespace MamiyaTool {
    internal class SplitterState {
        static BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        static Lazy<Type> splitterStateType = new Lazy<Type>(() => {
            var type = typeof(EditorWindow).Assembly.GetTypes().First(x => x.FullName == "UnityEditor.SplitterState");
            return type;
        });

        static Lazy<MethodInfo> fromRelative = new Lazy<MethodInfo>(() => {
            var type = splitterStateType.Value;
            return type.GetMethod("FromRelative", flags, null, new Type[] { typeof(float[]), typeof(float[]), typeof(float[]) }, null);
        });

        static Lazy<FieldInfo> m_RealSizes = new Lazy<FieldInfo>(() => {
            var type = splitterStateType.Value;
            return type.GetField("realSizes", flags);
        });

        public object Value => value;
        private object value;

        private SplitterState() { }

        public static SplitterState FromRelative(float[] relativeSizes, float[] minSizes, float[] maxSizes) {
            SplitterState result = new SplitterState();
            result.value = fromRelative.Value.Invoke(null, new object[] { relativeSizes, minSizes, maxSizes });
            return result;
        }

        public float[] realSizes {
            get {
                return m_RealSizes.Value.GetValue(value) as float[];
            }
        }
    }
}
using UnityEngine;

namespace MFramework {
    public class FieldChangeAttribute : PropertyAttribute {
        public string MethodName { get; private set; }
        public FieldChangeAttribute(string methodName) {
            MethodName = methodName;
        }
    }
}
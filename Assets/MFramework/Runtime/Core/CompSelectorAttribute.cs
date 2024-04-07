using UnityEngine;

namespace MFramework {
    public class CompSelectorAttribute : PropertyAttribute {
        public System.Type[] types { get; private set; }
        public CompSelectorAttribute() { }
        public CompSelectorAttribute(params System.Type[] types) {
            this.types = types;
        }
    }
}
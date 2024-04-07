using System;

namespace MFramework {
    public class RefCompMenuAttribute : Attribute {
        public string MenuPath { get; private set; }
        public RefCompMenuAttribute(string menuPath) {
            MenuPath = menuPath;
        }
    }
}
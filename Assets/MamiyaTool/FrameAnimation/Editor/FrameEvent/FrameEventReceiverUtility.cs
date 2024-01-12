using UnityEditor;

namespace MamiyaTool {
    public class FrameEventReceiverUtility {
        private const int k_DefaultTreeviewHeaderHeight = 20;
        public static int headerHeight {
            get { return k_DefaultTreeviewHeaderHeight; }
        }
        public static SerializedProperty FindEventNameProperty(SerializedObject obj) {
            return obj.FindProperty("events.eventNames");
        }
        public static SerializedProperty FindEventsProperty(SerializedObject obj) {
            return obj.FindProperty("events.events");
        }
    }
}
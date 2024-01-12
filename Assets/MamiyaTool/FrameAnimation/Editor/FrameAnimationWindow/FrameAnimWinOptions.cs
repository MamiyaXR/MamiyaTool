using UnityEditor;

namespace MamiyaTool {
    internal static class FrameAnimWinOptions {
        static string kTimeFormat = "FrameAnimWin.TimeFormat";
        static string kFilterBySelection = "FrameAnimWin.FilterBySelection";
        static string kShowReadOnly = "FrameAnimWin.ShowReadOnly";
        static string kShowFrameRate = "FrameAnimWin.ShowFrameRate";

        private static TimeArea.TimeFormat m_TimeFormat;
        private static bool m_FilterBySelection;
        private static bool m_ShowReadOnly;
        private static bool m_ShowFrameRate;

        static FrameAnimWinOptions() {
            m_TimeFormat = (TimeArea.TimeFormat)EditorPrefs.GetInt(kTimeFormat, (int)TimeArea.TimeFormat.Frame);
            m_FilterBySelection = EditorPrefs.GetBool(kFilterBySelection, false);
            m_ShowReadOnly = EditorPrefs.GetBool(kShowReadOnly, false);
            m_ShowFrameRate = EditorPrefs.GetBool(kShowFrameRate, false);
        }

        public static TimeArea.TimeFormat timeFormat {
            get {
                return m_TimeFormat;
            }
            set {
                m_TimeFormat = value;
                EditorPrefs.SetInt(kTimeFormat, (int)value);
            }
        }

        public static bool filterBySelection {
            get {
                return m_FilterBySelection;
            }
            set {
                m_FilterBySelection = value;
                EditorPrefs.SetBool(kFilterBySelection, value);
            }
        }

        public static bool showReadOnly {
            get {
                return m_ShowReadOnly;
            }
            set {
                m_ShowReadOnly = value;
                EditorPrefs.SetBool(kShowReadOnly, value);
            }
        }

        public static bool showFrameRate {
            get {
                return m_ShowFrameRate;
            }
            set {
                m_ShowFrameRate = value;
                EditorPrefs.SetBool(kShowFrameRate, value);
            }
        }
    }
}
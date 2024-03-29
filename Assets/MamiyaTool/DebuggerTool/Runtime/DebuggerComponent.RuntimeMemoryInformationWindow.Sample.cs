﻿using UnityEngine;

namespace MamiyaTool {
    public sealed partial class DebuggerComponent : MonoBehaviour {
        private sealed partial class RuntimeMemoryInformationWindow<T> : ScrollableDebuggerWindowBase where T : Object {
            private sealed class Sample {
                private readonly string m_Name;
                private readonly string m_Type;
                private readonly long m_Size;
                private bool m_Highlight;

                public Sample(string name, string type, long size) {
                    m_Name = name;
                    m_Type = type;
                    m_Size = size;
                    m_Highlight = false;
                }

                public string Name => m_Name;
                public string Type => m_Type;
                public long Size => m_Size;
                public bool Highlight { get => m_Highlight; set => m_Highlight = value; }
            }
        }
    }
}

using System;
using UnityEngine;

namespace MamiyaTool
{
    public sealed partial class DebuggerComponent : MonoBehaviour
    {
        private sealed class PathInformationWindow : ScrollableDebuggerWindowBase
        {
            protected override void OnDrawScrollableWindow()
            {
                GUILayout.Label("<b>Path Information</b>");
                GUILayout.BeginVertical("box");
                {
                    DrawItem("Current Directory", GetRegularPath(Environment.CurrentDirectory));
                    DrawItem("Data Path", GetRegularPath(Application.dataPath));
                    DrawItem("Persistent Data Path", GetRegularPath(Application.persistentDataPath));
                    DrawItem("Streaming Assets Path", GetRegularPath(Application.streamingAssetsPath));
                    DrawItem("Temporary Cache Path", GetRegularPath(Application.temporaryCachePath));
#if UNITY_2018_3_OR_NEWER
                    DrawItem("Console Log Path", GetRegularPath(Application.consoleLogPath));
#endif
                }
                GUILayout.EndVertical();
            }
        }
    }
}

using UnityEditor;
using UnityEngine;

namespace MamiyaTool {
    internal static class Styles {
        public static readonly GUIContent RetroactiveLabel = L10n.TextContent("Retroactive", "Use retroactive to emit this frame even if playback starts afterwards.");
        public static readonly GUIContent EmitOnceLabel = L10n.TextContent("Emit Once", "Emit the event once during loops.");
        public static readonly GUIContent EmitFrameEventLabel = L10n.TextContent("Emit FrameEvent", "Select which Event Name to emit.");
        public static readonly GUIContent ObjectLabel = L10n.TextContent("Receiver Component on", "The FrameEvent Receiver Component on the bound GameObject.");

        public static readonly GUIContent CreateNewFrameEvent = L10n.TextContent("Create FrameEvent...");
        public static readonly GUIContent AddFrameEventReceiverComponent = L10n.TextContent("Add FrameEvent Receiver", "Creates a FrameEvent Receiver component on the track binding and the reaction for the current signal.");
        public static readonly GUIContent EmptyEventList = L10n.TextContent("None");
        public static readonly GUIContent AddEventButton = L10n.TextContent("Add Event");

        public static readonly string EventListDuplicateOption = L10n.Tr("Duplicate");
        public static readonly string EventListDeleteOption = L10n.Tr("Delete");
        public static readonly string NoBoundGO = L10n.Tr("Track has no bound GameObject.");
        public static readonly string MultiEditNotSupportedOnDifferentBindings = L10n.Tr("Multi-edit not supported for FrameEventReceivers on tracks bound to different GameObjects.");
        public static readonly string MultiEditNotSupportedOnDifferentSignals = L10n.Tr("Multi-edit not supported for FrameEventReceivers when FrameEventEmitters use different Events.");

        public static readonly string UndoDuplicateRow = L10n.Tr("Duplicate Row");
        public static readonly string UndoDeleteRow = L10n.Tr("Delete Row");
        public static readonly string UndoAddEvent = L10n.Tr("Add FrameEvent Receiver Reaction");
        public static readonly string NoReaction = L10n.Tr("No reaction for {0} has been defined in this receiver");
        public static readonly string NoSignalReceiverComponent = L10n.Tr("There is no FrameEvent Receiver component on {0}");
    }
}
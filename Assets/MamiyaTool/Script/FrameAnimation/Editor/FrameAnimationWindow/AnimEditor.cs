using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MamiyaTool {
    internal class AnimEditor {
        #region private field
        private SplitterState m_HorizontalSplitter;
        private WindowState m_State;
        private TimeArea m_DopeSheet;
        private Rect m_Position;
        private bool m_TriggerFraming;
        private bool m_Initialized;
        private EditorWindow m_OwnerWindow;
        private AnimEditorOverlay m_Overlay;
        private FrameAnimWinHierarchy m_Hierarchy;
        #endregion

        #region accessor
        public bool stateDisabled { get { return m_State.disabled; } }
        public WindowState state => m_State;
        public FrameAnimWinSelectionItem selection => m_State.selection;
        private float hierarchyWidth { get { return m_HorizontalSplitter.realSizes[0]; } }
        private float contentWidth { get { return m_HorizontalSplitter.realSizes[1]; } }
        private int layoutRowHeight { get { return (int)Extensions.kWindowToolbarHeight; } }
        private bool triggerFraming {
            get { return m_TriggerFraming; }
            set { m_TriggerFraming = value; }
        }
        #endregion

        #region color define
        static private Color s_FilterBySelectionColorLight = new Color(0.82f, 0.97f, 1.00f, 1.00f);
        static private Color s_FilterBySelectionColorDark = new Color(0.54f, 0.85f, 1.00f, 1.00f);
        static private Color filterBySelectionColor {
            get {
                return EditorGUIUtility.isProSkin ? s_FilterBySelectionColorDark : s_FilterBySelectionColorLight;
            }
        }
        static private Color s_InRangeColorLight = new Color32(211, 211, 211, 255);
        static private Color s_InRangeColorDark = new Color32(75, 75, 75, 255);
        static private Color inRangeColor {
            get {
                return EditorGUIUtility.isProSkin ? s_InRangeColorDark : s_InRangeColorLight;
            }
        }
        static private Color s_OutOfRangeColorLight = new Color32(160, 160, 160, 127);
        static private Color s_OutOfRangeColorDark = new Color32(40, 40, 40, 127);

        static private Color outOfRangeColor {
            get {
                return EditorGUIUtility.isProSkin ? s_OutOfRangeColorDark : s_OutOfRangeColorLight;
            }
        }
        #endregion

        #region const define
        internal const int kSliderThickness = 13;
        internal const int kIntFieldWidth = 35;
        internal const int kHierarchyMinWidth = 300;
        internal const int kToggleButtonWidth = 80;
        internal const float kDisabledRulerAlpha = 0.12f;
        #endregion
        /*****************************************************************
         * 
         *      lifecycle
         * 
         *****************************************************************/
        public AnimEditor() {
            m_State = new WindowState();
            m_State.animEditor = this;
            InitializeHorizontalSplitter();
            InitializeDopeSheet();
            InitializeOverlay();

            m_State.timeArea = m_DopeSheet;
            m_DopeSheet.state = m_State;
            m_Overlay.state = m_State;
        }
        public void Update() {
            if(m_State == null)
                return;

            PlaybackUpdate();
        }
        /*****************************************************************
         * 
         *      public method
         * 
         *****************************************************************/
        public void OnAnimEditorGUI(EditorWindow parent, Rect position) {
            m_DopeSheet.m_Owner = parent;
            m_OwnerWindow = parent;
            m_Position = position;

            if(!m_Initialized)
                Initialize();

            if(m_State.disabled && m_State.recording)
                m_State.recording = false;

            SynchronizeLayout();

            using(new EditorGUI.DisabledScope(m_State.disabled)) {
                OverlayEventOnGUI();

                GUILayout.BeginHorizontal();
                SplitterGUILayout.BeginHorizontalSplit(m_HorizontalSplitter.Value);

                #region left side
                GUILayout.BeginVertical();
                // first row of controls
                GUILayout.BeginHorizontal(DirectorStyles.animPlayToolBar);
                PlayControlsOnGUI();
                GUILayout.EndHorizontal();
                // second row of controls
                GUILayout.BeginHorizontal(DirectorStyles.animClipToolBar);
                GUILayout.FlexibleSpace();
                FrameRateInputFieldOnGUI();
                AddKeyframeButtonOnGUI();
                AddEventButtonOnGUI();
                GUILayout.EndHorizontal();
                HierarchyOnGUI();
                GUILayout.EndVertical();
                #endregion

                #region right side
                GUILayout.BeginVertical();
                // Acquire Rects
                Rect timerulerRect = GUILayoutUtility.GetRect(contentWidth, layoutRowHeight);
                Rect eventsRect = GUILayoutUtility.GetRect(contentWidth, layoutRowHeight - 1);
                Rect contentLayoutRect = GUILayoutUtility.GetRect(contentWidth, contentWidth, 0f, float.MaxValue, GUILayout.ExpandHeight(true));
                // MainContent must be done first since it resizes the Zoomable area.
                MainContentOnGUI(contentLayoutRect);
                TimeRulerOnGUI(timerulerRect);
                GUILayout.EndVertical();
                #endregion

                SplitterGUILayout.EndHorizontalSplit();
                GUILayout.EndHorizontal();

                // Overlay
                OverlayOnGUI(contentLayoutRect);
            }
        }
        public void Repaint() {
            if(m_OwnerWindow != null)
                m_OwnerWindow.Repaint();
        }

        #region event
        public void OnSelectionChanged() {
            triggerFraming = true;
            Repaint();
        }
        #endregion
        /*****************************************************************
         * 
         *      private method
         * 
         *****************************************************************/
        private void SynchronizeLayout() {
            m_HorizontalSplitter.realSizes[1] = (int)Mathf.Max(Mathf.Min(m_Position.width - m_HorizontalSplitter.realSizes[0], m_HorizontalSplitter.realSizes[1]), 0);

            if(selection.animationAsset != null) {
                m_State.frameRate = selection.animationAsset.SampleRate;
            } else {
                m_State.frameRate = WindowState.kDefaultFrameRate;
            }
        }
        private void OverlayEventOnGUI() {
            if(!m_State.animatorIsOptimized)
                return;
            if(m_State.disabled)
                return;
            Rect overlayRect = new Rect(hierarchyWidth - 1, 0f, contentWidth - kSliderThickness, m_Position.height - kSliderThickness);
            GUI.BeginGroup(overlayRect);
            m_Overlay.HandleEvents();
            GUI.EndGroup();
        }
        private void PlaybackUpdate() {
            if(m_State.disabled && m_State.playing)
                m_State.playing = false;

            if(m_State.PlaybackUpdate())
                Repaint();
        }

        #region init
        private void Initialize() {
            DirectorStyles.Initialize();
            InitializeHierarchy();

            // The rect here is only for initialization and will be overriden at layout
            m_HorizontalSplitter.realSizes[0] = kHierarchyMinWidth;
            m_HorizontalSplitter.realSizes[1] = (int)Mathf.Max(m_Position.width - kHierarchyMinWidth, kHierarchyMinWidth);
            m_DopeSheet.rect = new Rect(0, 0, contentWidth, 100);

            m_Initialized = true;
        }
        private void InitializeHorizontalSplitter() {
            if(m_HorizontalSplitter == null) {
                m_HorizontalSplitter = SplitterState.FromRelative(new float[] { kHierarchyMinWidth, kHierarchyMinWidth * 3 }, new float[] { kHierarchyMinWidth, kHierarchyMinWidth }, null);
                m_HorizontalSplitter.realSizes[0] = kHierarchyMinWidth;
                m_HorizontalSplitter.realSizes[1] = kHierarchyMinWidth;
            }
        }
        private void InitializeDopeSheet() {
            m_DopeSheet = new TimeArea(m_OwnerWindow);
            m_DopeSheet.SetTickMarkerRanges();
            m_DopeSheet.hSlider = true;
            m_DopeSheet.shownArea = new Rect(1, 1, 1, 1);
            // The rect here is only for initialization and will be overriden at layout
            m_DopeSheet.rect = new Rect(0, 0, contentWidth, 100);
            m_DopeSheet.SetTickModulosForFrameRate(m_State.frameRate);
        }
        private void InitializeOverlay() {
            m_Overlay = new AnimEditorOverlay();
        }
        private void InitializeHierarchy() {
            m_Hierarchy = new FrameAnimWinHierarchy(m_State, m_OwnerWindow, new Rect(0, 0, hierarchyWidth, 100));
        }
        #endregion

        #region play contols
        private void PlayControlsOnGUI() {
            using(new EditorGUI.DisabledScope(!m_State.canPreview)) {
                PreviewButtonOnGUI();
            }

            using(new EditorGUI.DisabledScope(!m_State.canRecord)) {
                RecordButtonOnGUI();
            }

            if(GUILayout.Button(DirectorStyles.firstKeyContent, EditorStyles.toolbarButton)) {
                // todo
            }
            if(GUILayout.Button(DirectorStyles.prevKeyContent, EditorStyles.toolbarButton)) {
                // todo
            }

            using(new EditorGUI.DisabledScope(!m_State.canPlay)) {
                PlayButtonOnGUI();
            }

            if(GUILayout.Button(DirectorStyles.nextKeyContent, EditorStyles.toolbarButton)) {
                // todo
            }
            if(GUILayout.Button(DirectorStyles.lastKeyContent, EditorStyles.toolbarButton)) {
                // todo
            }

            GUILayout.FlexibleSpace();

            EditorGUI.BeginChangeCheck();
            int newFrame = EditorGUILayout.DelayedIntField(m_State.currentFrame, EditorStyles.toolbarTextField, GUILayout.Width(kIntFieldWidth));
            if(EditorGUI.EndChangeCheck()) {
                m_State.currentFrame = newFrame;
            }
        }
        private void PreviewButtonOnGUI() {
            EditorGUI.BeginChangeCheck();
            bool previewingEnable = GUILayout.Toggle(m_State.previewing, DirectorStyles.previewContent, EditorStyles.toolbarButton);
            if(EditorGUI.EndChangeCheck()) {
                m_State.previewing = previewingEnable;
            }
        }
        private void RecordButtonOnGUI() {
            EditorGUI.BeginChangeCheck();

            Color backupColor = GUI.color;
            if(m_State.recording) {
                Color recordedColor = Color.red;
                recordedColor.a *= GUI.color.a;
                GUI.color = recordedColor;
            }
            bool recordingEnabled = GUILayout.Toggle(m_State.recording, DirectorStyles.recordContent, EditorStyles.toolbarButton);
            if(EditorGUI.EndChangeCheck()) {
                m_State.recording = recordingEnabled;
            }
            GUI.color = backupColor;
        }
        private void PlayButtonOnGUI() {
            EditorGUI.BeginChangeCheck();
            bool playbackEnable = GUILayout.Toggle(m_State.playing, DirectorStyles.playContent, EditorStyles.toolbarButton);
            if(EditorGUI.EndChangeCheck()) {
                m_State.playing = playbackEnable;
            }
        }
        private void FrameRateInputFieldOnGUI() {
            using(new EditorGUI.DisabledScope(!selection.animationIsEditable)) {
                GUILayout.Label(DirectorStyles.samples);
                EditorGUI.BeginChangeCheck();
                int clipFrameRate = EditorGUILayout.DelayedIntField(m_State.ClipFrameRate, EditorStyles.toolbarTextField, GUILayout.Width(kIntFieldWidth));
                if(EditorGUI.EndChangeCheck()) {
                    m_State.ClipFrameRate = clipFrameRate;
                }
            }
        }
        private void AddKeyframeButtonOnGUI() {
            bool canAddKey = selection.animationIsEditable;
            using(new EditorGUI.DisabledScope(!canAddKey)) {
                if(GUILayout.Button(DirectorStyles.addKeyframeContent, DirectorStyles.animClipToolbarButton)) {
                    // todo
                    EditorGUIUtility.ExitGUI();
                }
            }
        }
        private void AddEventButtonOnGUI() {
            using(new EditorGUI.DisabledScope(!selection.animationIsEditable)) {
                if(GUILayout.Button(DirectorStyles.addEventContent, DirectorStyles.animClipToolbarButton)) {
                    // todo
                }
            }
        }
        private void HierarchyOnGUI() {
            FrameAnimationAsset asset = m_State.selection.animationAsset;
            bool dirty = false;
            if(asset != null) {
                var obj = new SerializedObject(asset);
                var tracksProperty = obj.FindProperty("tracks");

                const float toggleWith = 18f;
                const float btnWidth = 20f;

                if(tracksProperty != null && tracksProperty.arraySize > 0) {
                    for(int i = 0; i < tracksProperty.arraySize; ++i) {
                        bool breakFlag = false;
                        var trackProperty = tracksProperty.GetArrayElementAtIndex(i);

                        Rect propertyPos = GUILayoutUtility.GetRect(0f, float.MaxValue, 0f, layoutRowHeight);
                        Rect enablePos = new Rect(propertyPos.x, propertyPos.y, toggleWith, propertyPos.height);
                        Rect minusBtnPos = new Rect(propertyPos.xMax - btnWidth, propertyPos.y, btnWidth, propertyPos.height);
                        float mainWidth = (propertyPos.width - toggleWith - btnWidth) / 2f - 2f;
                        Rect popupPos = new Rect(enablePos.xMax, propertyPos.y, mainWidth, propertyPos.height);
                        Rect pathPos = new Rect(popupPos.xMax + 2f, propertyPos.y + 1f, mainWidth, propertyPos.height);

                        EditorGUI.BeginProperty(propertyPos, GUIContent.none, trackProperty);

                        // enable button
                        var enableProperty = trackProperty.FindPropertyRelative("enable");
                        if(enableProperty != null) {
                            EditorGUI.BeginChangeCheck();
                            bool trackEnable = GUI.Toggle(enablePos, enableProperty.boolValue, "", DirectorStyles.trackEnableToggle);
                            if(EditorGUI.EndChangeCheck()) {
                                enableProperty.boolValue = trackEnable;
                                dirty = true;
                            }
                        }

                        // track type
                        var type = trackProperty.managedReferenceValue?.GetType();
                        var idx = Array.IndexOf(FrameTrackDrawer.types, type);
                        idx = EditorGUI.Popup(popupPos, "", idx, FrameTrackDrawer.typenames);
                        if(idx >= 0) {
                            var serType = FrameTrackDrawer.types[idx];
                            if(trackProperty.managedReferenceValue?.GetType() != serType) {
                                trackProperty.managedReferenceValue = Activator.CreateInstance(serType);
                                dirty = true;
                            }
                        }

                        // path field
                        var pathProperty = trackProperty.FindPropertyRelative("componentPath");
                        if(pathProperty != null) {
                            EditorGUI.BeginChangeCheck();
                            string trackPath = EditorGUI.DelayedTextField(pathPos, pathProperty.stringValue, DirectorStyles.trackPathField);
                            if(EditorGUI.EndChangeCheck()) {
                                pathProperty.stringValue = trackPath;
                                dirty = true;
                            }
                        }

                        // minus button
                        if(GUI.Button(minusBtnPos, "", DirectorStyles.trackMinusBtn)) {
                            tracksProperty.DeleteArrayElementAtIndex(i);
                            dirty = true;
                            breakFlag = true;
                        }
                        
                        EditorGUI.EndProperty();

                        if(breakFlag)
                            break;
                    }
                }

                // add button
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                Rect plusBtnPos = GUILayoutUtility.GetRect(0f, btnWidth, 0f, layoutRowHeight);
                if(GUI.Button(plusBtnPos, "", DirectorStyles.trackPlusBtn)) {
                    tracksProperty.InsertArrayElementAtIndex(tracksProperty.arraySize);
                    dirty = true;
                }
                EditorGUILayout.EndHorizontal();

                if(dirty)
                    obj.ApplyModifiedProperties();
            }
        }
        #endregion

        #region content
        private void MainContentOnGUI(Rect contentLayoutRect) {
            var mainAreaControlID = 0;
            if(m_State.disabled) {
                SetupWizardOnGUI(contentLayoutRect);
            } else {
                Event evt = Event.current;

                if(triggerFraming && evt.type == EventType.Repaint) {
                    m_DopeSheet.FrameClip();
                    triggerFraming = false;
                }

                DopeSheetOnGUI(contentLayoutRect);
                mainAreaControlID = m_DopeSheet.areaControlID;
            }

            HandleMainAreaCopyPaste(mainAreaControlID);
        }
        private void SetupWizardOnGUI(Rect position) {
            GUI.Label(position, GUIContent.none, DirectorStyles.dopeSheetBackground);

            Rect positionWithoutScrollBar = new Rect(position.x, position.y, position.width - kSliderThickness, position.height - kSliderThickness);
            GUI.BeginClip(positionWithoutScrollBar);
            GUI.enabled = true;

            m_State.timeArea.SetShownHRangeInsideMargins(0f, 1f);

            bool animatableObject = m_State.activeGameObject && !EditorUtility.IsPersistent(m_State.activeGameObject);

            if(animatableObject) {
                var missingObjects = (!m_State.activeRootGameObject && m_State.activeAnimationAsset != null) ? DirectorStyles.animatorAndAnimationClip.text : DirectorStyles.animationClip.text;

                string txt = String.Format(DirectorStyles.formatIsMissing.text, m_State.activeGameObject.name, missingObjects);

                GUIContent textContent = Extensions.Temp(txt);
                Vector2 textSize = GUI.skin.label.CalcSize(textContent);
                Rect labelRect = new Rect(positionWithoutScrollBar.width * .5f - textSize.x * .5f, positionWithoutScrollBar.height * .5f - textSize.y * .5f, textSize.x, textSize.y);
                GUI.Label(labelRect, textContent);
            } else {
                Color oldColor = GUI.color;
                GUI.color = Color.gray;
                Vector2 textSize = GUI.skin.label.CalcSize(DirectorStyles.noAnimatableObjectSelectedText);
                Rect labelRect = new Rect(positionWithoutScrollBar.width * .5f - textSize.x * .5f, positionWithoutScrollBar.height * .5f - textSize.y * .5f, textSize.x, textSize.y);
                GUI.Label(labelRect, DirectorStyles.noAnimatableObjectSelectedText);
                GUI.color = oldColor;
            }
            GUI.EndClip();
            GUI.enabled = false;
        }
        private void DopeSheetOnGUI(Rect position) {
            Rect noVerticalSliderRect = new Rect(position.xMin, position.yMin, position.width - kSliderThickness, position.height);

            if(Event.current.type == EventType.Repaint) {
                m_DopeSheet.rect = noVerticalSliderRect;
                m_DopeSheet.SetTickMarkerRanges();
                m_DopeSheet.RecalculateBounds();
            }

            Rect noSlidersRect = new Rect(position.xMin, position.yMin, position.width - kSliderThickness, position.height - kSliderThickness);

            m_DopeSheet.BeginViewGUI();

            GUI.Label(position, GUIContent.none, DirectorStyles.dopeSheetBackground);

            if(!m_State.disabled) {
                m_DopeSheet.TimeRuler(noSlidersRect, m_State.frameRate, false, true, kDisabledRulerAlpha, (int)m_State.timeFormat);  // grid
            }
            m_DopeSheet.OnGUI(noSlidersRect, m_State.hierarchyState.scrollPos * -1);

            m_DopeSheet.EndViewGUI();

            //Rect verticalScrollBarPosition = new Rect(noVerticalSliderRect.xMax, noVerticalSliderRect.yMin, kSliderThickness, noSlidersRect.height);

            //float visibleHeight = m_Hierarchy.GetTotalRect().height;
            //float contentHeight = Mathf.Max(visibleHeight, m_Hierarchy.GetContentSize().y);

            //m_State.hierarchyState.scrollPos.y = GUI.VerticalScrollbar(verticalScrollBarPosition, m_State.hierarchyState.scrollPos.y, visibleHeight, 0f, contentHeight);

            //if(m_DopeSheet.spritePreviewLoading == true)
            //    Repaint();
        }
        private void HandleMainAreaCopyPaste(int controlID) {
            var evt = Event.current;
            var type = evt.GetTypeForControl(controlID);
            if(type != EventType.ValidateCommand && type != EventType.ExecuteCommand)
                return;

            //if(evt.commandName == EventCommandNames.Copy) {
            //    // If events timeline has selected events right now then bail out; copying of
            //    // these will get processed later by AnimationEventTimeLine.
            //    if(m_Events.HasSelectedEvents)
            //        return;

            //    if(type == EventType.ExecuteCommand) {
            //        if(m_State.showCurveEditor)
            //            UpdateSelectedKeysFromCurveEditor();
            //        m_State.CopyKeys();
            //    }
            //    evt.Use();
            //} else if(evt.commandName == EventCommandNames.Paste) {
            //    if(type == EventType.ExecuteCommand) {
            //        // If clipboard contains events right now then paste those.
            //        if(AnimationWindowEventsClipboard.CanPaste()) {
            //            m_Events.PasteEvents(m_State.activeRootGameObject, m_State.activeAnimationClip, m_State.currentTime);
            //        } else {
            //            SaveCurveEditorKeySelection();
            //            m_State.PasteKeys();
            //            UpdateSelectedKeysToCurveEditor();
            //        }

            //        // data is scheduled for an update, bail out now to avoid using out of date data.
            //        EditorGUIUtility.ExitGUI();
            //    }
            //    evt.Use();
            //}
        }
        private void TimeRulerOnGUI(Rect timeRulerRect) {
            Rect timeRulerRectNoScrollbar = new Rect(timeRulerRect.xMin, timeRulerRect.yMin, timeRulerRect.width - kSliderThickness, timeRulerRect.height);
            Rect timeRulerBackgroundRect = timeRulerRectNoScrollbar;

            GUI.Box(timeRulerBackgroundRect, GUIContent.none, DirectorStyles.timeRulerBackground);

            if(!m_State.disabled) {
                RenderInRangeOverlay(timeRulerRectNoScrollbar);
                RenderSelectionOverlay(timeRulerRectNoScrollbar);
            }

            m_State.timeArea.TimeRuler(timeRulerRectNoScrollbar, m_State.frameRate, true, false, 1f, (int)m_State.timeFormat);

            if(!m_State.disabled)
                RenderOutOfRangeOverlay(timeRulerRectNoScrollbar);
        }
        private void RenderInRangeOverlay(Rect rect) {
            Color color = inRangeColor;

            if(m_State.recording)
                color *= AnimationMode.recordedPropertyColor;
            else if(m_State.previewing)
                color *= AnimationMode.animatedPropertyColor;
            else
                color = Color.clear;

            Vector2 timeRange = m_State.timeRange;
            FrameAnimWinUtility.DrawInRangeOverlay(rect, color, m_State.TimeToPixel(timeRange.x) + rect.xMin, m_State.TimeToPixel(timeRange.y) + rect.xMin);
        }
        private void RenderSelectionOverlay(Rect rect) {
            //const int kOverlayMinWidth = 14;

            //Bounds bounds = m_State.showCurveEditor ? m_CurveEditor.selectionBounds : m_State.selectionBounds;

            //float startPixel = m_State.TimeToPixel(bounds.min.x) + rect.xMin;
            //float endPixel = m_State.TimeToPixel(bounds.max.x) + rect.xMin;

            //if((endPixel - startPixel) < kOverlayMinWidth) {
            //    float centerPixel = (startPixel + endPixel) * 0.5f;

            //    startPixel = centerPixel - kOverlayMinWidth * 0.5f;
            //    endPixel = centerPixel + kOverlayMinWidth * 0.5f;
            //}

            //FrameAnimWinUtility.DrawSelectionOverlay(rect, selectionRangeColor, startPixel, endPixel);
        }
        private void RenderOutOfRangeOverlay(Rect rect) {
            Color color = outOfRangeColor;

            if(m_State.recording)
                color *= AnimationMode.recordedPropertyColor;
            else if(m_State.previewing)
                color *= AnimationMode.animatedPropertyColor;

            Vector2 timeRange = m_State.timeRange;
            FrameAnimWinUtility.DrawOutOfRangeOverlay(rect, color, m_State.TimeToPixel(timeRange.x) + rect.xMin, m_State.TimeToPixel(timeRange.y) + rect.xMin);
        }
        private void OverlayOnGUI(Rect contentRect) {
            if(!m_State.animatorIsOptimized)
                return;
            if(m_State.disabled)
                return;
            if(Event.current.type != EventType.Repaint)
                return;

            Rect contentRectNoSliders = new Rect(contentRect.xMin, contentRect.yMin, contentRect.width - kSliderThickness, contentRect.height - kSliderThickness);
            Rect overlayRectNoSliders = new Rect(hierarchyWidth - 1, 0f, contentWidth - kSliderThickness, m_Position.height - kSliderThickness);

            GUI.BeginGroup(overlayRectNoSliders);

            Rect localRect = new Rect(0, 0, overlayRectNoSliders.width, overlayRectNoSliders.height);
            Rect localContentRect = contentRectNoSliders;
            localContentRect.position -= overlayRectNoSliders.min;
            m_Overlay.OnGUI(localRect, localContentRect);

            GUI.EndGroup();
        }
        #endregion
    }
}
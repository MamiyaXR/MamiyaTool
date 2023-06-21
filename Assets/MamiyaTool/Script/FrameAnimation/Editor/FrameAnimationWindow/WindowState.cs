using UnityEngine;
using UnityEditor;
using System.Drawing.Drawing2D;
using System;

namespace MamiyaTool {
    internal class WindowState {
        #region field
        public AnimEditor animEditor;
        public FrameAnimWinHierarchyState hierarchyState = new();
        private FrameAnimWinSelectionItem m_Selection;
        private int[] m_SelectionFilter;

        private float m_FrameRate = kDefaultFrameRate;
        private TimeArea m_TimeArea;

        public Action<float> onFrameRateChange;

        private RefreshType m_Refresh = RefreshType.None;
        #endregion

        #region accessor
        public FrameAnimWinSelectionItem selection {
            get {
                if(m_Selection == null) {
                    m_Selection = FrameAnimWinSelectionItem.Create(null, null);
                    m_Selection.state = this;
                }
                return m_Selection;
            }
        }
        public bool disabled { get => selection.disable; }
        public RefreshType refresh {
            get { return m_Refresh; }
            // Make sure that if full refresh is already ordered, nobody gets to f*** with it
            set {
                if((int)m_Refresh < (int)value)
                    m_Refresh = value;
            }
        }
        public bool filterBySelection {
            get {
                return FrameAnimWinOptions.filterBySelection;
            }
            set {
                FrameAnimWinOptions.filterBySelection = value;
                UpdateSelectionFilter();

                // Refresh everything.
                refresh = RefreshType.Everything;
            }
        }
        public float frameRate {
            get {
                return m_FrameRate;
            }
            set {
                if(m_FrameRate != value) {
                    m_FrameRate = value;
                    //if(onFrameRateChange != null)
                    //    onFrameRateChange(m_FrameRate);
                }
            }
        }
        public float clipFrameRate {
            get {
                if(activeAnimationAsset == null)
                    return kDefaultFrameRate;
                return activeAnimationAsset.SampleRate;
            }
        }
        public GameObject activeGameObject {
            get {
                return selection.gameObject;
            }
            set {
                selection.gameObject = value;
                OnSelectionChanged();
            }
        }
        public GameObject activeRootGameObject {
            get {
                return selection.rootGameObject;
            }
        }
        public FrameAnimation activeAnimationClip {
            get {
                return selection.animationClip;
            }
        }
        public FrameAnimationAsset activeAnimationAsset {
            get {
                return selection.animationAsset;
            }
            set {
                selection.animationAsset = value;
                OnSelectionChanged();
            }
        }
        public Vector2 timeRange {
            get {
                if(activeAnimationAsset != null)
                    return new Vector2(FrameAnimUtility.GetStartTime(activeAnimationAsset), FrameAnimUtility.GetStopTime(activeAnimationAsset));

                return Vector2.zero;
            }
        }
        public TimeArea.TimeFormat timeFormat {
            get {
                return FrameAnimWinOptions.timeFormat;
            }
            set {
                FrameAnimWinOptions.timeFormat = value;
            }
        }
        public TimeArea timeArea {
            get { return m_TimeArea; }
            set { m_TimeArea = value; }
        }
        // Pixel to time ratio (used for time-pixel conversions)
        public float pixelPerSecond {
            get { return timeArea.m_Scale.x; }
        }
        // The GUI x-coordinate, where time==0 (used for time-pixel conversions)
        public float zeroTimePixel {
            get { return timeArea.shownArea.xMin * timeArea.m_Scale.x * -1f; }
        }
        public float visibleTimeSpan { get; }
        public float minVisibleTime { get; }
        public float currentTime { get; }
        public bool animatorIsOptimized => selection.objectIsOptimized;

        public bool canPreview { get => selection.canPreview; }
        public bool canRecord { get => selection.canRecord; }
        public bool canPlay { get => selection.canPlay; }
        public bool previewing {
            get => selection.previewing;
            set {
                if(selection.previewing == value)
                    return;

                if(value) {
                    if(canPreview)
                        selection.previewing = true;
                } else {
                    selection.playing = false;
                    selection.recording = false;
                    selection.previewing = false;
                }
            }
        }
        public bool recording {
            get => selection.recording;
            set {
                if(selection.recording == value)
                    return;
                if(value) {
                    if(canRecord) {
                        selection.previewing = true;
                        if(selection.previewing)
                            selection.recording = true;
                    }
                } else
                    selection.recording = false;
            }
        }
        public bool playing {
            get => selection.playing;
            set {
                if(selection.playing == value)
                    return;
                if(value) {
                    if(canPlay) {
                        selection.previewing = true;
                        if(selection.previewing)
                            selection.playing = true;
                    }
                } else
                    selection.playing = false;
            }
        }
        public int currentFrame { get => selection.frame; set => selection.frame = value; }

        public int ClipFrameRate {
            get {
                if(activeAnimationAsset == null)
                    return kDefaultFrameRate;
                return activeAnimationAsset.SampleRate;
            }
            set {
                if(activeAnimationAsset != null && value > 0 && value <= 10000) {
                    ClearKeySelections();
                    FrameAnimUtility.SaveSampleRate(activeAnimationAsset, value);
                }
            }
        }
        #endregion

        #region const define
        public const int kDefaultFrameRate = 60;
        #endregion

        public bool FilterBySelection;

        public bool ShowReadOnly;
        /*****************************************************************
         * 
         *      public method
         * 
         *****************************************************************/
        public float TimeToPixel(float time) {
            return TimeToPixel(time, SnapMode.Disabled);
        }
        public float TimeToPixel(float time, SnapMode snap) {
            return SnapToFrame(time, snap) * pixelPerSecond + zeroTimePixel;
        }
        public float SnapToFrame(float time, SnapMode snap) {
            if(snap == SnapMode.Disabled)
                return time;

            float fps = (snap == SnapMode.SnapToFrame) ? frameRate : clipFrameRate;
            return SnapToFrame(time, fps);
        }
        public float SnapToFrame(float time, float fps) {
            float snapTime = Mathf.Round(time * fps) / fps;
            return snapTime;
        }
        public void ClearKeySelections() {

        }
        public void Repaint() {
            if(animEditor != null)
                animEditor.Repaint();
        }
        public bool PlaybackUpdate() {
            if(disabled)
                return false;
            if(!selection.playing)
                return false;
            return selection.PlaybackUpdate();
        }

        #region event
        public void OnSelectionChanged() {
            if(onFrameRateChange != null)
                onFrameRateChange(frameRate);

            UpdateSelectionFilter();

            if(animEditor != null)
                animEditor.OnSelectionChanged();
        }
        #endregion
        /*****************************************************************
         * 
         *      private method
         * 
         *****************************************************************/
        private void UpdateSelectionFilter() {
            m_SelectionFilter = (filterBySelection) ? (int[])Selection.instanceIDs.Clone() : null;
        }
        /*****************************************************************
         * 
         *      enum define
         * 
         *****************************************************************/
        public enum RefreshType {
            None = 0,
            CurvesOnly = 1,
            Everything = 2
        }
        public enum SnapMode {
            Disabled = 0,
            SnapToFrame = 1,
            [Obsolete("SnapToClipFrame has been made redundant with SnapToFrame, SnapToFrame will behave the same.")]
            SnapToClipFrame = 2
        }
    }
}
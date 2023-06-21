using UnityEngine;
using UnityEditor;

namespace MamiyaTool {
    internal class AnimEditorOverlay {
        #region field
        public WindowState state;

        private TimeCursorManipulator m_PlayHeadCursor;

        private Rect m_Rect;
        private Rect m_ContentRect;

        #endregion

        #region accessor
        public Rect rect => m_Rect;
        public Rect contentRect => m_ContentRect;
        #endregion
        /*****************************************************************
         * 
         *      public method
         * 
         *****************************************************************/
        public void Initialize() {
            if(m_PlayHeadCursor == null) {
                m_PlayHeadCursor = new TimeCursorManipulator(DirectorStyles.playHead);
                m_PlayHeadCursor.onStartDrag += (FrameAnimWinManipulator manipulator, Event evt) => {
                    if(evt.mousePosition.y <= (m_Rect.yMin + 20))
                        return OnStartDragPlayHead(evt);
                    return false;
                };
                m_PlayHeadCursor.onDrag += (FrameAnimWinManipulator manipulator, Event evt) => {
                    return OnDragPlayHead(evt);
                };
                m_PlayHeadCursor.onEndDrag += (FrameAnimWinManipulator manipulator, Event evt) => {
                    return OnEndDragPlayHead(evt);
                };
            }
        }
        public void OnGUI(Rect rect, Rect contentRect) {
            if(Event.current.type != EventType.Repaint)
                return;

            m_Rect = rect;
            m_ContentRect = contentRect;

            Initialize();

            m_PlayHeadCursor.OnGUI(m_Rect, m_Rect.xMin + TimeToPixel(state.currentTime));
        }
        public void HandleEvents() {
            Initialize();

            m_PlayHeadCursor.HandleEvents();
        }
        /*****************************************************************
         * 
         *      private method
         * 
         *****************************************************************/
        private bool OnStartDragPlayHead(Event evt) {
            state.playing = false;

            state.selection.time = MousePositionToTime(evt);
            return true;
        }
        private bool OnDragPlayHead(Event evt) {
            state.selection.time = MousePositionToTime(evt);
            return true;
        }
        private bool OnEndDragPlayHead(Event evt) {
            return true;
        }
        public float MousePositionToTime(Event evt) {
            float width = m_ContentRect.width;
            float time = Mathf.Max(((evt.mousePosition.x / width) * state.visibleTimeSpan + state.minVisibleTime), 0);
            time = state.SnapToFrame(time, WindowState.SnapMode.SnapToFrame);
            return time;
        }
        public float TimeToPixel(float time) {
            return state.TimeToPixel(time);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace MamiyaTool {
    internal class TimeArea {
        #region reflection info
        static BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        #region type
        static Lazy<Type> timeAreaType = new Lazy<Type>(() => {
            var type = typeof(EditorWindow).Assembly.GetTypes().First(x => x.FullName == "UnityEditor.TimeArea");
            return type;
        });
        static Lazy<Type> dopeSheetEditorType = new Lazy<Type>(() => {
            var type = typeof(EditorWindow).Assembly.GetTypes().First(x => x.FullName == "UnityEditorInternal.DopeSheetEditor");
            return type;
        });
        static Lazy<Type> timeFormatType = new Lazy<Type>(() => {
            var type = timeAreaType.Value.GetNestedType("TimeFormat");
            return type;
        });
        static Lazy<Type> guiClipType = new Lazy<Type>(() => {
            var type = typeof(GUIUtility).Assembly.GetTypes().First(x => x.FullName == "UnityEngine.GUIClip");
            return type;
        });
        #endregion

        #region constructor
        static Lazy<ConstructorInfo> timeAreaCtor = new Lazy<ConstructorInfo>(() => {
            var type = dopeSheetEditorType.Value;
            return type.GetConstructor(flags, null, new Type[] { typeof(EditorWindow) }, null);
        });
        #endregion

        #region field
        static Lazy<FieldInfo> m_ScaleInfo = new Lazy<FieldInfo>(() => {
            var type = timeAreaType.Value;
            return type.GetField("m_Scale", flags);
        });
        static Lazy<FieldInfo> ownerInfo = new Lazy<FieldInfo>(() => {
            var type = dopeSheetEditorType;
            return type.Value.GetField("m_Owner", flags);
        });
        static Lazy<FieldInfo> boundsInfo = new Lazy<FieldInfo>(() => {
            var type = dopeSheetEditorType;
            return type.Value.GetField("m_Bounds", flags);
        });
        static Lazy<FieldInfo> areaControlIDInfo = new Lazy<FieldInfo>(() => {
            var type = dopeSheetEditorType.Value;
            return type.GetField("areaControlID", flags);
        });
        static Lazy<FieldInfo> pointRenderInfo = new Lazy<FieldInfo>(() => {
            var type = dopeSheetEditorType.Value;
            return type.GetField("m_PointRenderer", flags);
        });
        static Lazy<FieldInfo> rectangleToolInfo = new Lazy<FieldInfo>(() => {
            var type = dopeSheetEditorType.Value;
            return type.GetField("m_RectangleTool", flags);
        });
        #endregion

        #region property
        static Lazy<PropertyInfo> shownAreaInfo = new Lazy<PropertyInfo>(() => {
            var type = timeAreaType.Value;
            return type.GetProperty("shownArea", flags);
        });
        static Lazy<MethodInfo> shownGet = new Lazy<MethodInfo>(() => {
            return shownAreaInfo.Value.GetGetMethod();
        });
        static Lazy<MethodInfo> shownSet = new Lazy<MethodInfo>(() => {
            return shownAreaInfo.Value.GetSetMethod();
        });
        static Lazy<PropertyInfo> hSliderInfo = new Lazy<PropertyInfo>(() => {
            var type = timeAreaType.Value;
            return type.GetProperty("hSlider", flags);
        });
        static Lazy<MethodInfo> hSliderGet = new Lazy<MethodInfo>(() => {
            return hSliderInfo.Value.GetGetMethod();
        });
        static Lazy<MethodInfo> hSliderSet = new Lazy<MethodInfo>(() => {
            return hSliderInfo.Value.GetSetMethod();
        });
        static Lazy<PropertyInfo> rectInfo = new Lazy<PropertyInfo>(() => {
            var type = timeAreaType.Value;
            return type.GetProperty("rect", flags);
        });
        static Lazy<MethodInfo> rectGet = new Lazy<MethodInfo>(() => {
            return rectInfo.Value.GetGetMethod();
        });
        static Lazy<MethodInfo> rectSet = new Lazy<MethodInfo>(() => {
            return rectInfo.Value.GetSetMethod();
        });
        static Lazy<PropertyInfo> hTicksInfo = new Lazy<PropertyInfo>(() => {
            var type = timeAreaType.Value;
            return type.GetProperty("hTicks", flags);
        });
        static Lazy<MethodInfo> hTicksGet = new Lazy<MethodInfo>(() => {
            return hTicksInfo.Value.GetGetMethod();
        });
        static Lazy<MethodInfo> hTicksSet = new Lazy<MethodInfo>(() => {
            return hTicksInfo.Value.GetSetMethod();
        });
        #endregion

        #region method
        static Lazy<MethodInfo> timeRuler = new Lazy<MethodInfo>(() => {
            var type = timeAreaType.Value;
            return type.GetMethod("TimeRuler", flags, null, new Type[] { typeof(Rect), typeof(float), typeof(bool), typeof(bool), typeof(float), timeFormatType.Value }, null);
        });
        static Lazy<MethodInfo> drawVerticalLine = new Lazy<MethodInfo>(() => {
            var type = timeAreaType.Value;
            return type.GetMethod("DrawVerticalLine", flags, null, new Type[] { typeof(float), typeof(float), typeof(float), typeof(Color) }, null);
        });
        static Lazy<MethodInfo> setShownHRangeInsideMargins = new Lazy<MethodInfo>(() => {
            var type = timeAreaType.Value;
            return type.GetMethod("SetShownHRangeInsideMargins", flags, null, new Type[] { typeof(float), typeof(float) }, null);
        });
        static Lazy<MethodInfo> setTickMarkerRanges = new Lazy<MethodInfo>(() => {
            var type = timeAreaType.Value;
            return type.GetMethod("SetTickMarkerRanges", flags, null, Type.EmptyTypes, null);
        });
        static Lazy<MethodInfo> beginViewGUI = new Lazy<MethodInfo>(() => {
            var type = timeAreaType.Value;
            return type.GetMethod("BeginViewGUI", flags, null, Type.EmptyTypes, null);
        });
        static Lazy<MethodInfo> endViewGUI = new Lazy<MethodInfo>(() => {
            var type = timeAreaType.Value;
            return type.GetMethod("EndViewGUI", flags, null, Type.EmptyTypes, null);
        });
        static Lazy<MethodInfo> initInfo = new Lazy<MethodInfo>(() => {
            var type = dopeSheetEditorType.Value;
            return type.GetMethod("Init", flags, null, Type.EmptyTypes, null);
        });
        static Lazy<MethodInfo> drawMasterDopelineBackground = new Lazy<MethodInfo>(() => {
            var type = dopeSheetEditorType.Value;
            return type.GetMethod("DrawMasterDopelineBackground", flags, null, new Type[] { typeof(Rect) }, null);
        });
        static Lazy<MethodInfo> drawBox = new Lazy<MethodInfo>(() => {
            var type = dopeSheetEditorType.Value;
            return type.GetMethod("DrawBox", flags, null, new Type[] { typeof(Rect), typeof(Color) }, null);
        });
        #endregion

        #endregion

        #region field
        private EditorWindow owner;
        public object Value {
            get {
                if(value == null)
                    value = timeAreaCtor.Value.Invoke(new object[] { owner });
                return value;
            }
        }
        private object value;
        public WindowState state;
        private object PointRenderer {
            get {
                if(pointRenderer == null)
                    pointRenderer = pointRenderInfo.Value.GetValue(value);
                return pointRenderer;
            }
        }
        private object pointRenderer;
        private object RectangleTool {
            get {
                if(rectangleTool == null)
                    rectangleTool = rectangleToolInfo.Value.GetValue(value);
                return rectangleTool;
            }
        }
        private object rectangleTool;
        #endregion

        #region accessor
        internal Vector2 m_Scale {
            get {
                return (Vector2)(m_ScaleInfo.Value.GetValue(Value));
            }
        }
        public Rect shownArea {
            get {
                return (Rect)(shownGet.Value.Invoke(Value, null));
            }
            set {
                shownSet.Value.Invoke(Value, new object[] { value });
            }
        }
        public bool hSlider {
            get {
                return (bool)(hSliderGet.Value.Invoke(Value, null));
            }
            set {
                hSliderSet.Value.Invoke(Value, new object[] { value });
            }
        }
        public Rect rect {
            get {
                return (Rect)(rectGet.Value.Invoke(Value, null));
            }
            set {
                rectSet.Value.Invoke(Value, new object[] { value });
            }
        }
        public EditorWindow m_Owner {
            get { return ownerInfo.Value.GetValue(Value) as EditorWindow; }
            set { ownerInfo.Value.SetValue(Value, value); }
        }
        public Bounds m_Bounds {
            get { return (Bounds)(boundsInfo.Value.GetValue(Value)); }
            set { boundsInfo.Value.SetValue(Value, value); }
        }
        internal int areaControlID {
            get { return (int)(areaControlIDInfo.Value.GetValue(Value)); }
            set { areaControlIDInfo.Value.SetValue(Value, value); }
        }
        private bool m_SpritePreviewLoading {
            get => (bool)(ReflectionUtility.GetField(dopeSheetEditorType.Value, "m_SpritePreviewLoading", Value));
            set => ReflectionUtility.SetField(dopeSheetEditorType.Value, "m_SpritePreviewLoading", Value, value);
        }
        private bool m_IsDragging {
            get => (bool)(ReflectionUtility.GetField(dopeSheetEditorType.Value, "m_IsDragging", Value));
            set => ReflectionUtility.SetField(dopeSheetEditorType.Value, "m_IsDragging", Value, value);
        }
        private int m_SpritePreviewCacheSize {
            get => (int)(ReflectionUtility.GetField(dopeSheetEditorType.Value, "m_SpritePreviewCacheSize", Value));
            set => ReflectionUtility.SetField(dopeSheetEditorType.Value, "m_SpritePreviewCacheSize", Value, value);
        }
        #endregion

        #region public method
        public TimeArea(EditorWindow owner) {
            this.owner = owner;
        }
        public void TimeRuler(Rect position, float frameRate, bool labels, bool useEntireHeight, float alpha,
            int timeFormat) {
            timeRuler.Value.Invoke(Value, new object[] { position, frameRate, labels, useEntireHeight, alpha, timeFormat });
        }
        public static void DrawVerticalLine(float x, float minY, float maxY, Color color) {
            drawVerticalLine.Value.Invoke(null, new object[] { x, minY, maxY, color });
        }
        public void SetShownHRangeInsideMargins(float min, float max) {
            setShownHRangeInsideMargins.Value.Invoke(Value, new object[] { min, max });
        }
        public void SetTickMarkerRanges() {
            setTickMarkerRanges.Value.Invoke(Value, null);
        }
        public void SetTickModulosForFrameRate(float frameRate) {
            object hTicks = hTicksGet.Value.Invoke(Value, null);
            Type type = hTicks.GetType();
            ReflectionUtility.InvokeMethod(type, "SetTickModulosForFrameRate", hTicks, new Type[] { typeof(float) }, new object[] { frameRate });
        }
        public void FrameClip() {
            if(state.disabled)
                return;

            Vector2 timeRange = state.timeRange;
            timeRange.y = Mathf.Max(timeRange.x + 0.1f, timeRange.y);
            SetShownHRangeInsideMargins(timeRange.x, timeRange.y);
        }
        public void RecalculateBounds() {
            if(!state.disabled) {
                Vector2 timeRange = state.timeRange;
                Bounds bounds = m_Bounds;
                bounds.SetMinMax(new Vector3(timeRange.x, 0, 0), new Vector3(timeRange.y, 0, 0));
                m_Bounds = bounds;
            }
        }
        public void BeginViewGUI() {
            beginViewGUI.Value.Invoke(Value, null);
        }
        public void EndViewGUI() {
            endViewGUI.Value.Invoke(Value, null);
        }
        public void OnGUI(Rect position, Vector2 scrollPosition) {
            initInfo.Value.Invoke(Value, null);
            ReflectionUtility.InvokeStaticMethod(guiClipType.Value, "Push",
                                    new Type[] { typeof(Rect), typeof(Vector2), typeof(Vector2), typeof(bool) },
                                    new object[] { position, scrollPosition, Vector2.zero, false });

            Rect localRect = new Rect(0, 0, position.width, position.height);
            Rect dopesheetRect = DopelinesGUI(localRect, scrollPosition);

            ReflectionUtility.InvokeStaticMethod(guiClipType.Value, "Pop");
        }
        #endregion

        #region private method
        private Rect DopelinesGUI(Rect position, Vector2 scrollPosition) {
            Color oldColor = GUI.color;
            Rect linePosition = position;

            ReflectionUtility.InvokeMethod(PointRenderer.GetType(), "Clear", PointRenderer);
            if(Event.current.type == EventType.Repaint)
                m_SpritePreviewLoading = false;
            if(Event.current.type == EventType.MouseDown)
                m_IsDragging = false;

            UpdateSpritePreviewCacheSize();

            List<DopeLine> dopelines = state.dopelines;
            for(int i = 0; i < dopelines.Count; ++i) {
                DopeLine dopeLine = dopelines[i];

                dopeLine.position = linePosition;
                dopeLine.position.height = (dopeLine.tallMode ? k_DopeSheetRowHeightTall : k_DopeSheetRowHeight);

                if(dopeLine.position.yMin + scrollPosition.y >= position.yMin && dopeLine.position.yMin + scrollPosition.y <= position.yMax ||
                    dopeLine.position.yMax + scrollPosition.y >= position.yMin && dopeLine.position.yMax + scrollPosition.y <= position.yMax) {
                    Event evt = Event.current;

                    switch(evt.type) {
                        case EventType.DragUpdated:
                        case EventType.DragPerform:
                            break;
                        case EventType.ContextClick:
                            break;
                        case EventType.MouseDown:
                            break;
                        case EventType.Repaint: {
                            DopeLineRepaint(dopeLine);
                            break;
                        }
                    }
                }

                linePosition.y += dopeLine.position.height;
            }

            Rect dopelinesRect = new Rect(position.xMin, position.yMin, position.width, linePosition.yMax - position.yMin);

            if(Event.current.type == EventType.Repaint)
                ReflectionUtility.InvokeMethod(PointRenderer.GetType(), "Render", PointRenderer);

            return dopelinesRect;
        }
        private void UpdateSpritePreviewCacheSize() {
            
        }
        private void DopeLineRepaint(DopeLine dopeline) {
            Color oldColor = GUI.color;
            Color color = Color.gray.AlphaMultiplied(0.05f);

            // Draw background
            if(dopeline.isMasterDopeline)
                DrawMasterDopelineBackground(dopeline.position);
            else
                DrawBox(dopeline.position, color);
        }
        private void DrawMasterDopelineBackground(Rect position) {
            drawMasterDopelineBackground.Value.Invoke(Value, new object[] { position });
        }
        private static void DrawBox(Rect position, Color color) {
            drawBox.Value.Invoke(null, new object[] { position, color });
        }
        #endregion

        #region enum define
        public enum TimeFormat {
            None, // Unformatted time
            TimeFrame, // Time:Frame
            Frame // Integer frame
        }
        #endregion

        #region value define
        private static readonly float k_DopeSheetRowHeight = (float)ReflectionUtility.GetField<EditorGUI>("kSingleLineHeight", null);
        private static readonly float k_DopeSheetRowHeightTall = k_DopeSheetRowHeight * 2f;
        #endregion
    }
}
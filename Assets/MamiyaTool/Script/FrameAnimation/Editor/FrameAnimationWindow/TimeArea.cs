using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System.Globalization;
using Mono.Reflection;

namespace MamiyaTool {
    #region timearea
    //[Serializable]
    //internal class TimeArea : ZoomableArea {
    //    [SerializeField] private TickHandler m_HTicks;

    //    public TickHandler hTicks {
    //        get { return m_HTicks; }
    //        set { m_HTicks = value; }
    //    }

    //    [SerializeField] private TickHandler m_VTicks;

    //    static readonly List<float> s_TickCache = new List<float>(1000);

    //    public TickHandler vTicks {
    //        get { return m_VTicks; }
    //        set { m_VTicks = value; }
    //    }

    //    internal const int kTickRulerDistMin = 3
    //    ;     // min distance between ruler tick marks before they disappear completely

    //    internal const int kTickRulerDistFull = 80; // distance between ruler tick marks where they gain full strength
    //    internal const int kTickRulerDistLabel = 40; // min distance between ruler tick mark labels
    //    internal const float kTickRulerHeightMax = 0.7f; // height of the ruler tick marks when they are highest

    //    internal const float kTickRulerFatThreshold = 0.5f
    //    ;     // size of ruler tick marks at which they begin getting fatter

    //    public enum TimeFormat {
    //        None, // Unformatted time
    //        TimeFrame, // Time:Frame
    //        Frame // Integer frame
    //    }

    //    class Styles2 {
    //        public GUIStyle timelineTick = "AnimationTimelineTick";
    //        public GUIStyle playhead = "AnimationPlayHead";
    //    }

    //    static Styles2 timeAreaStyles;

    //    static void InitStyles() {
    //        if(timeAreaStyles == null)
    //            timeAreaStyles = new Styles2();
    //    }

    //    public TimeArea(bool minimalGUI) : this(minimalGUI, true, true) { }

    //    public TimeArea(bool minimalGUI, bool enableSliderZoom) : this(minimalGUI, enableSliderZoom, enableSliderZoom) { }

    //    public TimeArea(bool minimalGUI, bool enableSliderZoomHorizontal, bool enableSliderZoomVertical) : base(minimalGUI, enableSliderZoomHorizontal, enableSliderZoomVertical) {
    //        float[] modulos = new float[]
    //        {
    //            0.0000001f, 0.0000005f, 0.000001f, 0.000005f, 0.00001f, 0.00005f, 0.0001f, 0.0005f,
    //            0.001f, 0.005f, 0.01f, 0.05f, 0.1f, 0.5f, 1, 5, 10, 50, 100, 500,
    //            1000, 5000, 10000, 50000, 100000, 500000, 1000000, 5000000, 10000000
    //        };
    //        hTicks = new TickHandler();
    //        hTicks.SetTickModulos(modulos);
    //        vTicks = new TickHandler();
    //        vTicks.SetTickModulos(modulos);
    //    }

    //    public void SetTickMarkerRanges() {
    //        hTicks.SetRanges(shownArea.xMin, shownArea.xMax, drawRect.xMin, drawRect.xMax);
    //        vTicks.SetRanges(shownArea.yMin, shownArea.yMax, drawRect.yMin, drawRect.yMax);
    //    }

    //    public void DrawMajorTicks(Rect position, float frameRate) {
    //        GUI.BeginGroup(position);
    //        if(Event.current.type != EventType.Repaint) {
    //            GUI.EndGroup();
    //            return;
    //        }
    //        InitStyles();

    //        ReflectionUtility.InvokeStaticMethod<HandleUtility>("ApplyWireMaterial");

    //        SetTickMarkerRanges();
    //        hTicks.SetTickStrengths(kTickRulerDistMin, kTickRulerDistFull, true);

    //        Color tickColor = timeAreaStyles.timelineTick.normal.textColor;
    //        tickColor.a = 0.1f;

    //        if(Application.platform == RuntimePlatform.WindowsEditor)
    //            GL.Begin(GL.QUADS);
    //        else
    //            GL.Begin(GL.LINES);

    //        // Draw tick markers of various sizes
    //        Rect theShowArea = shownArea;
    //        for(int l = 0; l < hTicks.tickLevels; l++) {
    //            float strength = hTicks.GetStrengthOfLevel(l) * .9f;
    //            if(strength > kTickRulerFatThreshold) {
    //                s_TickCache.Clear();
    //                hTicks.GetTicksAtLevel(l, true, s_TickCache);
    //                for(int i = 0; i < s_TickCache.Count; i++) {
    //                    if(s_TickCache[i] < 0)
    //                        continue;
    //                    int frame = Mathf.RoundToInt(s_TickCache[i] * frameRate);
    //                    float x = FrameToPixel(frame, frameRate, position, theShowArea);
    //                    // Draw line
    //                    DrawVerticalLineFast(x, 0.0f, position.height, tickColor);
    //                }
    //            }
    //        }

    //        GL.End();
    //        GUI.EndGroup();
    //    }

    //    public void TimeRuler(Rect position, float frameRate) {
    //        TimeRuler(position, frameRate, true, false, 1f, TimeFormat.TimeFrame);
    //    }

    //    public void TimeRuler(Rect position, float frameRate, bool labels, bool useEntireHeight, float alpha,
    //        TimeFormat timeFormat) {
    //        Color backupCol = GUI.color;
    //        GUI.BeginGroup(position);
    //        InitStyles();

    //        ReflectionUtility.InvokeStaticMethod<HandleUtility>("ApplyWireMaterial");

    //        Color tempBackgroundColor = GUI.backgroundColor;

    //        SetTickMarkerRanges();
    //        hTicks.SetTickStrengths(kTickRulerDistMin, kTickRulerDistFull, true);

    //        Color baseColor = timeAreaStyles.timelineTick.normal.textColor;
    //        baseColor.a = 0.75f * alpha;

    //        if(Event.current.type == EventType.Repaint) {
    //            if(Application.platform == RuntimePlatform.WindowsEditor)
    //                GL.Begin(GL.QUADS);
    //            else
    //                GL.Begin(GL.LINES);

    //            // Draw tick markers of various sizes

    //            Rect cachedShowArea = shownArea;
    //            for(int l = 0; l < hTicks.tickLevels; l++) {
    //                float strength = hTicks.GetStrengthOfLevel(l) * .9f;
    //                s_TickCache.Clear();
    //                hTicks.GetTicksAtLevel(l, true, s_TickCache);
    //                for(int i = 0; i < s_TickCache.Count; i++) {
    //                    if(s_TickCache[i] < hRangeMin || s_TickCache[i] > hRangeMax)
    //                        continue;
    //                    int frame = Mathf.RoundToInt(s_TickCache[i] * frameRate);

    //                    float height = useEntireHeight
    //                        ? position.height
    //                        : position.height * Mathf.Min(1, strength) * kTickRulerHeightMax;
    //                    float x = FrameToPixel(frame, frameRate, position, cachedShowArea);

    //                    // Draw line
    //                    DrawVerticalLineFast(x, position.height - height + 0.5f, position.height - 0.5f,
    //                        new Color(1, 1, 1, strength / kTickRulerFatThreshold) * baseColor);
    //                }
    //            }

    //            GL.End();
    //        }

    //        if(labels) {
    //            // Draw tick labels
    //            int labelLevel = hTicks.GetLevelWithMinSeparation(kTickRulerDistLabel);
    //            s_TickCache.Clear();
    //            hTicks.GetTicksAtLevel(labelLevel, false, s_TickCache);
    //            for(int i = 0; i < s_TickCache.Count; i++) {
    //                if(s_TickCache[i] < hRangeMin || s_TickCache[i] > hRangeMax)
    //                    continue;

    //                int frame = Mathf.RoundToInt(s_TickCache[i] * frameRate);
    //                // Important to take floor of positions of GUI stuff to get pixel correct alignment of
    //                // stuff drawn with both GUI and Handles/GL. Otherwise things are off by one pixel half the time.

    //                float labelpos = Mathf.Floor(FrameToPixel(frame, frameRate, position));
    //                string label = FormatTickTime(s_TickCache[i], frameRate, timeFormat);
    //                GUI.Label(new Rect(labelpos + 3, -1, 40, 20), label, timeAreaStyles.timelineTick);
    //            }
    //        }
    //        GUI.EndGroup();

    //        GUI.backgroundColor = tempBackgroundColor;
    //        GUI.color = backupCol;
    //    }

    //    public static void DrawPlayhead(float x, float yMin, float yMax, float thickness, float alpha) {
    //        if(Event.current.type != EventType.Repaint)
    //            return;
    //        InitStyles();
    //        float halfThickness = thickness * 0.5f;
    //        Color lineColor = timeAreaStyles.playhead.normal.textColor.AlphaMultiplied(alpha);
    //        if(thickness > 1f) {
    //            Rect labelRect = Rect.MinMaxRect(x - halfThickness, yMin, x + halfThickness, yMax);
    //            EditorGUI.DrawRect(labelRect, lineColor);
    //        } else {
    //            DrawVerticalLine(x, yMin, yMax, lineColor);
    //        }
    //    }

    //    public static void DrawVerticalLine(float x, float minY, float maxY, Color color) {
    //        if(Event.current.type != EventType.Repaint)
    //            return;

    //        Color backupCol = Handles.color;

    //        ReflectionUtility.InvokeStaticMethod<HandleUtility>("ApplyWireMaterial");
    //        if(Application.platform == RuntimePlatform.WindowsEditor)
    //            GL.Begin(GL.QUADS);
    //        else
    //            GL.Begin(GL.LINES);
    //        DrawVerticalLineFast(x, minY, maxY, color);
    //        GL.End();

    //        Handles.color = backupCol;
    //    }

    //    public static void DrawVerticalLineFast(float x, float minY, float maxY, Color color) {
    //        if(Application.platform == RuntimePlatform.WindowsEditor) {
    //            GL.Color(color);
    //            GL.Vertex(new Vector3(x - 0.5f, minY, 0));
    //            GL.Vertex(new Vector3(x + 0.5f, minY, 0));
    //            GL.Vertex(new Vector3(x + 0.5f, maxY, 0));
    //            GL.Vertex(new Vector3(x - 0.5f, maxY, 0));
    //        } else {
    //            GL.Color(color);
    //            GL.Vertex(new Vector3(x, minY, 0));
    //            GL.Vertex(new Vector3(x, maxY, 0));
    //        }
    //    }

    //    public enum TimeRulerDragMode {
    //        None,
    //        Start,
    //        End,
    //        Dragging,
    //        Cancel
    //    }

    //    static float s_OriginalTime;
    //    static float s_PickOffset;

    //    public TimeRulerDragMode BrowseRuler(Rect position, ref float time, float frameRate, bool pickAnywhere,
    //        GUIStyle thumbStyle) {
    //        int id = GUIUtility.GetControlID(3126789, FocusType.Passive);
    //        return BrowseRuler(position, id, ref time, frameRate, pickAnywhere, thumbStyle);
    //    }

    //    public TimeRulerDragMode BrowseRuler(Rect position, int id, ref float time, float frameRate, bool pickAnywhere,
    //        GUIStyle thumbStyle) {
    //        Event evt = Event.current;
    //        Rect pickRect = position;
    //        if(time != -1) {
    //            pickRect.x = Mathf.Round(TimeToPixel(time, position)) - thumbStyle.overflow.left;
    //            pickRect.width = thumbStyle.fixedWidth + thumbStyle.overflow.horizontal;
    //        }

    //        switch(evt.GetTypeForControl(id)) {
    //            case EventType.Repaint:
    //                if(time != -1) {
    //                    bool hover = position.Contains(evt.mousePosition);
    //                    pickRect.x += thumbStyle.overflow.left;
    //                    thumbStyle.Draw(pickRect, id == GUIUtility.hotControl, hover || id == GUIUtility.hotControl,
    //                        false, false);
    //                }
    //                break;
    //            case EventType.MouseDown:
    //                if(pickRect.Contains(evt.mousePosition)) {
    //                    GUIUtility.hotControl = id;
    //                    s_PickOffset = evt.mousePosition.x - TimeToPixel(time, position);
    //                    evt.Use();
    //                    return TimeRulerDragMode.Start;
    //                } else if(pickAnywhere && position.Contains(evt.mousePosition)) {
    //                    GUIUtility.hotControl = id;

    //                    float newT = SnapTimeToWholeFPS(PixelToTime(evt.mousePosition.x, position), frameRate);
    //                    s_OriginalTime = time;
    //                    if(newT != time)
    //                        GUI.changed = true;
    //                    time = newT;
    //                    s_PickOffset = 0;
    //                    evt.Use();
    //                    return TimeRulerDragMode.Start;
    //                }
    //                break;
    //            case EventType.MouseDrag:
    //                if(GUIUtility.hotControl == id) {
    //                    float newT = SnapTimeToWholeFPS(PixelToTime(evt.mousePosition.x - s_PickOffset, position),
    //                        frameRate);
    //                    if(newT != time)
    //                        GUI.changed = true;
    //                    time = newT;

    //                    evt.Use();
    //                    return TimeRulerDragMode.Dragging;
    //                }
    //                break;
    //            case EventType.MouseUp:
    //                if(GUIUtility.hotControl == id) {
    //                    GUIUtility.hotControl = 0;
    //                    evt.Use();
    //                    return TimeRulerDragMode.End;
    //                }
    //                break;
    //            case EventType.KeyDown:
    //                if(GUIUtility.hotControl == id && evt.keyCode == KeyCode.Escape) {
    //                    if(time != s_OriginalTime)
    //                        GUI.changed = true;
    //                    time = s_OriginalTime;

    //                    GUIUtility.hotControl = 0;
    //                    evt.Use();
    //                    return TimeRulerDragMode.Cancel;
    //                }
    //                break;
    //        }
    //        return TimeRulerDragMode.None;
    //    }

    //    private float FrameToPixel(float i, float frameRate, Rect rect, Rect theShownArea) {
    //        return (i - theShownArea.xMin * frameRate) * rect.width / (theShownArea.width * frameRate);
    //    }

    //    public float FrameToPixel(float i, float frameRate, Rect rect) {
    //        return FrameToPixel(i, frameRate, rect, shownArea);
    //    }

    //    public float TimeField(Rect rect, int id, float time, float frameRate, TimeFormat timeFormat) {
    //        if(timeFormat == TimeFormat.None) {
    //            float newTime = DoFloatField(rect, new Rect(0, 0, 0, 0), id, time, false);
    //            return SnapTimeToWholeFPS(newTime, frameRate);
    //        }

    //        if(timeFormat == TimeFormat.Frame) {
    //            int frame = Mathf.RoundToInt(time * frameRate);

    //            int newFrame = DoIntField(rect, new Rect(0, 0, 0, 0), id, frame, false, 0f);

    //            return (float)newFrame / frameRate;
    //        } else // if (timeFormat == TimeFormat.TimeFrame)
    //          {
    //            string str = FormatTime(time, frameRate, TimeFormat.TimeFrame);

    //            string allowedCharacters = "0123456789.,:";

    //            bool changed;
    //            str = DoTextField(id, rect, str, allowedCharacters, out changed, false, false, false);

    //            if(changed) {
    //                if(GUIUtility.keyboardControl == id) {
    //                    GUI.changed = true;

    //                    // Make sure that comma & period are interchangable.
    //                    str = str.Replace(',', '.');

    //                    // format is time:frame
    //                    int index = str.IndexOf(':');
    //                    if(index >= 0) {
    //                        string timeStr = str.Substring(0, index);
    //                        string frameStr = str.Substring(index + 1);

    //                        int timeValue, frameValue;
    //                        if(int.TryParse(timeStr, out timeValue) && int.TryParse(frameStr, out frameValue)) {
    //                            float newTime = (float)timeValue + (float)frameValue / frameRate;
    //                            return newTime;
    //                        }
    //                    }
    //                    // format is floating time value.
    //                    else {
    //                        float newTime;
    //                        if(float.TryParse(str, out newTime)) {
    //                            return SnapTimeToWholeFPS(newTime, frameRate);
    //                        }
    //                    }
    //                }
    //            }
    //        }

    //        return time;
    //    }

    //    public float ValueField(Rect rect, int id, float value) {
    //        float newValue = DoFloatField(rect, new Rect(0, 0, 0, 0), id, value, false);
    //        return newValue;
    //    }

    //    public string FormatTime(float time, float frameRate, TimeFormat timeFormat) {
    //        if(timeFormat == TimeFormat.None) {
    //            int hDecimals;
    //            if(frameRate != 0) {
    //                hDecimals = (int)(ReflectionUtility.InvokeStaticMethod<MathUtils>("GetNumberOfDecimalsForMinimumDifference",
    //                                    new Type[] { typeof(float) },
    //                                    new object[] { 1 / frameRate }));
    //            } else {
    //                hDecimals = (int)(ReflectionUtility.InvokeStaticMethod<MathUtils>("GetNumberOfDecimalsForMinimumDifference",
    //                                    new Type[] { typeof(float) },
    //                                    new object[] { shownArea.width / drawRect.width }));
    //            }

    //            return time.ToString("N" + hDecimals, CultureInfo.InvariantCulture.NumberFormat);
    //        }

    //        int frame = Mathf.RoundToInt(time * frameRate);

    //        if(timeFormat == TimeFormat.TimeFrame) {
    //            int frameDigits = frameRate != 0 ? ((int)frameRate - 1).ToString().Length : 1;
    //            string sign = string.Empty;
    //            if(frame < 0) {
    //                sign = "-";
    //                frame = -frame;
    //            }
    //            return sign + (frame / (int)frameRate) + ":" +
    //                (frame % frameRate).ToString().PadLeft(frameDigits, '0');
    //        } else {
    //            return frame.ToString();
    //        }
    //    }

    //    public virtual string FormatTickTime(float time, float frameRate, TimeFormat timeFormat) {
    //        return FormatTime(time, frameRate, timeFormat);
    //    }

    //    public string FormatValue(float value) {
    //        int vDecimals = (int)(ReflectionUtility.InvokeStaticMethod<MathUtils>("GetNumberOfDecimalsForMinimumDifference",
    //                                new Type[] { typeof(float) },
    //                                new object[] { shownArea.height / drawRect.height }));
    //        return value.ToString("N" + vDecimals, CultureInfo.InvariantCulture.NumberFormat);
    //    }

    //    public float SnapTimeToWholeFPS(float time, float frameRate) {
    //        if(frameRate == 0)
    //            return time;
    //        return Mathf.Round(time * frameRate) / frameRate;
    //    }

    //    public void DrawTimeOnSlider(float time, Color c, float maxTime, float leftSidePadding = 0, float rightSidePadding = 0) {
    //        const float maxTimeFudgeFactor = 3;
    //        if(!hSlider)
    //            return;

    //        if(styles.horizontalScrollbar == null)
    //            styles.InitGUIStyles(false, allowSliderZoomHorizontal, allowSliderZoomVertical);

    //        var inMin = TimeToPixel(0, rect); // Assume 0 minTime
    //        var inMax = TimeToPixel(maxTime, rect);
    //        var outMin = TimeToPixel(shownAreaInsideMargins.xMin, rect) + styles.horizontalScrollbarLeftButton.fixedWidth + leftSidePadding;
    //        var outMax = TimeToPixel(shownAreaInsideMargins.xMax, rect) - (styles.horizontalScrollbarRightButton.fixedWidth + rightSidePadding);
    //        var x = (TimeToPixel(time, rect) - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    //        if(x > rect.xMax - (styles.horizontalScrollbarLeftButton.fixedWidth + leftSidePadding + maxTimeFudgeFactor))
    //            return;
    //        var inset = styles.sliderWidth - styles.visualSliderWidth;
    //        var otherInset = (vSlider && hSlider) ? inset : 0;
    //        var hRangeSliderRect = new Rect(drawRect.x + 1, drawRect.yMax - inset, drawRect.width - otherInset, styles.sliderWidth);

    //        var p1 = new Vector2(x, hRangeSliderRect.yMin);
    //        var p2 = new Vector2(x, hRangeSliderRect.yMax);

    //        var lineRect = Rect.MinMaxRect(p1.x - 0.5f, p1.y, p2.x + 0.5f, p2.y);
    //        EditorGUI.DrawRect(lineRect, c);
    //    }

    //    static BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
    //    private float DoFloatField(Rect position, Rect dragHotZone, int id, float value, bool draggable) {
    //        Type type = typeof(EditorGUI);
    //        MethodInfo doFloatField = type.GetMethod("DoFloatField", flags);
    //        object editor = ReflectionUtility.GetField<EditorGUI>("s_RecycledEditor", null);
    //        object formatString = ReflectionUtility.GetField<EditorGUI>("kFloatFieldFormatString", null);
    //        return (float)(doFloatField.Invoke(null, new object[] { editor, position, dragHotZone, id, value, formatString, EditorStyles.numberField, draggable }));
    //    }
    //    private int DoIntField(Rect position, Rect dragHotZone, int id, int value, bool draggable, float dragSensitivity) {
    //        Type type = typeof(EditorGUI);
    //        MethodInfo doIntField = type.GetMethod("DoIntField", flags);
    //        object editor = ReflectionUtility.GetField<EditorGUI>("s_RecycledEditor", null);
    //        object formatString = ReflectionUtility.GetField<EditorGUI>("kIntFieldFormatString", null);
    //        return (int)(doIntField.Invoke(null, new object[] { editor, position, dragHotZone, id, value, formatString, EditorStyles.numberField, draggable, dragSensitivity }));
    //    }
    //    private string DoTextField(int id, Rect position, string text, string allowedellters, out bool changed, bool reset, bool multiline, bool passwordField) {
    //        Type type = typeof(EditorGUI);
    //        MethodInfo doTextField = type.GetMethod("DoTextField", flags);
    //        object editor = ReflectionUtility.GetField<EditorGUI>("s_RecycledEditor", null);
    //        changed = false;
    //        object[] args = new object[] { editor, id, position, text, EditorStyles.numberField, allowedellters, changed, reset, multiline, passwordField };
    //        string result = (string)(doTextField.Invoke(null, args));
    //        changed = (bool)args[6];
    //        return result;
    //    }
    //}
    #endregion

    #region reflection timearea
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

            // Draw keys
            if(state.selection.clipIsEditable) {
                foreach(var track in state.activeAnimationAsset.Tracks) {
                    while(track.Enumerator.MoveNext()) {
                        var frame = track.Enumerator.Current as FrameDataBase;





                    }
                }
            }
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
    #endregion
}
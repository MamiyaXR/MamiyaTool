using UnityEngine;
using UnityEngine.Events;

namespace MFramework {
    public enum EWindowMode {
        DlgWin = 0,
        FullWin,
        StaticWim,
    }
    public enum EWindowLayer {
        Bottom = 0,
        Normal,
        Top,
    }
    public enum EWindowState {
        Open = 0,
        Close,
    }

    [RequireComponent(typeof(Canvas))]
    public sealed class Window : MonoBehaviour {
        [SerializeField, ReadOnly] internal int id;
        [SerializeField, ReadOnly] internal string path;
        [SerializeField, ReadOnly] private EWindowState state = EWindowState.Close;

        public Canvas canvas;

        [SerializeField, StringInList(typeof(MVCSettings), "WinGroups")]
        private string group = "Normal";
        [SerializeField] private EWindowLayer layer = EWindowLayer.Normal;
        [SerializeField, Range(0, 255), Tooltip("Sort index at the same WindowLayer")]
        private int sortOrder = 128;
        [SerializeField, FieldChange(nameof(OnModeChanged))]
        private EWindowMode mode = EWindowMode.FullWin;
        [SerializeField, EnumFlags]
        private EWindowMode hide = 0;

        [SerializeField, Range(0, 60), Tooltip("Destroy time after the window closed\n0: Never\n>0: Special time")]
        private int cacheTime = 30;
        [SerializeField] private int zPosition = 0;
        [SerializeField] private bool pop;
        [SerializeField] private bool mask;
        [SerializeField] private bool needBack;
        [SerializeField] private Animator anim;

        [SerializeField, TextArea, ReadOnly]
        private string intent;

        public int WinId => id;
        public string WinPath => path;
        public EWindowState State => state;
        public bool IsOpen => state == EWindowState.Open && winStack;
        public bool IsClose => state == EWindowState.Close;
        public bool IsClosed => IsClose && !gameObject.activeSelf;

        public string Group => group;
        public EWindowLayer Layer => layer;
        public int SortOrder => sortOrder;
        public EWindowMode Mode => mode;
        public EWindowMode Hide => hide;
        public int CacheTime => cacheTime;
        public int ZPosition => zPosition;
        public bool Pop => pop;
        public bool Mask => mask;

        public uint OpenIdx { get; private set; }
        public uint SortIdx => ((uint)layer << 28) & 0xf0000000 | ((uint)sortOrder << 20) & 0x0ff00000 | OpenIdx & 0x000fffff;
        public string Intent => intent;
        public string ModelResult { get; private set; }

        public bool NeedBack => needBack;

        public bool IsTop => winStack && winStack.TopWindow == this;

        public Window ParentWin { get; private set; }

        [SerializeField]
        public class VisibleChangeEvent : UnityEvent<bool> { }
        [HideInInspector] public UnityEvent onOpen = new UnityEvent();
        [HideInInspector] public VisibleChangeEvent onVisibleChanged = new VisibleChangeEvent();
        [HideInInspector] public UnityEvent onClose = new UnityEvent();
        [HideInInspector] public UnityEvent onClosed = new UnityEvent();
        [HideInInspector] public UnityEvent onLayerChange = new UnityEvent();

        //internal IResRequester resRequester;
        internal WindowStack winStack;
        internal bool tagDontCloseByPopWin;
        internal float closedTime;
        private static uint gOpenIdx = 0;
        private static int ghOpen = Animator.StringToHash("open");
        private static int ghClose = Animator.StringToHash("close");
        /******************************************************************
         * 
         *      lifecycle
         * 
         ******************************************************************/
        private void Awake() {
            if(!canvas)
                canvas = GetComponent<Canvas>();
            if(anim)
                anim.enabled = false;
        }
        private void OnDestroy() {
            //resRequester?.Dispose();
        }
        private void OnValidate() {
            canvas = GetComponent<Canvas>();
            anim = GetComponent<Animator>();
        }
        /******************************************************************
         * 
         *      public method
         * 
         ******************************************************************/
        public T GetIntent<T>() {
            if(!string.IsNullOrEmpty(intent))
                return JsonUtility.FromJson<T>(intent);
            return default;
        }
        public void SetModelResult(string value) {
            Debug.Assert(!string.IsNullOrEmpty(value), "ModalResult can't be null or empty");
            ModelResult = value;
            CloseSelf();
        }
        [ContextMenu("BringToFront")]
        public void BringToFront() {
            OpenIdx = ++gOpenIdx;
            if(winStack)
                winStack.UpdateWindows();
        }
        [ContextMenu("SendToBottom")]
        public void SendToBottom() {
            OpenIdx = 0;
            if(winStack)
                winStack.UpdateWindows();
        }
        [ContextMenu("CloseSelf")]
        public void CloseSelf() {
            //UIService.CloseWindow(this);
        }
        public void CloseSelfDelay(float time) {
            CancelInvoke(nameof(CloseSelf));
            Invoke(nameof(CloseSelf), time);
        }
        /******************************************************************
         * 
         *      private method
         * 
         ******************************************************************/
        private bool PlayWinAnim(int state) {
            if(anim && anim.runtimeAnimatorController && anim.HasState(0, state)) {
                anim.enabled = true;
                anim.Play(state, 0);
                return true;
            }
            return false;
        }
        private void OnWinClosed() {
            if(State != EWindowState.Close)
                return;
            try {
                onClosed?.Invoke();
            } catch(System.Exception e) {
                Debug.LogException(e);
            }
            gameObject.SetActive(false);
            winStack.RemoveWindow(this);
            intent = null;
        }
        internal void DoOpen(string intent) {
            this.intent = intent;

            OpenIdx = ++gOpenIdx;
            ModelResult = null;
            gameObject.SetActive(true);
            state = EWindowState.Open;
        }
        internal void DoOpened(bool openedAndVisible) {
            try {
                onOpen?.Invoke();
            } catch(System.Exception e) {
                Debug.LogException(e);
            }

            if(!openedAndVisible && anim) {
                CancelInvoke();
                PlayWinAnim(ghOpen);
            }
        }
        internal void ChangeLayer(int layerId, int order) {
            canvas.overrideSorting = true;
            canvas.sortingLayerID = layerId;
            canvas.sortingOrder = order;

            try {
                onLayerChange?.Invoke();
            } catch(System.Exception e) {
                Debug.LogException(e);
            }
        }
        internal void ChangeVisible(bool value) {
            if(gameObject.activeSelf == value)
                return;
            if(!value) {
                onVisibleChanged?.Invoke(false);
            }
            gameObject.SetActive(value);
            if(value) {
                onVisibleChanged?.Invoke(true);
            }
        }
        internal void DoClose() {
            if(State == EWindowState.Close)
                return;
            state = EWindowState.Close;
            closedTime = Time.time + 0.5f;
            try {
                onClose?.Invoke();
            } catch(System.Exception e) {
                Debug.LogException(e);
            }

            if(PlayWinAnim(ghClose)) {
                var info = anim.GetCurrentAnimatorClipInfo(0);
                if(info != null) {
                    closedTime = Time.time + info.Length + 0.5f;
                    Invoke(nameof(OnWinClosed), info.Length);
                } else {
                    OnWinClosed();
                }
            } else {
                OnWinClosed();
            }
        }
        internal void SetNeedBack() {
            needBack = true;
        }
        internal void ClearModelResult() {
            ModelResult = null;
        }
        private void OnModeChanged() {
            switch(mode) {
                case EWindowMode.DlgWin:
                    hide = 0;
                    break;
                case EWindowMode.FullWin:
                    hide = (EWindowMode)(1 << (int)EWindowMode.DlgWin | 1 << (int)EWindowMode.FullWin);
                    break;
                default:
                    hide = (EWindowMode)(1 << (int)EWindowMode.DlgWin);
                    break;
            }
        }
        internal void SetParentWin(Window value) {
            ParentWin = value;
        }
    }
}
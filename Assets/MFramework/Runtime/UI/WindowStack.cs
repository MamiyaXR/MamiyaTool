using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MFramework {
    public enum EWindowStackState {
        Open = 0,
        Close,
    }
    [RequireComponent(typeof(Canvas))]
    public class WindowStack : MonoBehaviour {
        public Canvas canvas;
        [StringInList(typeof(MVCSettings), "WinGroups")]
        public string group;
        [StringInList(typeof(MVCSettings), "WinGroups")]
        public string[] exclusiveWinGroups;
        public int layerOffset = 100;

        public GameObject uiMask;
        //public AnimPlayer apActive;
        //public AnimPlayer apDeactive;

        public Window MaskedWin { get; private set; }

        public bool isNormal => group == "Normal";
        [SerializeField]
        private List<Window> _winStack = new List<Window>();
        public Window TopWindow => _winStack.Count > 0 ? _winStack[_winStack.Count - 1] : null;
        public Window TopNormalWindow => _winStack.FindLast(x => x && x.Layer == EWindowLayer.Normal);

        private EWindowStackState _state = EWindowStackState.Open;
        private static int ghOpen = Animator.StringToHash("open");
        private static int ghClose = Animator.StringToHash("close");
        /******************************************************************
         * 
         *      lifecycle
         * 
         ******************************************************************/
        private void Awake() {
            if(uiMask)
                uiMask.SetActive(false);
        }
        private void OnValidate() {
            canvas = GetComponent<Canvas>();
        }
        /******************************************************************
         * 
         *      public method
         * 
         ******************************************************************/
        public void SetActive(bool value, bool imm) {
            if(value) {
                DoOpen(imm);
            } else {
                DoClose(imm);
            }
        }
        /******************************************************************
         * 
         *      private method
         * 
         ******************************************************************/
        private static int CompareWindow(Window l, Window r) => l.SortIdx.CompareTo(r.SortIdx);
        internal void UpdateWindows() {
            _winStack.RemoveAll(x => x == null);
            _winStack.Sort(CompareWindow);

            if(uiMask)
                uiMask.transform.SetAsLastSibling();

            int lastMask = 0;
            for(int i = _winStack.Count - 1; i >= 0; --i) {
                Window win = _winStack[i];
                win.transform.SetSiblingIndex(i);

                bool vsb = !((int)win.Mode).InMask(lastMask);
                if(win.ParentWin) {
                    vsb &= win.ParentWin.IsOpen;
                }
                win.ChangeVisible(vsb);
                lastMask |= (int)win.Hide;
            }

            var zPosition = 0;
            var cnt = 0;
            foreach(var win in _winStack) {
                if(win.gameObject.activeInHierarchy) {
                    cnt++;
                    win.ChangeLayer(canvas.sortingLayerID, canvas.sortingOrder + cnt * layerOffset);
                    if(zPosition != 0) {
                        var rt = (RectTransform)win.transform;
                        rt.anchoredPosition3D = new Vector3(0, 0, -zPosition);
                    }
                    zPosition += win.ZPosition;
                }
            }

            MaskedWin = null;
            if(uiMask) {
                bool masked = false;
                for(var i = _winStack.Count - 1; i >= 0; --i) {
                    var win = _winStack[i];
                    if(win.Mask && win.gameObject.activeInHierarchy) {
                        masked = true;
                        MaskedWin = win;

                        var idx = win.transform.GetSiblingIndex();
                        uiMask.transform.SetSiblingIndex(idx);
                        break;
                    }
                }
                uiMask.SetActive(masked);
            }
        }
        internal void OpenWindow(Window win, string intent) {
            _winStack.Remove(win);
            _winStack.Add(win);
            win.winStack = this;

            var openedAndVisible = win.IsOpen && win.gameObject.activeInHierarchy;
            win.DoOpen(intent);
            UpdateWindows();
            win.DoOpened(openedAndVisible);
        }
        internal void CloseWindow(Window win) {
            if(!win || win.IsClose)
                return;
            win.DoClose();
        }
        internal void CloseWinDir(Window win, bool isTop) {
            var index = _winStack.IndexOf(win);
            if(index < 0)
                return;
            var wins = isTop ? _winStack.GetRange(index + 1, _winStack.Count - index - 1) : _winStack.GetRange(0, index - 1);
            for(var i = wins.Count - 1; i >= 0; --i) {
                wins[i].CloseSelf();
            }
        }
        internal void RemoveWindow(Window win) {
            _winStack.Remove(win);
            UpdateWindows();
        }
        internal Window GetMaskedWin(GameObject go) {
            return go == uiMask ? MaskedWin : null;
        }
        internal void ClosePopWindows(Window[] clickedWins) {
            int idx = _winStack.FindLastIndex(x => x != null && x.IsOpen && x.Pop);
            if(idx < 0)
                return;

            Window popWin = _winStack[idx];
            if(popWin != null && popWin.IsOpen && !popWin.tagDontCloseByPopWin) {
                // 检测是否点击了上层的窗体
                for(int i = idx + 1; i < _winStack.Count; ++i) {
                    Window overWin = _winStack[i];
                    if(overWin && overWin.IsOpen && clickedWins.Contains(overWin)) {
                        return;
                    }
                }

                popWin.CloseSelf();
            }
        }
        internal void ClearWindows() {
            _winStack.Clear();
        }
        internal bool Back() {
            var lastBackWin = _winStack.FindLast(x => x && (x.NeedBack || x.Pop));
            if(lastBackWin != null) {
                lastBackWin.CloseSelf();
                return true;
            }

            return false;
        }
        private void DoOpen(bool imm) {
            _state = EWindowStackState.Open;

            gameObject.SetActive(true);
            //if(!imm && apActive) {
            //    apActive.Play();
            //} else {
            //    // todo: 需重置状态
            //}
        }
        private void OnCloseStack() {
            if(_state != EWindowStackState.Close)
                return;
            gameObject.SetActive(false);
        }
        private void DoClose(bool imm) {
            _state = EWindowStackState.Close;

            //if(!imm && apDeactive) {
            //    apDeactive.Play();
            //    Invoke(nameof(OnCloseStack), apDeactive.Duration);
            //} else {
            //    gameObject.SetActive(false);
            //}
            gameObject.SetActive(false);
        }
    }
}
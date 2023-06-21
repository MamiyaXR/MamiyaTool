using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace MamiyaTool {
    internal class FrameAnimationWindow : EditorWindow {
        public VisualElement Root => rootVisualElement;

        private AnimEditor m_AnimEditor;

        [SerializeField] private int m_LastSelectedObjectID;

        #region elements
        private ListView assetList;
        private ToolbarSearchField search;
        private VisualElement assetEditor;
        #endregion

        #region datas
        private Dictionary<string, FrameAnimationAsset> Anims {
            get {
                if(anims == null)
                    anims = new Dictionary<string, FrameAnimationAsset>();
                return anims;
            }
        }
        private Dictionary<string, FrameAnimationAsset> anims;

        private List<FrameAnimationAsset> AssetListSrc {
            get {
                if(assetListSrc == null)
                    assetListSrc = new List<FrameAnimationAsset>();
                return assetListSrc;
            }
        }
        private List<FrameAnimationAsset> assetListSrc;
        #endregion
        /*****************************************************************
         * 
         *      lifecycle
         * 
         *****************************************************************/
        public void CreateGUI() {
            InitRoot();
            InitSearchField();
            InitAssetList();
            InitAssetEditor();
        }
        private void OnEnable() {
            Undo.undoRedoPerformed += UndoRedoPerformed;
        }
        private void Update() {
            if(m_AnimEditor == null)
                return;
            m_AnimEditor.Update();
        }
        private void OnGUI() {
            DoOnGUI();
        }
        private void OnSelectionChange() {
            if(m_AnimEditor == null)
                return;

            Object activeObject = Selection.activeObject;

            if(activeObject is GameObject activeGameObject) {
                EditGameObject(activeGameObject);
            } else {
                if(activeObject is Transform activeTransform) {
                    EditGameObject(activeTransform.gameObject);
                }
            }
        }
        private void OnDisable() {
            Undo.undoRedoPerformed -= UndoRedoPerformed;
        }
        /*****************************************************************
         * 
         *      private method
         * 
         *****************************************************************/
        private T LoadAsset<T>(string guid) where T : Object {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            return AssetDatabase.LoadAssetAtPath<T>(path);
        }
        #region root
        private void InitRoot() {
            var guids = AssetDatabase.FindAssets(ROOT_UXML_NAME);
            if(guids == null || guids.Length <= 0)
                return;
            VisualTreeAsset rootPanel = null;
            foreach(var guid in guids) {
                rootPanel = LoadAsset<VisualTreeAsset>(guid);
                if(rootPanel != null)
                    break;
            }
            if(rootPanel == null)
                return;
            rootPanel.CloneTree(rootVisualElement);
        }
        #endregion
        #region search field
        private void InitSearchField() {
            search = Root.Q<ToolbarSearchField>(SEARCH_FIELD_NAME);
            if(search == null)
                return;
        }
        #endregion
        #region asset list
        private void InitAssetList() {
            assetList = Root.Q<ListView>(ASSET_LIST_NAME);
            if(assetList == null)
                return;

            assetList.makeItem += OnAssetListMakeItem;
            assetList.bindItem += OnAssetListBindItem;
            assetList.onSelectionChange += OnAssetListSelectChange;
            assetList.itemsSource = AssetListSrc;
        }
        private bool GetAnims() {
            bool changed = false;
            List<string> guids = new List<string>(AssetDatabase.FindAssets(ASSET_FILTER));
            foreach(var guid in guids) {
                if(Anims.ContainsKey(guid))
                    continue;

                Anims.Add(guid, LoadAsset<FrameAnimationAsset>(guid));
                changed = true;
            }
            List<string> removeList = new List<string>();
            foreach(var guid in Anims.Keys) {
                if(!guids.Contains(guid))
                    removeList.Add(guid);
            }
            foreach(var guid in removeList) {
                Anims.Remove(guid);
                changed = true;
            }
            return changed;
        }
        private VisualElement OnAssetListMakeItem() {
            Label result = new Label();
            result.style.unityTextAlign = TextAnchor.MiddleLeft;
            return result;
        }
        private void OnAssetListBindItem(VisualElement item, int idx) {
            Label itemContent = item as Label;
            if(idx >= 0 && idx < assetListSrc.Count) {
                itemContent.text = assetListSrc[idx].name;
            }
        }
        private void OnAssetListSelectChange(IEnumerable<object> items) {
            m_AnimEditor.state.activeAnimationAsset = assetList.selectedItem as FrameAnimationAsset;
        }
        #endregion
        #region asset editor
        private void InitAssetEditor() {
            assetEditor = Root.Q<VisualElement>(ASSET_EDITOR_PANEL_NAME);
            if(assetEditor == null)
                return;
            if(m_AnimEditor == null)
                m_AnimEditor = new AnimEditor();
            assetEditor.Add(new AssetEditorPanel(this, m_AnimEditor));
        }
        #endregion
        private void DoOnGUI() {
            if(assetList != null) {
                bool refreshAssetList = false;
                List<FrameAnimationAsset> tempSrc = null;
                GetAnims();
                tempSrc = Anims.Values.ToList();
                if(search != null) {
                    if(!string.IsNullOrEmpty(search.value)) {
                        tempSrc = tempSrc.Where((v) => v.name == search.value).ToList();
                    }
                }
                if(!FrameAnimWinUtility.CompareAssetList(tempSrc, assetListSrc)) {
                    refreshAssetList = true;
                    assetListSrc.Clear();
                    assetListSrc.AddRange(tempSrc);
                }
                if(refreshAssetList)
                    assetList.RefreshItems();
            }
        }
        private bool EditGameObject(GameObject gameObject) {
            if(EditorUtility.IsPersistent(gameObject))
                return false;

            if((gameObject.hideFlags & HideFlags.NotEditable) != 0)
                return false;

            m_LastSelectedObjectID = gameObject != null ? gameObject.GetInstanceID() : 0;
            m_AnimEditor.state.activeGameObject = gameObject;
            return true;
        }
        private void UndoRedoPerformed() {
            Repaint();
        }
        /*****************************************************************
         * 
         *      static method
         * 
         *****************************************************************/
        [MenuItem("Tools/FrameAnimationWindow")]
        public static void ShowExample() {
            FrameAnimationWindow wnd = GetWindow<FrameAnimationWindow>();
            wnd.titleContent = new GUIContent("FrameAnimationWindow");
        }
        /*****************************************************************
         * 
         *      string define
         * 
         *****************************************************************/
        private const string ROOT_UXML_NAME = "FrameAnimationWindow";
        private const string ASSET_FILTER = "t:FrameAnimationAsset";

        private const string ASSET_LIST_NAME = "AssetList";
        private const string SEARCH_FIELD_NAME = "SearchField";
        private const string ASSET_EDITOR_PANEL_NAME = "AssetEditorPanel";
    }
}
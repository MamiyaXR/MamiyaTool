using System.Collections.Generic;
using UnityEngine;

namespace MFramework {
    public interface IUpdateable {
        void Update();
    }
    public class Updateables {
        private List<IUpdateable> _list = new List<IUpdateable>();
        private List<IUpdateable> _adding = new List<IUpdateable>();
        private List<IUpdateable> _removed = new List<IUpdateable>();

        public int Count => _list.Count + _adding.Count;

        public void Update() {
            foreach(var updater in _removed)
                _list.Remove(updater);
            _list.AddRange(_adding);
            _removed.Clear();
            _adding.Clear();
            foreach(var updater in _list)
                updater.Update();
        }
        public void Add(IUpdateable updater) {
            _adding.Add(updater);
            _removed.Remove(updater);
        }
        public void Remove(IUpdateable updater) {
            _adding.Remove(updater);
            _removed.Add(updater);
        }
        public void Clear() {
            _list.Clear();
            _adding.Clear();
            _removed.Clear();
        }
    }

    [ExecuteInEditMode]
    public class UpdaterService : MonoBehaviour {
        private static UpdaterService _Inst;

        private static Updateables _Updaters = new Updateables();
        private static Updateables _LateUpdaters = new Updateables();
        private static Updateables _FixedUpdaters = new Updateables();

        private bool _AutoCreated;
        /******************************************************************
         * 
         *      lifecycle
         * 
         ******************************************************************/
        private void Awake() {
            _Inst = this;
        }
        private void Update() {
            _Updaters.Update();
#if UNITY_EDITOR
            CheckRelease();
#endif
        }
        private void LateUpdate() {
            _LateUpdaters.Update();
#if UNITY_EDITOR
            CheckRelease();
#endif
        }
        private void FixedUpdate() {
            _FixedUpdaters.Update();
#if UNITY_EDITOR
            CheckRelease();
#endif
        }
        private void OnDestroy() {
            _Updaters.Clear();
            _LateUpdaters.Clear();
            _FixedUpdaters.Clear();
        }
        /******************************************************************
         * 
         *      public method
         * 
         ******************************************************************/
        public static void AddUpdater(IUpdateable updater) {
#if UNITY_EDITOR
            CreateInst();
#endif
            _Updaters.Add(updater);
        }
        public static void RemoveUpdater(IUpdateable updater) {
            _Updaters.Remove(updater);
        }
        public static void AddLateUpdater(IUpdateable updater) {
#if UNITY_EDITOR
            CreateInst();
#endif
            _LateUpdaters.Add(updater);
        }
        public static void RemoveLateUpdater(IUpdateable updater) {
            _LateUpdaters.Remove(updater);
        }
        public static void AddFixedUpdater(IUpdateable updater) {
#if UNITY_EDITOR
            CreateInst();
#endif
            _FixedUpdaters.Add(updater);
        }
        public static void RemoveFixedUpdater(IUpdateable updater) {
            _FixedUpdaters.Remove(updater);
        }
        /******************************************************************
         * 
         *      private method
         * 
         ******************************************************************/
#if UNITY_EDITOR
        [UnityEditor.InitializeOnEnterPlayMode]
        private static void OnEnterPlay() {
            if(_Inst && _Inst._AutoCreated)
                Destroy(_Inst.gameObject);
        }
        private static void CreateInst() {
            if(Application.isPlaying || _Inst)
                return;
            var go = new GameObject("UpdaterService");
            _Inst = go.AddComponent<UpdaterService>();
            _Inst._AutoCreated = true;
        }
        private static void CheckRelease() {
            if(Application.isPlaying)
                return;
            if(_Inst && _Inst._AutoCreated && _Updaters.Count == 0 && _LateUpdaters.Count == 0 && _FixedUpdaters.Count == 0) {
                if(!UnityEditor.PrefabUtility.IsPartOfAnyPrefab(_Inst) &&
                    !UnityEditor.PrefabUtility.IsPartOfPrefabAsset(_Inst) &&
                    !UnityEditor.PrefabUtility.IsPartOfPrefabInstance(_Inst)) {
                    DestroyImmediate(_Inst.gameObject);
                }
            }
        }
        [ContextMenu("PrintInfo")]
        private static void PrintInfo() {
            Debug.Log(
                $"CreatedByEditor:{(_Inst != null ? _Inst._AutoCreated.ToString() : "Null")} " +
                $"Update:{_Updaters.Count} " +
                $"LateUpdate:{_LateUpdaters.Count} " +
                $"FixedUpdate:{_FixedUpdaters.Count}");
        }
#endif
    }
}
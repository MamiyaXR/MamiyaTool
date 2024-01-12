using System;
using System.Collections.Generic;

namespace MamiyaTool {
    #region IState
    public interface IState {
        bool Condition();
        void Enter();
        void Update();
        void FixedUpdate();
        void OnGUI();
        void Exit();
    }
    #endregion

    #region CustomState
    public class CustomState : IState {
        private Func<bool> mOnCondition;
        private Action mOnEnter;
        private Action mOnUpdate;
        private Action mOnFixedUpdate;
        private Action mOnGUI;
        private Action mOnExit;
        
        public CustomState OnCondition(Func<bool> onCondition) {
            mOnCondition = onCondition;
            return this;
        }
        public CustomState OnEnter(Action onEnter) {
            mOnEnter = onEnter;
            return this;
        }
        public CustomState OnUpdate(Action onUpdate) {
            mOnUpdate = onUpdate;
            return this;
        }
        public CustomState OnFixedUpdate(Action onFixedUpdate) {
            mOnFixedUpdate = onFixedUpdate;
            return this;
        }
        public CustomState OnGUI(Action onGUI) {
            mOnGUI = onGUI;
            return this;
        }
        public CustomState OnExit(Action onExit) {
            mOnExit = onExit;
            return this;
        }

        public bool Condition() {
            var result = mOnCondition?.Invoke();
            return result == null || result.Value;
        }
        public void Enter() {
            mOnEnter?.Invoke();
        }
        public void Update() {
            mOnUpdate?.Invoke();
        }
        public void FixedUpdate() {
            mOnFixedUpdate?.Invoke();
        }
        public void OnGUI() {
            mOnGUI?.Invoke();
        }
        public void Exit() {
            mOnExit?.Invoke();
        }
    }
    #endregion

    #region FSM
    public class FSM<T> {
        protected Dictionary<T, IState> mStates = new Dictionary<T, IState>();
        public T CurStateId => mCurStateId;
        private T mCurStateId;
        public IState CurState => mCurState;
        private IState mCurState;
        public T PreStateId { get; private set; }
        public long FrameCountOfCurState { get; private set; } = 0;
        private Action<T, T> mOnStateChanged = (_, __) => { };

        public void AddState(T id, IState state) {
            mStates.Add(id, state);
        }
        public CustomState State(T t) {
            if(mStates.ContainsKey(t))
                return mStates[t] as CustomState;

            CustomState state = new CustomState();
            mStates.Add(t, state);
            return state;
        }
        public void ChangeState(T t) {
            if(t.Equals(CurStateId))
                return;

            if(mStates.TryGetValue(t, out var state)) {
                if(mCurState != null && state.Condition()) {
                    mCurState.Exit();
                    PreStateId = mCurStateId;
                    mCurState = state;
                    mCurStateId = t;
                    mOnStateChanged?.Invoke(PreStateId, CurStateId);
                    FrameCountOfCurState = 0;
                    mCurState.Enter();
                }
            }
        }
        public void OnStateChanged(Action<T, T> onStateChanged) {
            mOnStateChanged += onStateChanged;
        }
        public void StartState(T t) {
            if(mStates.TryGetValue(t, out var state)) {
                PreStateId = t;
                mCurState = state;
                mCurStateId = t;
                FrameCountOfCurState = 0;
                mCurState.Enter();
            }
        }
        public void Update() {
            mCurState?.Update();
        }
        public void FixedUpdate() {
            mCurState?.FixedUpdate();
        }
        public void OnGUI() {
            mCurState?.OnGUI();
        }
        public void Clear() {
            mCurState = null;
            mCurStateId = default;
            mStates.Clear();
        }
    }
    #endregion

    #region AbstractState
    public abstract class AbstractState<TStateId, TTarget> : IState {
        protected FSM<TStateId> mFSM;
        protected TTarget mTarget;

        public AbstractState(FSM<TStateId> fsm, TTarget target) {
            mFSM = fsm;
            mTarget = target;
        }

        bool IState.Condition() {
            return OnCondition();
        }
        void IState.Enter() {
            OnEnter();
        }
        void IState.Update() {
            OnUpdate();
        }
        void IState.FixedUpdate() {
            OnFixedUpdate();
        }
        void IState.OnGUI() {
            OnGUI();
        }
        void IState.Exit() {
            OnExit();
        }

        protected virtual bool OnCondition() => true;
        protected virtual void OnEnter() { }
        protected virtual void OnUpdate() { }
        protected virtual void OnFixedUpdate() { }
        protected virtual void OnGUI() { }
        protected virtual void OnExit() { }
    }
    #endregion
}
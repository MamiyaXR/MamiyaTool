namespace MamiyaTool
{
    public enum EBehaviorStatus
    {
        /// <summary>
        /// 无效的
        /// </summary>
        Invalid = 0,
        /// <summary>
        /// 成功
        /// </summary>
        Success,
        /// <summary>
        /// 运行中
        /// </summary>
        Running,
        /// <summary>
        /// 执行返回失败
        /// </summary>
        Failure,
        /// <summary>
        /// 中断
        /// </summary>
        Aborted,
    }
    public interface IBehavior
    {
        /// <summary>
        /// 在 Update 调用之前调用一次
        /// </summary>
        void OnInitialize();
        /// <summary>
        /// 在每次行为树更新时被调用且仅被调用一次，直到返回状态表示该状态已停止
        /// </summary>
        EBehaviorStatus Update();
        /// <summary>
        /// 当刚刚更新的行为不在处于运行状态时，立即调用一次
        /// </summary>
        void OnTerminate(EBehaviorStatus status);
    }
}
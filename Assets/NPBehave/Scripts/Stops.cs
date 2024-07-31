namespace NPBehave
{
    public enum Stops
    {
        /// <summary>
        /// 永远不会停止任何正在运行的节点
        /// </summary>
        NONE,
        /// <summary>
        /// 不满足时停止自身
        /// </summary>
        SELF,
        /// <summary>
        /// 满足时将停止比此结点优先级较低的节点
        /// </summary>
        LOWER_PRIORITY,
        /// <summary>
        /// 不满足时将同时停止自身和优先级较低的节点
        /// </summary>
        BOTH,
        /// <summary>
        /// 满足时，它将停止优先级较低的节点
        /// </summary>
        IMMEDIATE_RESTART,
        /// <summary>
        /// 满足时，它将停止优先级较低的节点(基本不用)
        /// </summary>
        LOWER_PRIORITY_IMMEDIATE_RESTART
    }
}
namespace MamiyaTool
{
    public class Behavior : IBehavior
    {
        public BTDatabase database { get; set; }
        public EBehaviorStatus status { get; set; }
        public bool IsTerminated {
            get {
                return status == EBehaviorStatus.Success || status == EBehaviorStatus.Failure;
            }
        }
        public bool IsRunning {
            get {
                return status == EBehaviorStatus.Running;
            }
        }
        /******************************************************************
         * 
         * 
         * 
         ******************************************************************/
        public Behavior()
        {
            status = EBehaviorStatus.Invalid;
        }
        public EBehaviorStatus Tick()
        {
            if(status != EBehaviorStatus.Running)
                OnInitialize();
            status = Update();
            if(status != EBehaviorStatus.Running)
                OnTerminate(status);
            return status;
        }
        public void Reset()
        {
            status = EBehaviorStatus.Invalid;
        }
        public void Abort()
        {
            OnTerminate(EBehaviorStatus.Aborted);
            status = EBehaviorStatus.Aborted;
        }
        /******************************************************************
         * 
         *      override
         * 
         ******************************************************************/
        public virtual void OnInitialize() { }
        public virtual void OnTerminate(EBehaviorStatus status) { }
        public virtual EBehaviorStatus Update() { return default; }
    }
}
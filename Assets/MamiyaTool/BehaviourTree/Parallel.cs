namespace MamiyaTool
{
    public class Parallel : Composite
    {
        /// <summary>
        /// 策略
        /// </summary>
        public enum Policy
        {
            RequireOne = 0,
            RequireAll,
        }

        protected Policy successPolicy;
        protected Policy failurePolicy;
        /******************************************************************
         * 
         * 
         * 
         ******************************************************************/
        public Parallel(Policy success, Policy failure)
        {
            successPolicy = success;
            failurePolicy = failure;
        }
        public override EBehaviorStatus Update()
        {
            int successCount = 0;
            int failureCount = 0;

            for(int i = 0; i < children.Count; i++) {
                var child = children[i];
                if(!child.IsTerminated)
                    child.Tick();
                if(child.status == EBehaviorStatus.Success) {
                    successCount++;
                    if(successPolicy == Policy.RequireOne)
                        return EBehaviorStatus.Success;
                }
                if(child.status == EBehaviorStatus.Failure) {
                    failureCount++;
                    if(failurePolicy == Policy.RequireOne)
                        return EBehaviorStatus.Failure;
                }
            }

            if(failurePolicy == Policy.RequireAll && failureCount == children.Count)
                return EBehaviorStatus.Failure;
            if(successPolicy == Policy.RequireAll && successCount == children.Count)
                return EBehaviorStatus.Success;

            return EBehaviorStatus.Failure;
        }
        public override void OnTerminate(EBehaviorStatus status)
        {
            base.OnTerminate(status);
            children.ForEach(child => {
                if(child.IsRunning)
                    child.Abort();
            });
        }
    }
}
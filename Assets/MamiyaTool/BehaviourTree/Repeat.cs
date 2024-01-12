namespace MamiyaTool
{
    public class Repeat : Decorator
    {
        private readonly int limit;
        private int counter = 0;
        /******************************************************************
         * 
         * 
         * 
         ******************************************************************/
        public Repeat(Behavior child, int limit) : base(child)
        {
            this.limit = limit;
        }
        public override void OnInitialize()
        {
            base.OnInitialize();
            counter = 0;
        }
        public override EBehaviorStatus Update()
        {
            while(true) {
                child.Tick();
                if(child.status == EBehaviorStatus.Running)
                    break;
                if(child.status == EBehaviorStatus.Failure)
                    return EBehaviorStatus.Failure;
                if(++counter == limit)
                    return EBehaviorStatus.Success;
                child.Reset();
            }
            return EBehaviorStatus.Invalid;
        }
    }
}
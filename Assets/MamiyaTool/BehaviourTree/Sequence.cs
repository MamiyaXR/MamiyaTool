namespace MamiyaTool
{
    public class Sequence : Composite
    {
        private int curIndex = 0;
        /******************************************************************
         * 
         * 
         * 
         ******************************************************************/
        public override void OnInitialize()
        {
            base.OnInitialize();
            curIndex = 0;
        }
        public override EBehaviorStatus Update()
        {
            while(true) {
                var child = children[curIndex];
                var status = child.Tick();
                if(status != EBehaviorStatus.Success)
                    return status;
                if(++curIndex == children.Count)
                    return EBehaviorStatus.Success;
            }
        }
        public override void OnTerminate(EBehaviorStatus status)
        {
            base.OnTerminate(status);
        }
    }
}
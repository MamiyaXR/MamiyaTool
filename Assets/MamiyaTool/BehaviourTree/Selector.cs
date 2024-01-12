namespace MamiyaTool
{
    public class Selector : Composite
    {
        protected int curIndex;
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
                if(status != EBehaviorStatus.Failure)
                    return status;
                if(++curIndex == children.Count)
                    return EBehaviorStatus.Failure;
            }
        }
    }
}
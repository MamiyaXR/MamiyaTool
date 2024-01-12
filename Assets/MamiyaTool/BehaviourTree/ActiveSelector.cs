namespace MamiyaTool
{
    public class ActiveSelector : Selector
    {
        public override EBehaviorStatus Update()
        {
            var prev = curIndex;
            base.OnInitialize();
            EBehaviorStatus result = base.Update();
            if(prev != children.Count && curIndex != prev)
                children[prev].OnTerminate(EBehaviorStatus.Aborted);
            return result;
        }
    }
}
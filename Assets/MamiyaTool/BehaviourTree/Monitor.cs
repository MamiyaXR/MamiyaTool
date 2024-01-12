namespace MamiyaTool
{
    public class Monitor : Parallel
    {
        public Monitor(Policy success, Policy failure) : base(success, failure) { }
        public Monitor() : base(Policy.RequireAll, Policy.RequireAll) { }
        public void AddCondition(BTCondition condition)
        {
            if(condition != null)
                children.Insert(0, condition);
        }
        public void AddAction(BTAction action)
        {
            if(action != null)
                children.Add(action);
        }
    }
}
namespace MamiyaTool
{
    public class Filter : Sequence
    {
        public void AddCondition(BTCondition condition)
        {
            if(condition != null)
                children.Insert(0, condition);
        }
        public void AddAction(Behavior action)
        {
            if(action != null)
                children.Add(action);
        }
    }
}
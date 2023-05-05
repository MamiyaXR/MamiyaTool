using System.Collections.Generic;

namespace MamiyaTool
{
    public class Composite : Behavior
    {
        protected List<Behavior> children = new List<Behavior>();
        /******************************************************************
         * 
         * 
         * 
         ******************************************************************/
        public override void OnInitialize()
        {
            base.OnInitialize();
        }
        public override void OnTerminate(EBehaviorStatus status)
        {
            base.OnTerminate(status);
        }
        public void AddChild(Behavior behavior)
        {
            if(behavior != null)
                children.Add(behavior);
        }
        public void RemoveChild(Behavior behavior)
        {
            children.Remove(behavior);
        }
        public void ClearChild()
        {
            children.Clear();
        }
    }
}
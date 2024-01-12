using System;

namespace MamiyaTool
{
    public class BTAction : Behavior
    {
        public BTAction() : base() { }
    }

    public class DoAction : BTAction
    {
        private readonly Action<BTDatabase> action;

        public DoAction(Action<BTDatabase> action) : base()
        {
            this.action = action;
        }

        public override EBehaviorStatus Update()
        {
            action?.Invoke(database);
            return EBehaviorStatus.Success;
        }
    }
}
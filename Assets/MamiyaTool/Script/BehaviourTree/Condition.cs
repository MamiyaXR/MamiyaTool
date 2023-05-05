using System;

namespace MamiyaTool
{
    public class BTCondition : Behavior
    {
        public BTCondition() : base() { }
    }

    public class FuncBTCondition : BTCondition
    {
        private readonly Func<BTDatabase, bool> match;

        public FuncBTCondition(Func<BTDatabase, bool> match) : base()
        {
            this.match = match;
        }

        public override EBehaviorStatus Update()
        {
            if(match(database))
                return EBehaviorStatus.Success;
            return EBehaviorStatus.Failure;
        }
    }
}
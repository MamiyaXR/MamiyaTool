using System;
using static MamiyaTool.Parallel;

namespace MamiyaTool
{
    public class BehaviorTreeBuilder
    {
        public BehaviorTree behaviorTree { get; private set; }
        /******************************************************************
         * 
         *      public method
         * 
         ******************************************************************/
        public BehaviorTreeBuilder()
        {
            behaviorTree = new BehaviorTree();
        }
        public BehaviorTree SetRoot(Behavior root)
        {
            if(behaviorTree.root == null)
                behaviorTree.root = root;
            return behaviorTree;
        }
        public ActiveSelector CreateActiveSelector()
        {
            ActiveSelector activeSelector = new ActiveSelector();
            SetBTDatabase(activeSelector);
            return activeSelector;
        }
        public Selector CreateSelector()
        {
            Selector selector = new Selector();
            SetBTDatabase(selector);
            return selector;
        }
        public Decorator CreateDecorator(Behavior child)
        {
            Decorator decorator = new Decorator(child);
            SetBTDatabase(decorator);
            return decorator;
        }
        public Sequence CreateSequence()
        {
            Sequence sequence = new Sequence();
            SetBTDatabase(sequence);
            return sequence;
        }
        public Filter CreateFilter()
        {
            Filter filter = new Filter();
            SetBTDatabase(filter);
            return filter;
        }
        public Parallel CreateParallel(Policy success = Policy.RequireAll, Policy failure = Policy.RequireOne)
        {
            Parallel parallel = new Parallel(success, failure);
            SetBTDatabase(parallel);
            return parallel;
        }
        public Monitor CreateMonitor(Policy success = Policy.RequireAll, Policy failure = Policy.RequireOne)
        {
            Monitor monitor = new Monitor(success, failure);
            SetBTDatabase(monitor);
            return monitor;
        }
        public Repeat CreateRepeat(Behavior child, int limit)
        {
            Repeat repeat = new Repeat(child, limit);
            SetBTDatabase(repeat);
            return repeat;
        }
        public BTCondition CreateCondition<T>() where T : BTCondition, new()
        {
            BTCondition condition = new T();
            SetBTDatabase(condition);
            return condition;
        }
        public BTCondition CreateCondition(Func<BTDatabase, bool> match)
        {
            FuncBTCondition condition = new FuncBTCondition(match);
            SetBTDatabase(condition);
            return condition;
        }
        public BTCondition CreateCondition(bool match)
        {
            return CreateCondition((blackboard) => match);
        }
        public BTAction CreateAction<T>() where T : BTAction, new()
        {
            BTAction action = new T();
            SetBTDatabase(action);
            return action;
        }
        public BTAction CreateAction(Action<BTDatabase> doAction)
        {
            DoAction action = new DoAction(doAction);
            SetBTDatabase(action);
            return action;
        }
        /******************************************************************
         * 
         *      private method
         * 
         ******************************************************************/
        private void SetBTDatabase(Behavior behavior)
        {
            behavior.database = behaviorTree.database;
        }
    }
}
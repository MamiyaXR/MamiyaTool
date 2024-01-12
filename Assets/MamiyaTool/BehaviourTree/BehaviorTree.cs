using System;
using UnityEngine;

namespace MamiyaTool
{
    public class BehaviorTree
    {
        public Behavior root { get; set; }
        public BTDatabase database { get; private set; }
        private bool isLog;
        /******************************************************************
         * 
         * 
         * 
         ******************************************************************/
        public BehaviorTree()
        {
            database = new BTDatabase();
        }
        public BehaviorTree(Behavior root) : base()
        {
            this.root = root;
        }
        public void Tick()
        {
            if(root == null)
                throw new Exception("MamiyaTool Exception : Root can not be null in BehaviourTree");

            if(root.IsTerminated) {
                if(isLog) {
                    isLog = false;
#if UNITY_EDITOR
                    Debug.Log($"行为树执行完成,状态为{root.status}");
#endif
                    return;
                }
            }
            isLog = true;
            root.Tick();
        }
    }
}
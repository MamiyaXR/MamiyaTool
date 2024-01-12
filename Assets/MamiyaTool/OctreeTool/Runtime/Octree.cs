using System;
using System.Collections.Generic;
using UnityEngine;

namespace MamiyaTool {
    [Serializable]
    public class Octree {
        public OctreeNode root { get; private set; }
        /********************************************************
         * 
         *      lifecycle
         * 
         ********************************************************/
        public Octree(GameObject[] objs, float minSize) {
            List<BoundObject> bounds = ExtractBounds(objs);
            Bounds bound = new Bounds();
            foreach(BoundObject b in bounds)
                bound.Encapsulate(b.bound);
            float maxSize = Mathf.Max(bound.size.x, bound.size.y, bound.size.z);
            Vector3 sizeVector = new Vector3(maxSize, maxSize, maxSize) * 0.5f;
            bound.SetMinMax(bound.center - sizeVector, bound.center + sizeVector);
            root = new OctreeNode(bound, minSize);
            foreach(BoundObject b in bounds)
                root.AddObject(b);
        }
        /********************************************************
         * 
         *      private method
         * 
         ********************************************************/
        private List<BoundObject> ExtractBounds(GameObject[] objs) {
            List<BoundObject> r = new List<BoundObject>();
            if(objs != null) {
                foreach(GameObject obj in objs) {
                    Collider col = obj.GetComponent<Collider>();
                    if(col != null)
                        r.Add(new BoundObject(obj, col.bounds));
                }
            }
            return r;
        }
    }
}
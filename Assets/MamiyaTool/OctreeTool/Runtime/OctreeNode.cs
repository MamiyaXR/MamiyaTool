using System.Collections.Generic;
using UnityEngine;

namespace MamiyaTool {
    public class OctreeNode {
        public List<BoundObject> sources { get; private set; } = new List<BoundObject>();
        public List<OctreeNode> children { get; private set; } = new List<OctreeNode>();

        private Bounds bounds;
        private float minSize;
        private Bounds[] childBounds;
        /********************************************************
         * 
         *      lifecycle
         * 
         ********************************************************/
        public OctreeNode(Bounds bounds, float minSize) {
            this.bounds = bounds;
            this.minSize = minSize;
            float quarter = bounds.size.x / 4f;
            Vector3 childSize = bounds.size / 2f;
            childBounds = new Bounds[L];
            for(int i = 0; i < L; ++i) {
                childBounds[i] = new Bounds(bounds.center + centerOffset[i] * quarter, childSize);
                children.Add(null);
            }
        }
        /********************************************************
         * 
         *      public method
         * 
         ********************************************************/
        /// <summary>
        /// 判断所在的区域里有没有 GameObject 并将其放入该区域的树枝中
        /// </summary>
        /// <param name="obj"></param>
        public void AddObject(BoundObject obj) {
            if(bounds.size.x < minSize) {
                sources.Add(obj);
                return;
            }
            for(int i = 0; i < L; ++i) {
                if(childBounds[i].Intersects(obj.bound)) {
                    if(children == null)
                        children = new List<OctreeNode>();
                    if(children[i] == null)
                        children[i] = new OctreeNode(childBounds[i], minSize);
                    children[i].AddObject(obj);
                }
            }
        }
        /// <summary>
        /// 绘制方法
        /// </summary>
        /// <param name="col"></param>
        public void Draw(Color col, Color colMark) {
            Color finalCol = sources.Count > 0 ? colMark : col;
            Gizmos.color = finalCol;
            if(sources.Count > 0) {
                Gizmos.DrawWireCube(bounds.center, bounds.size);
            }
            foreach(OctreeNode child in children)
                child?.Draw(col, colMark);
        }
        /********************************************************
         * 
         *      define
         * 
         ********************************************************/
        private const int L = 8;
        private static readonly Vector3[] centerOffset = new Vector3[L] {
            new Vector3(-1, 1, -1),
            new Vector3(-1, 1, 1),
            new Vector3(1, 1, 1),
            new Vector3(1, 1, -1),
            new Vector3(-1, -1, -1),
            new Vector3(-1, -1, 1),
            new Vector3(1, -1, 1),
            new Vector3(1, -1, -1),
        };
    }
    public struct BoundObject {
        public GameObject src;
        public Bounds bound;
        public BoundObject(GameObject src, Bounds bound) {
            this.src = src;
            this.bound = bound;
        }
    }
}
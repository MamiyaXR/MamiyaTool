using System.Collections.Generic;
using UnityEngine;

namespace MamiyaTool {
    public class CreateOctree : MonoBehaviour {
        [SerializeField] private List<GameObject> objs = new List<GameObject>();
        [SerializeField] private float minSize = 1f;
        [SerializeField] private Color col = Color.green;
        [SerializeField] private Color colMark = Color.red;

        private Octree octree;
        /********************************************************
         * 
         *      lifecycle
         * 
         ********************************************************/
        private void Update() {
            octree = new Octree(objs.ToArray(), minSize);
        }
        private void OnDrawGizmos() {
            if(octree == null || octree.root == null)
                return;
            octree.root.Draw(col, colMark);
        }
    }
}
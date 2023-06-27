using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace MamiyaTool {
    public static class BoxTool {
        /// <summary>
        /// 获得物体最小包围盒
        /// </summary>
        /// <param name="target">目标物体</param>
        /// <returns>包围盒</returns>
        public static Bounds GetBox(GameObject target) {
            Bounds bounds = new Bounds();
            if(target != null) {
                Renderer[] mfs = target.GetComponentsInChildren<Renderer>();
                if(mfs != null) {
                    foreach(var mf in mfs)
                        bounds = Encapsulate(bounds, mf.bounds);
                }
            }
            return bounds;
        }
        /// <summary>
        /// 获得多个物体的最小包围盒
        /// </summary>
        /// <param name="targets">目标物体</param>
        /// <returns>包围盒</returns>
        public static Bounds GetMultiObjBox(GameObject[] targets) {
            Bounds bounds = new Bounds();
            if(targets != null) {
                foreach(var target in targets) {
                    bounds = Encapsulate(bounds, GetBox(target));
                }
            }
            return bounds;
        }
        /// <summary>
        /// 为目标物体添加 BoxCollider
        /// </summary>
        /// <param name="target">目标物体</param>
        public static void AddBoxCollider(GameObject target) {
            Vector3 pos = target.transform.localPosition;
            Quaternion qt = target.transform.localRotation;
            Vector3 ls = target.transform.localScale;

            target.transform.position = Vector3.zero;
            target.transform.eulerAngles = Vector3.zero;
            target.transform.localScale = Vector3.one;
            
            Bounds itemBound = GetBox(target);

            target.transform.localPosition = pos;
            target.transform.localRotation = qt;
            target.transform.localScale = ls;

            var col = target.GetOrAddComponent<BoxCollider>();
            col.size = itemBound.size;
            col.center = itemBound.center;
        }
        /// <summary>
        /// 获得全部 GameObject
        /// </summary>
        /// <param name="src">目标物体</param>
        /// <returns>全部 GameObject</returns>
        public static GameObject[] GetAllGameObjectSingle(GameObject src) {
            if(src == null)
                return null;
            List<GameObject> result = new List<GameObject> { src };
            if(src.transform.childCount != 0) {
                for(int i = 0; i < src.transform.childCount; ++i) {
                    result.AddRange(GetAllGameObject(src.transform.GetChild(i).gameObject));
                }
            }
            return result.ToArray();
        }
        /// <summary>
        /// 获得全部 GameObject
        /// </summary>
        /// <param name="src">目标物体</param>
        /// <returns>全部 GameObject</returns>
        public static GameObject[] GetAllGameObject(params GameObject[] src) {
            if(src == null)
                return null;
            List<GameObject> result = new List<GameObject>();
            foreach(var s in src) {
                result.AddRange(GetAllGameObjectSingle(s));
            }
            return result.ToArray();
        }
        private static Bounds Encapsulate(Bounds b1, Bounds b2) {
            bool condition1 = IsInValid(b1);
            bool condition2 = IsInValid(b2);
            if(condition1 && !condition2)
                return b2;
            if(!condition1 && condition2)
                return b1;
            if(!condition1 && !condition2) {
                b1.Encapsulate(b2);
                return b1;
            }
            return invalid;
        }
        private static bool IsInValid(Bounds target) {
            return target.Equals(invalid);
        }
        private static Bounds invalid = new Bounds();
    }
}
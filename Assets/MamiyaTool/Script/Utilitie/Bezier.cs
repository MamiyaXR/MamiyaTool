using UnityEngine;

namespace MamiyaTool {
    /// <summary>
    /// 贝塞尔曲线
    /// </summary>
    public class Bezier {

        public static float pointCount = 15f;

        /// <summary>
        /// 获取二阶贝塞尔曲线路径数组
        /// </summary>
        /// <param name="startPos">开始位置</param>
        /// <param name="controlPos">控制位置</param>
        /// <param name="endPos">结束位置</param>
        /// <returns></returns>
        public static Vector3[] Bezier2Path(Vector3 startPos, Vector3 controlPos, Vector3 endPos) {
            Vector3[] path = new Vector3[(int)pointCount + 1];
            for(int i = 0; i <= pointCount; i++) {
                float t = i / pointCount;
                path[i] = Bezier2(startPos, controlPos, endPos, t);
            }
            return path;
        }
        public static Vector2[] Bezier2Path(Vector2 startPos, Vector2 controlPos, Vector2 endPos) {
            Vector2[] path = new Vector2[(int)pointCount + 1];
            for(int i = 0; i <= pointCount; ++i) {
                float t = i / pointCount;
                path[i] = Bezier2(startPos, controlPos, endPos, t);
            }
            return path;
        }

        /// <summary>
        /// 获取三阶贝塞尔曲线路径数组
        /// </summary>
        /// <param name="startPos"></param>
        /// <param name="controlPos1"></param>
        /// <param name="controlPos2"></param>
        /// <param name="endPos"></param>
        /// <returns></returns>
        public static Vector3[] Bezier3Path(Vector3 startPos, Vector3 controlPos1, Vector3 controlPos2, Vector3 endPos) {
            Vector3[] path = new Vector3[(int)pointCount];
            for(int i = 0; i <= pointCount; i++) {
                float t = i / pointCount;
                path[i] = Bezier3(startPos, controlPos1, controlPos2, endPos, t);
            }
            return path;
        }
        public static Vector2[] Bezier3Path(Vector2 startPos, Vector2 controlPos1, Vector2 controlPos2, Vector2 endPos) {
            Vector2[] path = new Vector2[(int)pointCount];
            for(int i = 0; i <= pointCount; ++i) {
                float t = i / pointCount;
                path[i] = Bezier3(startPos, controlPos1, controlPos2, endPos, t);
            }
            return path;
        }

        // 2阶贝塞尔曲线
        private static Vector3 Bezier2(Vector3 startPos, Vector3 controlPos, Vector3 endPos, float t) {
            return (1 - t) * (1 - t) * startPos + 2 * t * (1 - t) * controlPos + t * t * endPos;
        }
        private static Vector2 Bezier2(Vector2 startPos, Vector2 controlPos, Vector2 endPos, float t) {
            return (1 - t) * (1 - t) * startPos + 2 * t * (1 - t) * controlPos + t * t * endPos;
        }

        // 3阶贝塞尔曲线
        private static Vector3 Bezier3(Vector3 startPos, Vector3 controlPos1, Vector3 controlPos2, Vector3 endPos, float t) {
            float t2 = 1 - t;
            return t2 * t2 * t2 * startPos
                + 3 * t * t2 * t2 * controlPos1
                + 3 * t * t * t2 * controlPos2
                + t * t * t * endPos;
        }
        private static Vector2 Bezier2(Vector2 startPos, Vector2 controlPos1, Vector2 controlPos2, Vector2 endPos, float t) {
            float t2 = 1 - t;
            return t2 * t2 * t2 * startPos
                + 3 * t * t2 * t2 * controlPos1
                + 3 * t * t * t2 * controlPos2
                + t * t * t * endPos;
        }
    }
}
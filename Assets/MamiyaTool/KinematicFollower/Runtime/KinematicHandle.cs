using System;
using UnityEngine;

namespace MamiyaTool {
    [Serializable]
    public struct KinematicHandle {
        public float f;
        public float z;
        public float r;

        public float k1 { get => z / Mathf.PI / f; }
        public float k2 { get => 1 / Mathf.Pow(2f * Mathf.PI * f, 2f); }
        public float K3 { get => r * z / (2f * Mathf.PI * f); }
        /******************************************************************
         *
         *
         *
         ******************************************************************/
        public KinematicHandle(float f, float z, float r) {
            this.f = f;
            this.z = z;
            this.r = r;
        }
        public (float, float) Invoke(float yPre, float dyPre, float x, float xPre , float delta) {
            if(f == 0f)
                return (x, dyPre);
            float k2_stable = Mathf.Max(k2, delta * delta / 2f + delta * k1 / 2f, delta * k1);
            float y = yPre + dyPre * delta;
            float dx = (x - xPre) / delta;
            float a = (x + K3 * dx - y - k1 * dyPre) / k2_stable;
            float dy = dyPre + a * delta;
            return (y, dy);
        }
    }
}
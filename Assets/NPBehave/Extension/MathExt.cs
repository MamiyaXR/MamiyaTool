using System;

namespace NPBehave {
    public class MathExt {
        public static float Random() {
            var seed = Guid.NewGuid().GetHashCode();
            System.Random r = new System.Random(seed);
            int i = r.Next(0, 100000);
            return (float)i / 100000;
        }
    }
}
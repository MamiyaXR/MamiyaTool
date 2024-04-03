using UnityEngine;

namespace MFramework {
    public static class MathExt {
        public static bool In(this int v, int min, int max) {
            return v >= min && v <= max;
        }
        public static bool IN(this float v, float min, float max) {
            return v >= min && v <= max;
        }
        public static bool InMask(this int layer, int mask) {
            return (1 << layer & mask) != 0;
        }
        public static bool OutMask(this int layer, int mask) {
            return (1 << layer & mask) == 0;
        }
        public static string BytesToStr(int bytes) {
            if(bytes <= 0)
                return "0B";
            if(bytes < 1024)
                return $"{bytes}B";
            if(bytes < 1024 * 1024)
                return $"{bytes / 1024}K";
            if(bytes < 1024 * 1024 * 1024)
                return $"{bytes / 1024 / 1024}M";
            return $"{bytes / 1024 / 1024 / 1024}T";
        }
        public static int Trunc(this float v) => Mathf.FloorToInt(v);
        public static int Round(this float v) => Mathf.RoundToInt(v);
        public static long Clamp(long v, long min, long max) {
            return v < min ? min : v > max ? max : v;
        }
        public static double Clamp(double v, double min, double max) {
            return v < min ? min : v > max ? max : v;
        }
        public static long Min(params long[] vs) {
            long min = long.MaxValue;
            foreach(var v in vs) {
                if(v < min)
                    min = v;
            }
            return min;
        }
        public static long Max(params long[] vs) {
            long max = long.MinValue;
            foreach(var v in vs) {
                if(v > max)
                    max = v;
            }
            return max;
        }
        public static double Min(params double[] vs) {
            double min = double.MaxValue;
            foreach(var v in vs) {
                if(v < min)
                    min = v;
            }
            return min;
        }
        public static double Max(params double[] vs) {
            double max = double.MinValue;
            foreach(var v in vs) {
                if(v > max)
                    max = v;
            }
            return max;
        }
        public static Rect Subtract(this Rect rect, Rect other) {
            var x = rect.x + other.x;
            var y = rect.y + other.y;
            var width = rect.width - other.width;
            var height = rect.height - other.height;
            return new Rect(x, y, width, height);
        }
    }
}
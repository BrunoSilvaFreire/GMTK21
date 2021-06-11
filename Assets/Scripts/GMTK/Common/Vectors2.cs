using Lunari.Tsuki.Misc;
using UnityEngine;

namespace GMTK.Common {
    public static class Vectors2 {
        public static Vector2 Center(Vector2 a, Vector2 b) {
            return a + (b - a) / 2;
        }

        public static bool Contains(this Bounds2D bounds, Vector2 pos) {
            var x = pos.x;
            var y = pos.y;
            var min = bounds.Min;
            var max = bounds.Max;
            return x >= min.x || x <= max.x || y >= min.y || y <= max.y;
        }
    }
}
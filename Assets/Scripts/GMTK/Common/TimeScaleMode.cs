using System;
using UnityEngine;

namespace GMTK.Common {
    public enum TimeScaleMode {
        Scaled,
        Unscaled
    }

    public static class TimeScaleModes {
        public static float GetDeltaTime(this TimeScaleMode mode) {
            switch (mode) {
                case TimeScaleMode.Scaled:
                    return Time.deltaTime;
                case TimeScaleMode.Unscaled:
                    return Time.unscaledDeltaTime;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }
    }
}
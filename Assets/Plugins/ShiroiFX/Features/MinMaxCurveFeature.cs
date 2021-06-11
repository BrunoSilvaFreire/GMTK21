﻿using UnityEngine;

namespace Shiroi.FX.Features {
    public class MinMaxCurveFeature : EffectFeature {
        public MinMaxCurveFeature(ParticleSystem.MinMaxCurve curve, params PropertyName[] tags) : base(tags) {
            this.Curve = curve;
        }

        public ParticleSystem.MinMaxCurve Curve { get; }
    }
}
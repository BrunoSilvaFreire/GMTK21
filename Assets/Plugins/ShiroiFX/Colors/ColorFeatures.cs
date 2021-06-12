using Lunari.Tsuki.Runtime;
using Shiroi.FX.Features;
using UnityEngine;

namespace Shiroi.FX.Colors {
    public sealed class ColorFeature : EffectFeature {
        public ColorFeature(Color color, params PropertyName[] tags) : base(tags) {
            this.Color = color;
        }

        public Color Color { get; }
    }

    public sealed class GradientFeature : EffectFeature {
        public GradientFeature(Gradient gradient, params PropertyName[] tags) : base(tags) {
            this.Gradient = gradient;
        }

        public Gradient Gradient { get; }
    }

    public sealed class MinMaxColorFeature : EffectFeature {
        public MinMaxColorFeature(Color colorMin, Color colorMax, params PropertyName[] tags) : base(tags) {
            this.ColorMin = colorMin;
            this.ColorMax = colorMax;
        }

        public Color ColorMin { get; }

        public Color ColorMax { get; }

        public Color Color => Color.Lerp(ColorMin, ColorMax, Random.value);
    }

    public sealed class RandomColorFeature : EffectFeature {
        public RandomColorFeature(Color[] colors, params PropertyName[] tags) : base(tags) {
            this.Colors = colors;
        }

        public Color[] Colors { get; }

        public Color Color => Colors.RandomElement();
    }

    public sealed class MinMaxGradientFeature : EffectFeature {
        public MinMaxGradientFeature(Gradient minGradient, Gradient maxGradient, params PropertyName[] tags) :
            base(tags) {
            this.MinGradient = minGradient;
            this.MaxGradient = maxGradient;
        }

        public Gradient MinGradient { get; }

        public Gradient MaxGradient { get; }

        public Color GetColor(float time) {
            var a = MinGradient.Evaluate(time);
            var b = MaxGradient.Evaluate(time);
            return Color.Lerp(a, b, Random.value);
        }
    }
}
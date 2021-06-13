using Lunari.Tsuki.Runtime;
using Shiroi.FX.Features;
using UnityEngine;

namespace Shiroi.FX.Effects.BuiltIn {
    public class SpawnEffect : Effect {
        public GameObject obj;
        public override void Play(EffectContext context) {
            var position = context.GetRequiredFeature<PositionFeature>().Position;
            obj.Clone(position);
        }
    }
}
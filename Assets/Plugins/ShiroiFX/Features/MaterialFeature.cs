using UnityEngine;

namespace Shiroi.FX.Features {
    public class MaterialFeature : EffectFeature {
        public MaterialFeature(Material material, params PropertyName[] tags) : base(tags) {
            this.Material = material;
        }

        public Material Material { get; }
    }
}
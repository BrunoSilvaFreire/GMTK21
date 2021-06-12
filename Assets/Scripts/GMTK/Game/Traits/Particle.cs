using Lunari.Tsuki.Entities;
using UnityEngine;

namespace GMTK.Game.Traits {
    public class Particle : Trait {

        private Rigidbody rb;

        public new Rigidbody rigidbody => rb;

        public override void Configure(TraitDescriptor descriptor) {
            descriptor.RequiresComponent(out rb);
            descriptor.RequiresComponent<Collider>();
        }
    }
}
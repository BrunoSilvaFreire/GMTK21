using GMTK.Game;
using Lunari.Tsuki.Entities;
using UnityEngine;

namespace GMTK.Master {
    public class PlayerHUD : Trait {
        public override void Configure(TraitDescriptor descriptor) {
            if (descriptor.DependsOn(out NeutronAttractor attractor)) {
            }
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using Lunari.Tsuki.Entities;
using UnityEngine;
using UnityEngine.PlayerLoop;


namespace GMTK.Game.Traits {
    public class Neutron : Trait {

        private Rigidbody rb;
        
        public override void Configure(TraitDescriptor descriptor) {
            descriptor.RequiresComponent(out rb);
        }

        private void Start() {
            Player.Instance.Pawn.GetTrait<NeutronAttractor>().onAttract.AddListener(OnAttract);
        }

        private void OnDisable() {
            var player = Player.Instance; //cringe unity
            if (player) {
                player.Pawn.GetTrait<NeutronAttractor>().onAttract.RemoveListener(OnAttract);    
            }
        }

        private void OnAttract(Vector3 force) {
            rb.AddForce(force, ForceMode.Impulse);
        }
    }
}
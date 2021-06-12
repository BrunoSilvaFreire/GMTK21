using System;
using System.Collections;
using System.Collections.Generic;
using Lunari.Tsuki.Entities;
using UnityEngine;
using UnityEngine.PlayerLoop;


namespace GMTK.Game.Traits {
    public class Neutron : Particle {
        public static readonly List<Neutron> AllNeutrons = new();
        
        private void OnEnable() {
            AllNeutrons.Add(this);
        }
        private void Start() {
            Player.Instance.Pawn.GetTrait<NeutronAttractor>().onAttract.AddListener(OnAttract);
        }

        private void OnDisable() {
            AllNeutrons.Remove(this);
            var player = Player.Instance; //cringe unity
            if (player) {
                player.Pawn.GetTrait<NeutronAttractor>().onAttract.RemoveListener(OnAttract);    
            }
        }

        private void OnAttract(Vector3 force) {
            rigidbody.AddForce(force, ForceMode.Impulse);
        }
    }
}
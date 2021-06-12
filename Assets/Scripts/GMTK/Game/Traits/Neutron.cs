using System;
using System.Collections;
using System.Collections.Generic;
using Lunari.Tsuki.Entities;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;


namespace GMTK.Game.Traits {
    public class Neutron : Particle {
        public static readonly List<Neutron> AllNeutrons = new List<Neutron>();

        [SerializeField, Range(0f, 1f)] private float randomDirectionTendency = 0.4f;
        
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
            var randomDirection = Random.insideUnitCircle * force.magnitude;
            var direction = Vector3.Lerp(force, randomDirection, randomDirectionTendency);
            rigidbody.AddForce(direction, ForceMode.Impulse);
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Lunari.Tsuki.Entities;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;


namespace GMTK.Game.Traits {
    public class Neutron : Particle {
        public static readonly List<Neutron> AllNeutrons = new List<Neutron>();

        [SerializeField, Range(0f, 1f)]
        private float randomDirectionTendency = 0.4f;

        public float lifeTime = 5f;
        public bool immortal;

        private void OnEnable() {
            AllNeutrons.Add(this);
        }

        private void Start() {
            var attractor = Player.Instance.Pawn.GetTrait<NeutronAttractor>();
            attractor.onAttractDirection.AddListener(OnAttractDirection);
            attractor.onAttractPosition.AddListener(OnAttractPosition);

            if (!immortal) {
                transform.DOPunchScale(Vector3.one * 0.1f, 0.1f).OnComplete(() => {
                    transform.DOScale(Vector3.zero, 0.5f).OnComplete(() => {
                        if (AllNeutrons.Count > 1) {
                            Destroy(gameObject);
                        }
                    });
                }).SetDelay(lifeTime);
            }
        }

        private void OnDestroy() {
            transform.DOKill();
            AllNeutrons.Remove(this);
        }

        private void OnDisable() {
            AllNeutrons.Remove(this);
            var player = Player.Instance; //cringe unity
            if (player) {
                var attractor = Player.Instance.Pawn.GetTrait<NeutronAttractor>();
                attractor.onAttractDirection.RemoveListener(OnAttractDirection);
                attractor.onAttractPosition.RemoveListener(OnAttractPosition);
            }
        }

        private void OnAttractDirection(Vector3 force) {
            var randomDirection = Random.insideUnitCircle * force.magnitude;
            var direction = Vector3.Lerp(force, randomDirection, randomDirectionTendency);
            rigidbody.AddForce(direction, ForceMode.Impulse);
        }

        private void OnAttractPosition(Vector3 position) {
            rigidbody.AddForce(position - transform.position, ForceMode.Acceleration);
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Lunari.Tsuki.Entities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;


namespace GMTK.Game.Traits {
    public class Neutron : Particle {
        public static readonly List<Neutron> AllNeutrons = new List<Neutron>();
        public static readonly UnityEvent OnNumNeutronsChanged = new UnityEvent();
        [SerializeField, Range(0f, 1f)]
        private float randomDirectionTendency = 0.4f;

        public float lifeTime = 5f;
        public bool immortal;

        private void OnEnable() {
            AllNeutrons.Add(this);
            OnNumNeutronsChanged.Invoke();
        }

        private void Start() {
            var attractor = Player.Instance.Pawn.GetTrait<NeutronAttractor>();
            attractor.onAttractDirection.AddListener(OnAttractDirection);
            attractor.onAttractPosition.AddListener(OnAttractPosition);
            Invoke(nameof(DestroyNeutron), lifeTime); //hehehehe
            // Bruno: Do we REAAALLYY need this?
        }

        private void DestroyNeutron() {
            if (AllNeutrons.Count > 1) {
                AllNeutrons.Remove(this);
                OnNumNeutronsChanged.Invoke();
                transform.DOPunchScale(Vector3.one * 0.4f, 0.1f).OnComplete(() => {
                    transform.DOScale(Vector3.zero, 0.5f).OnComplete(() => { Destroy(gameObject); });
                });
            }
        }

        private void OnDestroy() {
            transform.DOKill();
        }

        private void OnDisable() {
            AllNeutrons.Remove(this);
            OnNumNeutronsChanged.Invoke();
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
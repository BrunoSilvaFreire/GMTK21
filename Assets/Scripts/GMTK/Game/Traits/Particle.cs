using System;
using DG.Tweening;
using Lunari.Tsuki.Entities;
using UnityEngine;

namespace GMTK.Game.Traits {
    public class Particle : Trait {
        private const string SpawnedParameter = "Spawned";
        protected const string DiedParameter = "Died";

        private Rigidbody rb;
        public Animator animator;
        private static readonly int Spawned = Animator.StringToHash(SpawnedParameter);

        public new Rigidbody rigidbody => rb;

        public override void Configure(TraitDescriptor descriptor) {
            descriptor.RequiresComponent(out rb);
            descriptor.RequiresComponent<Collider>();
            descriptor.RequiresComponent(out animator);
            descriptor.RequiresAnimatorParameter(SpawnedParameter, AnimatorControllerParameterType.Trigger);
            descriptor.RequiresAnimatorParameter(DiedParameter, AnimatorControllerParameterType.Trigger);
        }

        protected virtual void Start() {
            if (animator) {
                animator.SetTrigger(Spawned);
            }
        }
    }
}
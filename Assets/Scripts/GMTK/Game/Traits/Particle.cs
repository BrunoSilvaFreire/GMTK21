using System;
using DG.Tweening;
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

        private void Start() {
            var targetScale = transform.localScale;
            transform.localScale = Vector3.zero;
            transform.DOScale(targetScale, 0.2f).SetEase(Ease.OutElastic);
        }

        private void OnDestroy() {
            transform.DOKill();
        }
    }
}
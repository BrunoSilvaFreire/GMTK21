using System;
using System.Collections.Generic;
using GMTK.Common;
using Lunari.Tsuki.Entities;
using Shiroi.FX.Effects;
using Shiroi.FX.Features;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GMTK.Combat {
    [TraitLocation(TraitLocations.Combat)]
    public class Corrosive : Trait, IDamageSource {
        public uint damage = 3;
        public CollisionMode mode = CollisionMode.Collision;
        public UnityDamageEvent onDamage;

        [ShowIf(nameof(IsManualQuery))] public Collider2D query;

        [ShowIf(nameof(IsManualQuery))] public LayerMask queryMask;

        public Effect onDamageEffect;

        [NonSerialized] public List<Collider2D> manualQueryWhiteList = new();

        private Combatant personalCombatant;

        private void Start() {
            if (Owner != null) personalCombatant = Owner.GetTrait<Combatant>();
        }

        private void FixedUpdate() {
            if (!IsManualQuery()) return;

            var filter = new ContactFilter2D {
                layerMask = queryMask,
                useLayerMask = true,
                useTriggers = false
            };
            var elements = new Collider2D[4];
            var overlaps = query.OverlapCollider(filter, elements);
            for (var i = 0; i < overlaps; i++) {
                var found = elements[i];
                if (manualQueryWhiteList.Contains(found)) continue;

                TryDamage(found);
            }

            manualQueryWhiteList.Clear();
            for (var i = 0; i < overlaps; i++) manualQueryWhiteList.Add(elements[i]);
        }

        private void OnCollisionEnter2D(Collision2D other) {
            if (mode.HasFlag(CollisionMode.Collision)) TryDamage(other.collider);
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (mode.HasFlag(CollisionMode.Trigger)) TryDamage(other);
        }

        private bool IsManualQuery() {
            return mode.HasFlag(CollisionMode.ManualQuery);
        }

        private void TryDamage(Collider2D other) {
            if (Owner.Access(out Living own))
                if (own.Dead)
                    return;

            var e = other.GetComponentInParent<Entity>();
            if (e == null) return;

            if (!e.Access(out Living l)) return;

            if (personalCombatant != null)
                if (e.Access(out Combatant enemy))
                    if (!personalCombatant.CanAttack(enemy))
                        return;

            var result = l.Damage(new Damage(this, damage));
            onDamage.Invoke(result);
            onDamageEffect.PlayIfPresent(e, features: new EffectFeature[] {
            });
        }
    }
}
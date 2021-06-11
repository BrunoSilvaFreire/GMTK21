using System;
using UnityEngine;
using UnityEngine.Events;

namespace GMTK.Combat {
    [Serializable]
    public class UnityDamageEvent : UnityEvent<DamageEvent> {
    }

    [Serializable]
    public class DamageEvent {
        public DamageEvent(Damage damage, Living target) {
            this.Damage = damage;
            this.Target = target;
            Value = damage.Original;
        }

        public uint HealthAfter => Math.Max(0, Target.Health - Value);

        public uint Value { get; set; }

        public Damage Damage { get; }

        public Living Target { get; }
    }

    [Serializable]
    public struct Damage {
        [SerializeField] private uint original;

        public Damage(
            IDamageSource source,
            uint amount
        ) {
            this.Source = source;
            original = amount;
        }

        [field: SerializeReference] public IDamageSource Source { get; }

        public uint Original => original;
    }

    public interface IDamageSource {
    }

    public interface IDefendable : IDamageSource {
        float Defend(Combatant defendant);
        float Counter(Combatant defendant);
    }
}
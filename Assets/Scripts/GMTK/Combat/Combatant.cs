using System;
using JetBrains.Annotations;
using Lunari.Tsuki.Entities;
using Lunari.Tsuki.Runtime.Stacking;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace GMTK.Combat {
    [TraitLocation(TraitLocations.Combat)]
    public class Combatant : Trait {
        public enum Alignment {
            Ally,
            Neutral,
            Enemy
        }

        public const uint MidGameAttackDamageReference = 15;
        public static UnityEvent<Combatant> onAlignmentChanged = new();

        private static readonly int Attack = Animator.StringToHash("Attack");

        public Attack[] attacks;

        [SerializeField] private Alignment currentAlignment;

        public Animator animator;
        public FloatStackable damageMultiplier;
        public AttackEvent onAttackPerformed;

        [SerializeField] public bool attackAllowed = true;

        [ShowInInspector]
        public Alignment CurrentAlignment {
            get => currentAlignment;
            set {
                if (currentAlignment == value) return;

                currentAlignment = value;
                onAlignmentChanged.Invoke(this);
            }
        }

        private void Start() {
            attacks = Owner.GetComponentsInChildren<Attack>();
            foreach (var attack in attacks) attack.Setup(this);
        }

        [UsedImplicitly]
        public void SetAlignment(int alignmentIndex) {
            CurrentAlignment = (Alignment) alignmentIndex;
        }

        public void PerformAttack(Attack attack) {
            onAttackPerformed.Invoke(attack);
            attack.Execute(this);
        }

        public void PerformAnimatorAttack() {
            if (attackAllowed) animator.SetTrigger(Attack);
        }

        public bool CanAttack(Living other) {
            if (Owner == other.Owner) return false;

            if (other.Owner.Access(out Combatant combatant) && CurrentAlignment != Alignment.Neutral)
                if (!CanAttack(combatant))
                    return false;

            return true;
        }

        public bool CanAttack(Combatant other) {
            if (Owner == other.Owner) return false;

            if (CurrentAlignment == Alignment.Neutral) return true;

            if (other.CurrentAlignment == Alignment.Neutral) return true;

            return CurrentAlignment != other.CurrentAlignment;
        }

        //Do not remove, used in animators
        public void SetAttackAllowed(bool value) {
            attackAllowed = value;
        }

        public bool IsEnemy(Combatant combatant) {
            switch (currentAlignment) {
                case Alignment.Ally:
                    return combatant.currentAlignment == Alignment.Enemy;
                case Alignment.Neutral:
                    return false;
                case Alignment.Enemy:
                    return combatant.currentAlignment == Alignment.Ally;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
using System;
using GMTK.Common;
using UnityEngine.Events;

namespace GMTK.Combat {
    [Serializable]
    public class AttackEvent : UnityEvent<Attack> {
    }

    public abstract class Attack : Setupable<Combatant> {
        protected Combatant combatant;

        public override void Setup(Combatant obj) {
            combatant = obj;
        }


        public abstract void Execute(Combatant combatant);
    }
}
using System.Collections;
using System.Collections.Generic;
using Lunari.Tsuki.Entities;
using Lunari.Tsuki.Runtime;
using UnityEngine;

namespace GMTK.Game.Traits {
    public abstract class Atom : Particle {
        
        public LayerMask neutronMask;
        
        private void OnCollisionEnter(Collision collision) {
            if ((neutronMask.value & (1 << collision.gameObject.layer)) != 0) {
                OnCollisionWithNeutron(collision);
            }
        }
        
        protected abstract void OnCollisionWithNeutron(Collision collision);

        public void Delete() {
            Destroy(gameObject);
        }
    }
}
using UnityEngine;

namespace GMTK.Game.Traits {
    public class AngryAtom : Atom {
        protected override void OnCollisionWithNeutron(Collision collision) {
            Destroy(collision.rigidbody.gameObject);
        }
    }
}
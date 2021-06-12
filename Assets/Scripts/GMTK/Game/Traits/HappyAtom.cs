using System;
using Lunari.Tsuki.Runtime;
using UnityEngine;

namespace GMTK.Game.Traits {
    public class HappyAtom : Atom {

        [SerializeField] private Neutron neutronPrefab;
        [SerializeField] private AngryAtom angryAtom;

        public int neutronsReleasedOnCollision = 3;
        public int angryAtomsReleasedOnCollision = 2;

        private Vector3 Vector3FromAngle(float angle) {
            return new Vector3(Mathf.Cos(angle), Mathf.Sin(angle));
        }

        private float AngleFromIndex(int index, int totalIndexes) {
            return (float)index / totalIndexes * Mathf.PI * 2;
        }
        
        protected override void OnCollisionWithNeutron(Collision collision) {
            
            int totalChildren = neutronsReleasedOnCollision + angryAtomsReleasedOnCollision;
            int childIndex = 0;
            float angleOffset = Vector3.Angle(collision.contacts[0].normal, Vector3.right);
            float impulse = collision.impulse.magnitude;

            for (int i = 0; i < neutronsReleasedOnCollision; i++) {
                var direction = Vector3FromAngle(AngleFromIndex(childIndex, totalChildren) + angleOffset);
                var neutron = neutronPrefab.Clone(transform.position + direction * 4);
                neutron.rigidbody.AddForce(direction * impulse, ForceMode.Impulse);
                childIndex++;
            }

            for (int i = 0; i < angryAtomsReleasedOnCollision; i++) {
                var direction = Vector3FromAngle(AngleFromIndex(childIndex, totalChildren) + angleOffset);
                var angryAtoms = angryAtom.Clone(transform.position + direction * 4);
                angryAtoms.rigidbody.AddForce(direction * impulse, ForceMode.Impulse);
                childIndex++;
            }
            Destroy(gameObject);
            Destroy(collision.rigidbody.gameObject);
        }
    }
}
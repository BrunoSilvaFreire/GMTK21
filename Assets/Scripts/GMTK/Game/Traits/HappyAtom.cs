using System;
using System.Collections.Generic;
using Lunari.Tsuki.Runtime;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GMTK.Game.Traits {
    public class HappyAtom : Atom {
        
        public static readonly List<HappyAtom> AllHappyAtoms = new List<HappyAtom>();

        [SerializeField] private Neutron neutronPrefab;
        [SerializeField] private AngryAtom angryAtom;

        [Range(0f, 1f)]
        public float additionalNeutronChance = 0.5f;
        public int baseNeutronCount = 3;
        public int angryAtomsCount = 2;

        private Vector3 Vector3FromAngle(float angle) {
            return new Vector3(Mathf.Cos(angle), Mathf.Sin(angle));
        }

        private float AngleFromIndex(int index, int totalIndexes) {
            return (float)index / totalIndexes * Mathf.PI * 2;
        }
        
        protected override void OnCollisionWithNeutron(Collision collision) {
            
            var neutronCount = baseNeutronCount;
            if (Random.value < additionalNeutronChance) {
                neutronCount++;
            }
            
            int totalChildren = neutronCount + angryAtomsCount;
            int childIndex = 0;
            float angleOffset = Vector3.Angle(collision.contacts[0].normal, Vector3.right);
            float impulse = collision.impulse.magnitude;

            for (int i = 0; i < neutronCount; i++) {
                var direction = Vector3FromAngle(AngleFromIndex(childIndex, totalChildren) + angleOffset);
                var neutron = neutronPrefab.Clone(transform.position + direction * 2);
                neutron.rigidbody.AddForce(direction * impulse, ForceMode.Impulse);
                childIndex++;
            }

            for (int i = 0; i < angryAtomsCount; i++) {
                var direction = Vector3FromAngle(AngleFromIndex(childIndex, totalChildren) + angleOffset);
                var angryAtoms = angryAtom.Clone(transform.position + direction * 2);
                angryAtoms.rigidbody.AddForce(direction * impulse, ForceMode.Impulse);
                childIndex++;
            }
            Destroy(gameObject);
            Destroy(collision.rigidbody.gameObject);
        }
    }
}
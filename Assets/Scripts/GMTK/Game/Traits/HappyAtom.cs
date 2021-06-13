using System;
using System.Collections;
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
        private const int AngryAtomsCount = 2;

        [Range(0f, 90f)]
        public float neutronArc = 30f;
        public float neutronForce = 4f;
        public float angryAtomForce = 4f;
        public float velocityToDetonate = 20.0f;

        public float scaryRadius = 15f;
        
        public bool isScared { get; private set; }

        private Collider[] closeNeutrons;

        private Vector3 Vector3FromAngle(float angle) {
            return new Vector3(Mathf.Cos(angle), Mathf.Sin(angle));
        }

        private float AngleFromIndex(int index, int totalIndexes) {
            return Mathf.Deg2Rad * Mathf.Lerp(-neutronArc, neutronArc, (float)index / totalIndexes);
        }

        protected override void Start() {
            base.Start();
            StartCoroutine(CheckCloseNeutrons());
        }
        
        private IEnumerator CheckCloseNeutrons() {
            while (gameObject.activeInHierarchy) {
                isScared = Physics.OverlapSphereNonAlloc(transform.position, scaryRadius, closeNeutrons, 1 << neutronMask.value) > 0;
                yield return new WaitForSeconds(0.25f);
            }
        }

        protected override void OnCollisionWithNeutron(Collision collision) {

            if (collision.relativeVelocity.magnitude < velocityToDetonate) {
                return;
            }

            var normal = collision.contacts[0].normal;
            var rightToNormal = Vector3.Cross(normal, Vector3.forward);
            
            var neutronCount = baseNeutronCount;
            if (Random.value < additionalNeutronChance) {
                neutronCount++;
            }
            
            int childIndex = 0;
            float angleOffset = Mathf.Atan2(normal.y, normal.x);

            for (int i = 0; i < neutronCount; i++) {
                var direction = Vector3FromAngle(AngleFromIndex(childIndex, Mathf.Max(neutronCount - 1, 1)) + angleOffset);
                var neutron = neutronPrefab.Clone(transform.position + direction * 2);
                neutron.rigidbody.AddForce(direction * neutronForce, ForceMode.Impulse);
                childIndex++;
            }

            for (int i = 0; i < AngryAtomsCount; i++) {
                var direction = i % 2 == 0 ? rightToNormal : -rightToNormal;
                var angryAtoms = angryAtom.Clone(transform.position + direction * 2);
                angryAtoms.rigidbody.AddForce(direction * angryAtomForce, ForceMode.Impulse);
            }

            AtomSpawner.Instance.OnHappyAtomDestroyed(neutronCount);
            
            Destroy(gameObject);
            Destroy(collision.rigidbody.gameObject);
        }
    }
}
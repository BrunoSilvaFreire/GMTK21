using System.Collections;
using System.Collections.Generic;
using GMTK.Game.Traits;
using Lunari.Tsuki.Runtime;
using Lunari.Tsuki.Runtime.Singletons;
#if UNITY_EDITOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;
using UnityEngine.Events;

namespace GMTK.Game {
    public class AtomSpawner : Singleton<AtomSpawner> {

        [System.Serializable]
        public struct Phase {
            public AnimationCurve spawnCurve;
            public int neutronCountToNextPhase;
        }
        
        public int atomsToSpawnPerCollision = 3;
        
        public bool spawning = true;

        public HappyAtom happyAtomPrefab;

        public new Camera camera;
        
        public List<Phase> phases;

        [Range(0f, 1f)]
        public float spawnDirectionBias = 0.5f;

        public float maximumSpawnInterval = 1f;

        public Phase CurrentPhase => phases[CurrentPhaseIndex];
#if UNITY_EDITOR
        [ShowInInspector]
#endif
        public int CurrentPhaseIndex { get; private set; } = 0;

        public UnityEvent<int> onPhaseChanged;

        private int atomsToSpawn = 0;
        private int neutronCounter = 0;
        private Vector3 currentDirection;
        private bool neutronMoved = false;

        public int AtomCounter { get; private set; } = 0;

        private void Start() {
            StartCoroutine(SpawnAtoms());
        }
        
        public void OnHappyAtomDestroyed(int neutronsCreated) {
            neutronCounter += neutronsCreated;
            atomsToSpawn += atomsToSpawnPerCollision;
        }

        public void OnNeutronMoved(Vector3 direction) {
            currentDirection = direction;
            neutronMoved = true;
        }

        public void SetCurrentPhase(int index) {
            neutronCounter = 0;
            onPhaseChanged.Invoke(index);
            CurrentPhaseIndex = index;
        }

        private IEnumerator SpawnAtoms() {
            float nextSpawn = 0;
            while (spawning) {
                float timeout = Time.time + maximumSpawnInterval;
                yield return new WaitWhile(() => (atomsToSpawn <= 0 || Time.time < nextSpawn) && (Time.time < timeout || !neutronMoved));
                
                var worldPosition = camera.transform.position + Vector3.Lerp(Random.insideUnitCircle * 30, currentDirection, spawnDirectionBias);
                worldPosition.z = 0;
                happyAtomPrefab.Clone(worldPosition);
                AtomCounter++;
                nextSpawn = Time.time + CurrentPhase.spawnCurve.Evaluate((float)neutronCounter / CurrentPhase.neutronCountToNextPhase);
                if (neutronCounter > CurrentPhase.neutronCountToNextPhase) {
                    if (CurrentPhaseIndex < phases.Count - 1) {
                        SetCurrentPhase(CurrentPhaseIndex + 1);    
                    }
                }
                atomsToSpawn--;
            }
        } 
    }
}
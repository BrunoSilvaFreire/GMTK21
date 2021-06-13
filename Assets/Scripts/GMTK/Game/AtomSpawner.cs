﻿using System.Collections;
using System.Collections.Generic;
using GMTK.Game.Traits;
using Lunari.Tsuki.Runtime;
using Lunari.Tsuki.Runtime.Singletons;
using Sirenix.OdinInspector;
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

        public Phase CurrentPhase => phases[CurrentPhaseIndex];
        
        [ShowInInspector]
        public int CurrentPhaseIndex { get; private set; } = 0;

        public UnityEvent<int> onPhaseChanged;

        private int atomsToSpawn = 0;
        private int neutronCounter = 0;
        private Vector3 currentDirection;

        private void Start() {
            StartCoroutine(SpawnAtoms());
        }
        
        public void OnHappyAtomDestroyed(int neutronsCreated) {
            neutronCounter += neutronsCreated;
        }

        public void OnNeutronMoved(Vector3 direction) {
            atomsToSpawn += atomsToSpawnPerCollision;
            currentDirection = direction;
        }

        public void SetCurrentPhase(int index) {
            neutronCounter = 0;
            onPhaseChanged.Invoke(index);
            CurrentPhaseIndex = index;
        }

        private IEnumerator SpawnAtoms() {
            float nextSpawn = 0;
            while (spawning) {
                
                yield return new WaitWhile(() => atomsToSpawn <= 0 || Time.time < nextSpawn);
                
                var worldPosition = camera.transform.position + Vector3.Lerp(Random.insideUnitCircle * 30, currentDirection, spawnDirectionBias);
                worldPosition.z = 0;
                happyAtomPrefab.Clone(worldPosition);
                
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
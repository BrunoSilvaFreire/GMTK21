using System.Collections;
using GMTK.Game.Traits;
using Lunari.Tsuki.Runtime;
using Lunari.Tsuki.Runtime.Singletons;
using UnityEngine;

namespace GMTK.Game {
    public class AtomSpawner : Singleton<AtomSpawner> {
        
        public int atomsToSpawnPerCollision = 3;
        
        public bool spawning = true;

        public HappyAtom happyAtomPrefab;

        public new Camera camera;

        public float spawnInterval = 0.25f;
        
        private int atomsToSpawn = 0;

        private void Start() {
            StartCoroutine(SpawnAtoms());
        }
        
        public void OnHappyAtomDestroyed() {
            atomsToSpawn += atomsToSpawnPerCollision;
        }

        private IEnumerator SpawnAtoms() {
            float nextSpawn = 0;
            while (spawning) {
                
                yield return new WaitWhile(() => atomsToSpawn <= 0 || Time.time < nextSpawn);

                var screenPosition = Random.insideUnitCircle * (Random.value * 60);
                var worldPosition = camera.transform.position + new Vector3(screenPosition.x, screenPosition.y);
                worldPosition.z = 0;
                var clone = happyAtomPrefab.Clone(worldPosition);
                print(screenPosition);
                print(worldPosition);
                print(clone.transform.position);

                nextSpawn = Time.time + spawnInterval;
                atomsToSpawn--;
            }
        } 
    }
}
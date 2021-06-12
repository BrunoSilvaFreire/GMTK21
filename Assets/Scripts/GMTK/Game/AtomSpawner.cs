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

                var screenPosition = new Vector3(Screen.width * Random.value, Screen.height * Random.value);
                var worldPosition = camera.ScreenToWorldPoint(screenPosition);
                worldPosition.z = 0;
                happyAtomPrefab.Clone(worldPosition);

                nextSpawn = Time.time + spawnInterval;
                atomsToSpawn--;
            }
        } 
    }
}
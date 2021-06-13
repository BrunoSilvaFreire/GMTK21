using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GMTK.Game {
    public class PhaseController : MonoBehaviour {
        public PhaseSetup[] steps;
        public AtomSpawner spawner;

        [Serializable]
        public class PhaseSetup {
            public UnityEvent active;
            public UnityEvent inactive;
        }

        private void OnValidate() {
            if (spawner == null) {
                return;
            }

            var num = spawner.phases.Count;
            steps ??= new PhaseSetup[num];
            Array.Resize(ref steps, num);
        }

        private void Start() {
            spawner.onPhaseChanged.AddListener(PhaseChanged);
        }

        private void PhaseChanged(int arg0) {
            var old = spawner.CurrentPhaseIndex;
            steps[old]?.inactive.Invoke();
            steps[arg0]?.active.Invoke();
        }
    }
}
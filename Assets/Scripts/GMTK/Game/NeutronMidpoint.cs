using System;
using System.Linq;
using Cinemachine;
using GMTK.Game.Traits;
using UnityEngine;

namespace GMTK.Game {
    public class NeutronMidpoint : MonoBehaviour {
        public CinemachineTargetGroup group;
        public float radius = 5;
        private void Update() {
            var allNeutrons = Neutron.AllNeutrons;
            var targets = group.m_Targets;
            if (allNeutrons.Count != targets.Length) {
                Array.Resize(ref targets, allNeutrons.Count);
            }

            for (var i = 0; i < targets.Length; i++) {
                ref var tgt = ref targets[i];
                tgt.radius = radius;
                tgt.target = allNeutrons[i].transform;
                tgt.weight = 1;
            }

            @group.m_Targets = targets;
        }
    }
}
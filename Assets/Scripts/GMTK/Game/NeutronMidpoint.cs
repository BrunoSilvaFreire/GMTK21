using System;
using System.Linq;
using GMTK.Game.Traits;
using UnityEngine;

namespace GMTK.Game {
    public class NeutronMidpoint : MonoBehaviour {
        private void Update() {
            var centroid = Vector3.zero;
            var allNeutrons = Neutron.AllNeutrons;
            centroid = allNeutrons.Aggregate(centroid, (current, point) => current + point.transform.position);
            centroid /= allNeutrons.Capacity;
            transform.position = centroid;
        }
    }
}
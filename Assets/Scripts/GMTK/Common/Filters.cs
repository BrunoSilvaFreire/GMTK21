using System;
using System.Collections.Generic;
using System.Linq;
using Lunari.Tsuki;
using UnityEngine;

namespace GMTK.Common {
    [Serializable]
    public class Filters : Filter {
        [SerializeReference] public List<Filter> filters;

        public override bool Allowed(Collider2D collider) {
            if (filters == null || filters.IsEmpty()) return true;
            return filters.All(filter => filter.Allowed(collider));
        }
    }
}
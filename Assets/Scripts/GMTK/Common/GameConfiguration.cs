using Lunari.Tsuki.Runtime.Singletons;
using UnityEngine;

namespace GMTK.Common {
    [CreateAssetMenu(menuName = "Datenshi/GameConfiguration")]
    public class GameConfiguration : ScriptableSingleton<GameConfiguration> {
        public LayerMask attackableMask;
        public LayerMask worldMask;
        public LayerMask interactableMask;
    }
}
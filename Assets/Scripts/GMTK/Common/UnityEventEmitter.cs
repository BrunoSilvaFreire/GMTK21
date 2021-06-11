using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace GMTK.Common {
    /// <summary>
    ///     Just for debugging purposes
    /// </summary>
    public class UnityEventEmitter : MonoBehaviour {
        public UnityEvent unityEvent;

        [Button]
        public void Invoke() {
            unityEvent.Invoke();
        }
    }
}
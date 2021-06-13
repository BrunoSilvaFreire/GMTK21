#if UNITY_EDITOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;
using UnityEngine.Events;

namespace GMTK.Common {
    /// <summary>
    ///     Just for debugging purposes
    /// </summary>
    public class UnityEventEmitter : MonoBehaviour {
        public UnityEvent unityEvent;

#if UNITY_EDITOR
        [Button]
#endif
        public void Invoke() {
            unityEvent.Invoke();
        }
    }
}
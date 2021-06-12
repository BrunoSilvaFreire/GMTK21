using UnityEngine;
using UnityEngine.Events;

namespace GMTK.UI {
    public class DelegateRebuilder : MonoBehaviour {
        public UnityEvent onRebuild;

        private void OnRectTransformDimensionsChange() {
            onRebuild.Invoke();
        }
    }
}
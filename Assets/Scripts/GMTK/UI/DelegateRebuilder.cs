using UnityEngine;
using UnityEngine.Events;

namespace GMTK.UI {
    public class DelegateRebuilder : MonoBehaviour {
        public UnityEvent onRebuild = new();

        private void OnRectTransformDimensionsChange() {
            onRebuild.Invoke();
        }
    }
}
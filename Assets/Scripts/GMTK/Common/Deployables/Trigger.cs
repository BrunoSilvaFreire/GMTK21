using UnityEngine;
using UnityEngine.Events;

namespace GMTK.Common.Deployables {
    public class Trigger : MonoBehaviour {
        public enum TriggerMode {
            Enter,
            Exit
        }

        public Filters filters;
        public bool onlyAllowOnce;

        public UnityEvent onTriggered;
        public TriggerMode mode = TriggerMode.Enter;
        private bool triggered;

        private void Awake() {
            ResetTriggered();
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (mode != TriggerMode.Enter) return;

            TryInvoke(other);
        }

        private void OnTriggerExit2D(Collider2D other) {
            if (mode != TriggerMode.Exit) return;

            TryInvoke(other);
        }

        private void ResetTriggered() {
            triggered = false;
        }

        private void TryInvoke(Collider2D other) {
            if (filters != null && !filters.Allowed(other)) return;

            if (!onlyAllowOnce && triggered) return;

            onTriggered.Invoke();
            triggered = true;
        }
    }
}
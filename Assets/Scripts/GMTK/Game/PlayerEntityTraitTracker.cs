using Lunari.Tsuki.Entities;
using UnityEngine.Events;

namespace GMTK.Game {
    public class PlayerEntityTraitTracker<T> where T : Trait {
        private T current;

        public PlayerEntityTraitTracker() {
            PullFrom(Player.Instance.Pawn);
            Player.Instance.onPawnChanged.AddListener(PullFrom);
        }

        public event UnityAction<T> Detached, Attached;

        private void PullFrom(Entity pawn) {
            OnDetached();
            if (pawn == null) {
                current = null;
            }
            else {
                current = pawn.GetTrait<T>();
                OnAttached(current);
            }
        }

        public bool Get(out T value) {
            value = current;
            return value != null;
        }

        protected virtual void OnDetached() {
            if (current != null) Detached?.Invoke(current);
        }

        protected virtual void OnAttached(T arg0) {
            if (arg0 != null) Attached?.Invoke(arg0);
        }
    }
}
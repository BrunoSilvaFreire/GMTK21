using System;
using System.Linq;
using GMTK.Common;
using GMTK.Entities.Common;
using GMTK.Input;
using Lunari.Tsuki.Entities;
using Lunari.Tsuki.Singletons;
using UnityEngine;
using UnityEngine.Events;

namespace GMTK.Game {
    public class Player : Singleton<Player> {
        [SerializeField] [HideInInspector] private Entity pawn;

        public EntityEvent onPawnChanged;
        public InputSource playerSource;
        public int pawnCameraPriority = 10;
        public Canvas globalCanvas;
        private int cachedPriority;
        private InputSource cachedSource;

        public Entity Pawn {
            get => pawn;
            set {
                if (value == pawn) return;
                if (pawn != null || pawn != null && pawn != value) {
                    if (pawn.Access(out EntityInput input)) {
                        input.Reset();
                        input.source = cachedSource;
                    }

                    if (pawn.Access(out Filmed filmed)) filmed.Camera.Priority = cachedPriority;
                }

                pawn = value;

                Configure(value);
            }
        }


        private void Start() {
            if (Pawn == null) {
                var candidate = Character.AvailableCharacters.FirstOrDefault(character => character.controllable);
                if (candidate != null) Pawn = candidate.Owner;
            }
            else {
                Configure(Pawn);
            }
        }

        public void SetPawn(Entity entity) {
            Pawn = entity;
        }

        private void Configure(Entity value) {
            onPawnChanged.Invoke(value);
            if (value == null) {
                Debug.LogWarning("No Entity");
                return;
            }


            if (value.Access(out EntityInput input)) {
                cachedSource = input.source;
                input.source = playerSource;
            }

            if (value.Access(out Filmed filmed)) {
                cachedPriority = filmed.Camera.Priority;
                filmed.Camera.Priority = pawnCameraPriority;
            }
        }

        public void Bind<A>(UnityAction<A> onAttached) where A : Trait {
            if (pawn != null)
                if (pawn.Access(out A trait))
                    onAttached(trait);
            onPawnChanged.AddListener(delegate(Entity arg0) {
                if (arg0.Access(out A trait)) onAttached(trait);
            });
        }
    }

    [Serializable]
    public class IsPlayerFilter : Filter {
        public override bool Allowed(Collider2D collider) {
            return collider.GetComponentInParent<Entity>() == Player.Instance.Pawn;
        }
    }

    [Serializable]
    public class IsOnEntityFilter : Filter {
        public override bool Allowed(Collider2D collider) {
            var entity = collider.GetComponent<Entity>();
            if (entity == null) return false;

            return entity.gameObject == collider.gameObject;
        }
    }
}
using GMTK.Input;
using Lunari.Tsuki.Entities;
using UnityEngine;
using UnityEngine.Events;

namespace GMTK.Game {
    public class NeutronAttractor : Trait {

        public UnityEvent<Vector3> onAttract;

        private Camera camera;
        private InputSource input;

        private Vector3 startDraggingPosition;

        public override void Configure(TraitDescriptor descriptor) {
            if (descriptor.DependsOn<EntityInput>(out var entityInput)) {
                input = entityInput.source;
            }
            camera = Camera.main;
        }

        private void Update() {
            if (input.mouseDown) {
                startDraggingPosition = input.mousePosition;
            }

            if (input.mouseUp) {
                var endDraggingPosition = input.mousePosition;
                onAttract.Invoke(startDraggingPosition - endDraggingPosition);
            }
        }
    }
}
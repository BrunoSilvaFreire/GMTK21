using System;
using GMTK.Input;
using Lunari.Tsuki.Entities;
using UnityEngine;
using UnityEngine.Events;

namespace GMTK.Game {
    public class NeutronAttractor : Trait {

        public UnityEvent<Vector3> onAttract;

        private Camera camera;
        private InputSource input;

        public Vector3 StartDraggingPosition { get; private set; }
        public bool dragging;
        public override void Configure(TraitDescriptor descriptor) {
            if (descriptor.DependsOn<EntityInput>(out var entityInput)) {
                input = entityInput.source;
            }
            camera = Camera.main;
        }

        private void Update() {
            
        }

        private void FixedUpdate() {
            if (input.mouseDown) {
                StartDraggingPosition = camera.ScreenToWorldPoint(input.mousePosition);
            }

            if (input.mouseUp) {
                var endDraggingPosition = camera.ScreenToWorldPoint(input.mousePosition);
                onAttract.Invoke(endDraggingPosition - StartDraggingPosition);
            }
        }
    }
}
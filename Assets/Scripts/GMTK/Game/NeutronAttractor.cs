using System;
using GMTK.Input;
using Lunari.Tsuki.Entities;
using UnityEngine;
using UnityEngine.Events;

namespace GMTK.Game {
    public class NeutronAttractor : Trait {
        public UnityEvent<Vector3> onAttract;

        private new Camera camera;
        private EntityInput input;

        public Vector3 StartDraggingPosition { get; private set; }
        public bool dragging;

        public override void Configure(TraitDescriptor descriptor) {
            descriptor.DependsOn(out input);
            camera = Camera.main;
        }

        private void Update() {
            if (input.MouseDown) {
                StartDraggingPosition = input.MousePosition;
            }

            if (input.MouseUp) {
                var endDraggingPosition = input.MousePosition;
                onAttract.Invoke(StartDraggingPosition - endDraggingPosition);
            }
        }
    }
}
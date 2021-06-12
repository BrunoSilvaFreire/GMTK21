using System;
using GMTK.Input;
using Lunari.Tsuki.Entities;
using UnityEngine;
using UnityEngine.Events;

namespace GMTK.Game {
    public class NeutronAttractor : Trait {
        public UnityEvent<Vector3> onAttractDirection;
        public UnityEvent<Vector3> onAttractPosition;

        public new Camera camera;
        private EntityInput input;

        public Vector3 StartDraggingPosition { get; private set; }

        public EntityInput Input => input;

        public bool dragging;
        public float maxDistance = 20;
        public float forceMultiplier = 0.5F;

        public override void Configure(TraitDescriptor descriptor) {
            descriptor.DependsOn(out input);
            camera = Camera.main;
        }

        private void Update() {
            if (Input.LeftMouseDown) {
                dragging = true;
                StartDraggingPosition = Input.MousePosition;
            }

            if (Input.LeftMouseUp) {
                dragging = false;
                var direction = GetForce();
                onAttractDirection.Invoke(direction);
            }

            if (Input.RightMouse) {
                onAttractPosition.Invoke(camera.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f)));
            }
        }

        public Vector3 GetForce() {
            var anchorPoint = StartDraggingPosition;
            var cameraPosition = camera.transform.position;
            anchorPoint.z -= cameraPosition.z;
            var mousePoint = Input.MousePosition;
            mousePoint.z -= cameraPosition.z;
            var direction = camera.ScreenToWorldPoint(anchorPoint) - camera.ScreenToWorldPoint(mousePoint);
            var mag = Mathf.Clamp(direction.magnitude, 0, maxDistance);
            var percentageForce = Mathf.InverseLerp(0, maxDistance, mag);
            return direction.normalized * (percentageForce * forceMultiplier);
        }
    }
}
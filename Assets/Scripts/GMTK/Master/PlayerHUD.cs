using System;
using GMTK.Game;
using GMTK.UI;
using Lunari.Tsuki.Entities;
using UnityEngine;

namespace GMTK.Master {
    public class PlayerHUD : Trait {
        private NeutronAttractor attractor;
        public GameObject anchorObject;
        public View anchorView;
        public LineRenderer line;
        public View lineView;
        public float offset;

        public override void Configure(TraitDescriptor descriptor) {
            descriptor.DependsOn(out attractor);
        }

        private void Update() {
            var dragging = attractor.dragging;
            anchorView.Shown = dragging;
            lineView.Shown = dragging;
            if (!dragging) {
                return;
            }

            var startDraggingPosition = attractor.StartDraggingPosition;
            var mousePosition = attractor.Input.MousePosition;
            var attractorCamera = attractor.camera;
            var cameraZ = attractorCamera.transform.position.z;
            startDraggingPosition.z = -cameraZ;
            mousePosition.z = -cameraZ;
            var anchorPos = attractorCamera.ScreenToWorldPoint(startDraggingPosition);
            
            var currentPos = attractorCamera.ScreenToWorldPoint(mousePosition);
            var dir = (currentPos - anchorPos);
            var normalizedDir = dir.normalized;
            var opposite = anchorPos - dir;
            var offsetVector = normalizedDir * offset;
            var arr = new[] {
                anchorPos + offsetVector,
                opposite + offsetVector
            };
            line.SetPositions(arr);
            anchorObject.transform.position = anchorPos;
        }
    }
}
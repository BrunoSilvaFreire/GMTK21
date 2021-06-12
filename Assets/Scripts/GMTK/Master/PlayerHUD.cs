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

            var anchorPos = attractor.StartDraggingPosition;
            var currentPos = attractor.Input.MousePosition;
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
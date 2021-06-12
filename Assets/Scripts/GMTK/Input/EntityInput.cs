using Lunari.Tsuki.Entities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GMTK.Input {
    public class EntityInput : Trait {
        public InputSource source;
        public bool zeroed;

        [ShowInInspector]
        public Vector3 MousePosition { get; private set; }

        [ShowInInspector]
        public bool MouseDown { get; private set; }

        [ShowInInspector]
        public bool MouseUp { get; private set; }

        public bool Zeroed {
            get => zeroed;
            set {
                zeroed = value;
                if (value) {
                    MouseDown = false;
                    MouseUp = false;       
                }
            }
        }

        private void Update() {
            MousePosition = source.GetMousePosition();
            if (Zeroed) {
                MouseDown = false;
                MouseUp = false;
            } else {
                MouseDown = source.GetMouseDown();
                MouseUp = source.GetMouseUp();
            }
        }
    }
}
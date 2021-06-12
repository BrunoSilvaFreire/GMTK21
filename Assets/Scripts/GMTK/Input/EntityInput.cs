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
        public bool LeftMouseDown { get; private set; }
        [ShowInInspector]
        public bool LeftMouseUp { get; private set; }
        
        public bool RightMouse { get; private set; }

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
            LeftMouseDown = source.GetMouseDown();
            LeftMouseUp = source.GetMouseUp();
            RightMouse = source.GetRightMouse();
            if (Zeroed) {
                LeftMouseDown = false;
                LeftMouseUp = false;
            } else {
                LeftMouseDown = source.GetMouseDown();
                LeftMouseUp = source.GetMouseUp();
            }
        }
    }
}

using Lunari.Tsuki.Entities;
#if UNITY_EDITOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;

namespace GMTK.Input {
    public class EntityInput : Trait {
        public InputSource source;
        public bool zeroed;

#if UNITY_EDITOR
        [ShowInInspector]
#endif
        public Vector3 MousePosition { get; private set; }

#if UNITY_EDITOR
        [ShowInInspector]
#endif
        public bool LeftMouseDown { get; private set; }

#if UNITY_EDITOR
        [ShowInInspector]
#endif
        public bool LeftMouseUp { get; private set; }

        public bool RightMouse { get; private set; }

        public bool Zeroed {
            get => zeroed;
            set {
                zeroed = value;
                if (value) {
                    LeftMouseDown = false;
                    LeftMouseUp = false;
                    RightMouse = false;
                }
            }
        }

        private void Update() {
            MousePosition = source.GetMousePosition();
            if (zeroed) {
                LeftMouseDown = false;
                LeftMouseUp = false;
                RightMouse = false;
            } else {
                LeftMouseDown = source.GetMouseDown();
                LeftMouseUp = source.GetMouseUp();
                RightMouse = source.GetRightMouse();
            }
        }
    }
}

    using Lunari.Tsuki.Entities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GMTK.Input {
    public class EntityInput : Trait {
        public InputSource source;
        [ShowInInspector]
        public Vector3 MousePosition { get; private set; }
        [ShowInInspector]
        public bool LeftMouseDown { get; private set; }
        [ShowInInspector]
        public bool LeftMouseUp { get; private set; }
        
        public bool RightMouse { get; private set; }

        private void Update() {
            MousePosition = source.GetMousePosition();
            LeftMouseDown = source.GetMouseDown();
            LeftMouseUp = source.GetMouseUp();
            RightMouse = source.GetRightMouse();
        }
    }
}
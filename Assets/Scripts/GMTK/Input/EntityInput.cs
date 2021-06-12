using Lunari.Tsuki.Entities;
using UnityEngine;

namespace GMTK.Input {
    public class EntityInput : Trait {
        public InputSource source;
        public Vector3 MousePosition { get; private set; }
        public bool MouseDown { get; private set; }
        public bool MouseUp { get; private set; }

        private void Update() {
            MousePosition = UnityEngine.Input.mousePosition;
            MouseDown = UnityEngine.Input.GetMouseButtonDown(0);
            MouseUp = UnityEngine.Input.GetMouseButtonUp(0);
        }
    }
}
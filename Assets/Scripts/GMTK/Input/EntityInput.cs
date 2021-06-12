    using Lunari.Tsuki.Entities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GMTK.Input {
    public class EntityInput : Trait {
        public InputSource source;
        [ShowInInspector]
        public Vector3 MousePosition { get; private set; }
        [ShowInInspector]
        public bool MouseDown { get; private set; }
        [ShowInInspector]
        public bool MouseUp { get; private set; }

        private void Update() {
            MousePosition = source.GetMousePosition();
            MouseDown = source.GetMouseDown();
            MouseUp = source.GetMouseUp();
        }
    }
}
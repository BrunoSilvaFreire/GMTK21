using Lunari.Tsuki.Entities;
using UnityEngine;

namespace GMTK.Input {
    public class InputSource : Trait 
    {
        public bool GetInteract() {//cringe
            return false;
        }
        
        public Vector3 mousePosition;
        public bool mouseDown;
        public bool mouseUp;
    }

    public class EntityInput : Trait {
        public InputSource source;
        
        private void Update() {
            source.mousePosition = UnityEngine.Input.mousePosition;
            source.mouseDown = UnityEngine.Input.GetMouseButtonDown(0);
            source.mouseUp = UnityEngine.Input.GetMouseButtonUp(0);
        }
    }
}
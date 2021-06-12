using Lunari.Tsuki.Entities;
using UnityEngine;

namespace GMTK.Input {
    public class InputSource : Trait 
    {
        public bool GetInteract() {//cringe
            return false;
        }

        public Vector3 GetMousePosition() {
            return UnityEngine.Input.mousePosition;
        }

        public bool GetMouseDown() {
            return UnityEngine.Input.GetMouseButtonDown(0);
        }

        public bool GetMouseUp() {
            return UnityEngine.Input.GetMouseButtonUp(0);
        }
    }
}
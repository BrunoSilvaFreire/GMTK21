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
}
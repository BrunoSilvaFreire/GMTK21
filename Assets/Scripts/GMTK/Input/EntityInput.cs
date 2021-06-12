using Lunari.Tsuki.Entities;

namespace GMTK.Input {
    public class EntityInput : Trait {
        public InputSource source;
        
        private void Update() {
            source.mousePosition = UnityEngine.Input.mousePosition;
            source.mouseDown = UnityEngine.Input.GetMouseButtonDown(0);
            source.mouseUp = UnityEngine.Input.GetMouseButtonUp(0);
        }
    }
}
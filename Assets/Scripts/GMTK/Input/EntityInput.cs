using Lunari.Tsuki.Entities;

namespace GMTK.Input {
    public abstract class InputSource : Trait 
    {
        public abstract bool GetInteract();
    }


    public class EntityInput : Trait {
        public InputSource source;

        public void Reset() {
        }
    }
}
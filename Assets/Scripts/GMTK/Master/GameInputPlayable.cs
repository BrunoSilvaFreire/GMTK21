using GMTK.Input;
using Lunari.Tsuki.Entities;
using UnityEngine;
using UnityEngine.Playables;

namespace GMTK.Master {
    public class GameInputPlayable : PlayableBehaviour {
        public EntityInput input;
        public bool valid;


        public override void OnBehaviourPlay(Playable playable, FrameData info) {
            input.Zeroed = valid;
        }

        public override void OnBehaviourPause(Playable playable, FrameData info) {
            input.Zeroed = !valid;
        }
    }
}
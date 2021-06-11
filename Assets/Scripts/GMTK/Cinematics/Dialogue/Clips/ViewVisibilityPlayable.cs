using System;
using GMTK.UI;
using UnityEngine.Playables;

namespace GMTK.Cinematics.Dialogue.Clips {
    [Serializable]
    public class ViewVisibilityPlayable : PlayableBehaviour {
        public View view;
        public bool value;

        public override void OnBehaviourPlay(Playable playable, FrameData info) {
            view.Shown = value;
        }

        public override void OnBehaviourPause(Playable playable, FrameData info) {
            view.Shown = !value;
        }
    }
}
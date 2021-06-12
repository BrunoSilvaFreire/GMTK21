using UnityEngine;
using UnityEngine.Playables;

namespace GMTK.Master {
    public class LoopRegionPlayable : PlayableBehaviour {
        public double loopTime;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData) {
            var time = playable.GetTime();
            var count = time + info.deltaTime;
            var duration = playable.GetDuration();
            var director = playerData as PlayableDirector;
            if (director == null) {
                return;
            }
            if (count > duration || Mathf.Approximately((float) time, (float) duration)) {
                director.time = loopTime;
            }
        }
    }
}
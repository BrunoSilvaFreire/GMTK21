using UnityEngine;
using UnityEngine.Playables;

namespace GMTK.Master {
    public class LoopRegionPlayableAsset : PlayableAsset {
        public double loopTime;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner) {
            var p = ScriptPlayable<LoopRegionPlayable>.Create(graph);
            var playable = p.GetBehaviour();
            playable.loopTime = loopTime;
            return p;
        }
    }
}
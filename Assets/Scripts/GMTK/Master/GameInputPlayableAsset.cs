using System;
using GMTK.Input;
using UnityEngine;
using UnityEngine.Playables;

namespace GMTK.Master {
    [Serializable]
    public class GameInputPlayableAsset : PlayableAsset {
        
        public ExposedReference<EntityInput> input;
        public bool valid;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner) {
            var playable = ScriptPlayable<GameInputPlayable>.Create(graph);
            var inputPlayable = playable.GetBehaviour();
            inputPlayable.input = input.Resolve(graph.GetResolver());
            inputPlayable.valid = valid;
            return playable;
        }
    }
}
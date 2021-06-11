using UnityEngine;
using UnityEngine.Playables;

namespace GMTK.Cinematics.Dialogue {
    public class DialogueAsset : PlayableAsset {
        public DialogueLine line;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner) {
            return DialoguePlayable.CreatePlayable(graph, line);
        }
    }
}
using GMTK.UI;
using UnityEngine;
using UnityEngine.Playables;

namespace GMTK.Cinematics.Dialogue.Clips {
    public class ViewVisibilityAsset : PlayableAsset {
        public ExposedReference<View> view;
        public bool value;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner) {
            var p = ScriptPlayable<ViewVisibilityPlayable>.Create(graph);
            var b = p.GetBehaviour();
            b.value = value;
            b.view = view.Resolve(graph.GetResolver());
            return p;
        }
    }
}
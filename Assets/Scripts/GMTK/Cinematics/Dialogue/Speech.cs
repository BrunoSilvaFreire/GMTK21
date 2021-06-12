using System.Collections;
using GMTK.Entities.Common;
using Lunari.Tsuki.Runtime;
using Lunari.Tsuki.Entities;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Playables;

namespace GMTK.Cinematics.Dialogue {
    public class Speech : Trait {
        public DialoguePanel panel;
        private PlayableGraph graph;
        private Coroutine routine;

        public override void Configure(TraitDescriptor descriptor) {
            if (!descriptor.Initialize) return;
            var entity = descriptor.Entity;
            entity.onAwareChanged.AddListener(delegate {
                if (entity.Aware) return;
                if (graph.IsValid()) graph.Destroy();
                panel.view.Hide();
            });
        }

        [ShowInInspector]
        public void Say(string line) {
            Say(DialogueLine.From(line));
        }

        public void Say(DialogueLine line) {
            if (line.character.defaultValue == null) line.character.defaultValue = Owner.GetTrait<Character>();

            graph = PlayableGraph.Create("Speech - Say");
            var playable = DialoguePlayable.CreatePlayable(graph, line);
            playable.SetDuration(DialoguesConfiguration.Instance.TotalDurationOf(line));
            var output = ScriptPlayableOutput.Create(graph, "Speech - Output");
            output.SetUserData(panel);
            output.SetSourcePlayable(playable);
            panel.AdjustTo(line);
            Coroutines.ReplaceCoroutine(ref routine, this, Play());
        }

        private IEnumerator Play() {
            // graph.Evaluate(0);
            panel.view.Show();
            yield return new WaitUntil(panel.view.IsFullyShown);
            graph.Play();
            yield return new WaitWhile(() => graph.IsValid() && graph.IsPlaying());
            yield return new WaitForSeconds(2);
            panel.view.Hide();
            if (graph.IsValid()) graph.Destroy();
        }
    }
}
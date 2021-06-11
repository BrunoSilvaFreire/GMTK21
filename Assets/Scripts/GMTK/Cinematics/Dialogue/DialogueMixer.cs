using System.Collections.Generic;
using System.Linq;
using GMTK.Combat;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace GMTK.Cinematics.Dialogue {
    public class DialogueMixer : PlayableBehaviour {
        public List<Living> cancelOnDeathOf;
        public TimelineClip[] clips;
        public PlayableDirector director;
        public DialoguePanel panel;

        public override void OnBehaviourPlay(Playable playable, FrameData info) {
            cancelOnDeathOf ??= new List<Living>();
            foreach (var dialogueTarget in cancelOnDeathOf) dialogueTarget.onPreDamage += Skip;
        }

        private bool ClipFor(DialogueLine line, out TimelineClip clip) {
            foreach (var timelineClip in clips) {
                if (!(timelineClip.asset is DialogueAsset asset) || asset.line != line) continue;

                clip = timelineClip;
                return true;
            }

            clip = null;
            return false;
        }

        private void Skip(DamageEvent damage) {
            var last = clips.Select(clip => clip.end).Max();
            director.time = last;
        }

        public override void OnBehaviourPause(Playable playable, FrameData info) {
            foreach (var dialogueTarget in cancelOnDeathOf) dialogueTarget.onPreDamage -= Skip;
        }

        public override void ProcessFrame(Playable playable, FrameData info, object playerData) {
            var show = false;
            var time = playable.GetTime();

            foreach (var clip in clips) {
                if (clip.start > time || clip.end < time) continue;

                show = true;
                break;
            }

            var finalPanel = panel;
            if (finalPanel == null) finalPanel = GlobalDialoguePanel.Instance;
            if (finalPanel.view) finalPanel.view.Shown = show;
        }
    }
}
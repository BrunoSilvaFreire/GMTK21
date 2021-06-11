using System;
using System.Collections.Generic;
using System.Linq;
using GMTK.Combat;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace GMTK.Cinematics.Dialogue {
    [Serializable]
    public class DialogueTarget {
        public ExposedReference<Living> target;
    }

    [TrackClipType(typeof(DialogueAsset))]
    [TrackBindingType(typeof(DialoguePanel))]
    public class DialogueTrack : TrackAsset {
        [ListDrawerSettings] public List<DialogueTarget> cancelOnDeathOf;

        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount) {
            var director = go.GetComponent<PlayableDirector>();
            if (director == null) {
                Debug.Log("Null director");
                return base.CreateTrackMixer(graph, go, inputCount);
            }

            var p = ScriptPlayable<DialogueMixer>.Create(graph, inputCount);
            var stage = (DialoguePanel) director.GetGenericBinding(this);
            var b = p.GetBehaviour();
            var list = cancelOnDeathOf?.Select(target => target.target.Resolve(director)).ToList();
            b.panel = stage;
            b.clips = GetClips().ToArray();
            b.cancelOnDeathOf = list;
            b.director = director;
            return p;
        }
    }
}
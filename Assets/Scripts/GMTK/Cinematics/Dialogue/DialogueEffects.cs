using System;
using System.Collections;
using System.Collections.Generic;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;

#if UNITY_EDITOR

#endif

namespace GMTK.Cinematics.Dialogue {
    public struct DialogueEffectInfo {
        public int startIndex, endIndex;
        public double effectDuration, effectTime;
    }

    [Serializable]
    public abstract class DialogueEffect {
#if UNITY_EDITOR
        private List<EditorCoroutine> editorCoroutines;
#endif
        private List<Tuple<DialoguePanel, Coroutine>> ownedRoutines;
        public virtual bool AllowsDefaultAudio => true;

        private void EnsureHasCoroutines() {
            if (ownedRoutines == null) ownedRoutines = new List<Tuple<DialoguePanel, Coroutine>>();
#if UNITY_EDITOR
            if (editorCoroutines == null) editorCoroutines = new List<EditorCoroutine>();
#endif
        }

        protected void StartEffectCoroutine(DialoguePanel panel, IEnumerator enumerator) {
            EnsureHasCoroutines();
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying) {
                editorCoroutines.Add(EditorCoroutineUtility.StartCoroutine(enumerator, panel));
                return;
            }
#endif
            ownedRoutines.Add(new Tuple<DialoguePanel, Coroutine>(
                panel,
                panel.StartCoroutine(enumerator)
            ));
        }

        public virtual void Begin(
            Playable playable,
            FrameData info,
            DialoguePanel panel,
            DialogueEffectInfo effectInfo
        ) {
        }

        public virtual void End(
            Playable playable,
            FrameData info,
            DialoguePanel panel,
            DialogueEffectInfo effectInfo
        ) {
            EnsureHasCoroutines();
            ownedRoutines.RemoveAll(delegate(Tuple<DialoguePanel, Coroutine> tuple) {
                var (p, routine) = tuple;
                var equal = p == panel;
                if (equal) p.EnqueueCoroutineToBeDeleted(routine);

                return equal;
            });
#if UNITY_EDITOR
            editorCoroutines.RemoveAll(coroutine => {
                EditorCoroutineUtility.StopCoroutine(coroutine);
                return true;
            });
#endif
        }

        public virtual void Tick(Playable playable,
            FrameData info,
            DialoguePanel panel,
            DialogueEffectInfo effectInfo) {
        }
    }
}
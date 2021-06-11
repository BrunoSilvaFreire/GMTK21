using System;
using System.Collections;
using System.Linq;
using GMTK.Common;
using GMTK.Game;
using Lunari.Tsuki;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using Random = UnityEngine.Random;

#if UNITY_EDITOR

#endif

namespace GMTK.Cinematics.Dialogue {
    public class DialoguePlayable : PlayableBehaviour {
        private Coroutine inputCoroutine;
        private FrameData lastFrameData;
        private int lastFullyShownCharacterIndex;
        private DialoguePanel lastPanel;

        // 
        private Playable lastPlayable;

        public DialogueLine line;
        private bool stoppedLastFrame;

        public static ScriptPlayable<DialoguePlayable> CreatePlayable(PlayableGraph graph, DialogueLine line) {
            var playable = ScriptPlayable<DialoguePlayable>.Create(graph);
            var b = playable.GetBehaviour();
            b.line = line;
            return playable;
        }

        private static bool TryGetPanel(object data, out DialoguePanel panel) {
            switch (data) {
                case null:
                    panel = GlobalDialoguePanel.Instance;
                    return true;
                case DialoguePanel found:
                    panel = found;
                    return true;
                default:
                    panel = null;
                    return false;
            }
        }

        private static bool TryGetPanel(FrameData playable, out DialoguePanel panel) {
            var output = playable.output;
            if (output.IsPlayableOutputOfType<ScriptPlayableOutput>()) {
                var data = output.GetUserData();
                if (TryGetPanel(data, out panel)) return true;

                if (data is DialoguePanel candidate) {
                    panel = candidate;
                    return true;
                }
            }

            panel = null;
            return false;
        }

        public override void OnBehaviourPlay(Playable playable, FrameData info) {
            if (TryGetPanel(info, out var panel)) {
                panel.ClearCoroutines();
                panel.content.OnPreRenderText += TMPDelegate;
                ForEachInfo(
                    playable.GetTime(),
                    (effect, effectInfo) => { effect.Begin(playable, info, panel, effectInfo); });
            }
        }

        private void TMPDelegate(TMP_TextInfo obj) {
            using var scope = new TextMeshProMeshScope(
                TMP_VertexDataUpdateFlags.Colors32 | TMP_VertexDataUpdateFlags.Vertices,
                obj);
            UpdateTextBuffer(scope);
        }

        public override void OnBehaviourPause(Playable playable, FrameData info) {
            if (TryGetPanel(info, out var panel)) {
                panel.content.OnPreRenderText -= TMPDelegate;
                panel.ClearCoroutines();
                ForEachInfo(
                    playable.GetTime(),
                    (effect, effectInfo) => { effect.End(playable, info, panel, effectInfo); }
                );
            }
        }

        private void ForEachInfo(double time, Action<DialogueEffect, DialogueEffectInfo> action) {
            var config = DialoguesConfiguration.Instance;
            var idx = 0;
            double startTime = 0;
            var spans = line.spans;
            foreach (var span in spans) {
                var content = TMPHelper.RemoveRichText(span.inflatedContent);
                var length = content.Length;
                if (length == 0) continue;

                var end = idx + length - 1;
                var endTime = startTime + config.TotalDurationOf(span);
                var effects = span.effects;
                if (effects != null && !effects.IsEmpty()) {
                    var duration = endTime - startTime;
                    var effectInfo = new DialogueEffectInfo {
                        startIndex = idx,
                        endIndex = end,
                        effectDuration = duration,
                        effectTime = Mathf.InverseLerp((float) startTime, (float) endTime, (float) time)
                    };
                    foreach (var dialogueEffect in effects) action(dialogueEffect, effectInfo);
                }

                idx += length;
                startTime += endTime - startTime;
            }
        }

        public override void ProcessFrame(Playable playable, FrameData info, object playerData) {
            var duration = playable.GetDuration();
            // Current time in seconds since the start of the playable
            var time = playable.GetTime();
            var count = time + info.deltaTime;
            if (!TryGetPanel(playerData, out var panel)) return;

            ;
            if (count > duration || Mathf.Approximately((float) time, (float) duration)) {
                if (line.requireInteraction && !stoppedLastFrame) {
                    stoppedLastFrame = true;
                    Stop(playable, panel);
                }
            }
            else {
                stoppedLastFrame = false;
            }

            if (panel == null) return;

            var tmp = panel.content;
            var character = line.character.Resolve(playable.GetGraph().GetResolver());
            var title = panel.characterTitle;
            if (title != null) {
                string titleContent;
                if (line.customTitle.IsNullOrEmpty())
                    titleContent = character != null ? character.alias : string.Empty;
                else
                    titleContent = line.customTitle;

                title.text = titleContent;
            }

            var newText = line.GetText(true);
            lastPlayable = playable;
            lastFrameData = info;
            lastPanel = panel;
            if (tmp.text != newText) {
                tmp.text = newText;
                lastFullyShownCharacterIndex = 0;
                tmp.ForceMeshUpdate();
                panel.ClearCoroutines();
                panel.AdjustTo(character);
            }
            else {
                using var scope =
                    new TextMeshProMeshScope(TMP_VertexDataUpdateFlags.Colors32 | TMP_VertexDataUpdateFlags.Vertices,
                        tmp);
                UpdateTextBuffer(scope);
            }
        }


        private void UpdateTextBuffer(TextMeshProMeshScope scope) {
            if (lastPlayable.Equals(Playable.Null)) return;

            var time = lastPlayable.GetTime();
            var tInfo = scope.Info;
            var characterInfoArr = tInfo.characterInfo;
            var config = DialoguesConfiguration.Instance;

            // Total duration in **seconds** of the playable
            var playableDuration = lastPlayable.GetDuration();
            // The point in time (in seconds) where the last char of dialogue is displayed.
            var endpoint = config.DialogueDuration(playableDuration);

            line.ForEachCharacter(tInfo, (span, index, startTime, endTime) => {
                var characterInfo = characterInfoArr[index];
                if (!characterInfo.isVisible) return;


                byte alpha;

                float yOffset;
                var finalEndPoint = startTime + config.extraTimeToFullAlpha;
                float evalTime;
                if (finalEndPoint < time) {
                    alpha = byte.MaxValue;
                    evalTime = 1;
                }
                else if (startTime > time) {
                    alpha = byte.MinValue;
                    evalTime = 0;
                }
                else {
                    var charProgress = Mathf.InverseLerp((float) startTime, (float) finalEndPoint, (float) time);
                    alpha = (byte) (charProgress * byte.MaxValue);
                    evalTime = Mathf.InverseLerp((float) startTime, (float) finalEndPoint, (float) time);
                }

                var lastPanelAudio = lastPanel.audio;
                if (lastPanelAudio != null) {
                    var playAudio = time > startTime && index > lastFullyShownCharacterIndex;

                    if (playAudio && span.effects.Any(effect => !effect.AllowsDefaultAudio)) playAudio = false;

                    if (playAudio) {
                        lastFullyShownCharacterIndex = index;
                        const float pitchRadius = 0.05F;
                        float basePitch = 1;
                        var character = line.character.Resolve(lastPlayable.GetGraph().GetResolver());
                        if (character != null) basePitch = character.voicePitch;

                        lastPanelAudio.pitch = Random.value * pitchRadius + basePitch;
                        lastPanelAudio.Play();
                    }
                }


                yOffset = config.yOffsetCurve.Evaluate(evalTime);
                scope.SetCharacterAlpha(index, alpha);
                scope.AddYPosition(index, -yOffset);
            });
            ForEachInfo(time,
                (effect, effectInfo) => { effect.Tick(lastPlayable, lastFrameData, lastPanel, effectInfo); });
        }

        private void Stop(Playable playable, DialoguePanel panel) {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying) return;
#endif
            var graph = playable.GetGraph();
            graph.Stop();
            Coroutines.ReplaceCoroutine(ref inputCoroutine, panel, WaitForInputAndRestart(graph, panel));
        }

        private static IEnumerator WaitForInputAndRestart(PlayableGraph playableGraph, DialoguePanel panel) {
            var player = Player.Instance;
            var source = player.playerSource;
            var intView = panel.interactionView;
            if (intView != null) intView.Show();

            while (!source.GetInteract()) yield return null;

            if (intView != null) intView.Hide();

            playableGraph.Play();
        }
    }
}
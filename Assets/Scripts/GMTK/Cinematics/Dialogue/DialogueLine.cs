using System;
using System.Collections.Generic;
using GMTK.Common;
using GMTK.Entities.Common;
using Lunari.Tsuki.Runtime;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace GMTK.Cinematics.Dialogue {
    [Serializable]
    public class DialogueLine {
        public delegate void DialogueCharacterAction(DialogueSpan span, int index, double startTime, double endTime);

        public delegate void DialogueSpanAction(DialogueSpan span, int startIndex, int endIndex, double startTime,
            double endTime);

        public ExposedReference<Character> character;

        [SerializeField] public List<DialogueSpan> spans = new();

        public bool requireInteraction;
        public string customTitle;
        public bool noHoldTime;
        public float extraDuration;

        public string GetText(bool inflated = false) {
            var msg = string.Empty;
            foreach (var dialogueSpan in spans) {
                var txt = inflated ? dialogueSpan.inflatedContent : dialogueSpan.StrippedContent;
                msg += txt;
            }

            return msg;
        }

        public void ForEachSpan(DialogueSpanAction action) {
            if (spans == null || spans.IsEmpty()) return;

            var config = DialoguesConfiguration.Instance;
            var idx = 0;
            double startPoint = 0;
            foreach (var span in spans) {
                var length = span.inflatedContent.Length;
                var end = idx + length - 1;
                var endPoint = startPoint + config.TotalDurationOf(span);
                action(span, idx, end, startPoint, endPoint);
                idx += length;
                startPoint += endPoint - startPoint;
            }
        }

        public void ForEachCharacter(TMP_TextInfo textInfo, DialogueCharacterAction action) {
            var chars = textInfo.characterInfo;
            var config = DialoguesConfiguration.Instance;
            double time = 0;
            int currentSpanIndex = 0, localSpanIndex = 0;
            for (var i = 0; i < textInfo.characterCount; i++) {
                var duration = config.TimeOfCharacter(chars[i].character);
                var span = spans[currentSpanIndex];
                var endPoint = time + duration;
                action(span, i, time, endPoint);
                time = endPoint;
                if (++localSpanIndex >= span.inflatedContent.Length) {
                    currentSpanIndex++;
                    localSpanIndex = 0;
                }
            }
        }

        public static DialogueLine From(string line) {
            return new() {
                spans = new List<DialogueSpan> {
                    new() {
                        inflatedContent = line
                    }
                }
            };
        }
    }

    [Serializable]
    public class DialogueSpan {
        [TextArea] [FormerlySerializedAs("content")] [LabelText("content")]
        public string inflatedContent;

        [SerializeReference] [ShowInInspector] public List<DialogueEffect> effects = new();

        public string StrippedContent => TMPHelper.RemoveRichText(inflatedContent);
    }
}
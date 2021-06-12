using Lunari.Tsuki.Runtime.Singletons;
using UnityEngine;

namespace GMTK.Cinematics.Dialogue {
    [CreateAssetMenu(menuName = "Datenshi/Cinematics/DialoguesConfiguration")]
    public class DialoguesConfiguration : ScriptableSingleton<DialoguesConfiguration> {
        public int charactersPerSeconds = 40;
        public double holdDuration = 0.2;
        public double ponctuationTimeMultiplier = 2;
        public float extraTimeToFullAlpha = 0.25F;
        public AnimationCurve yOffsetCurve;

        public bool IsMultiplyingPonctuation(char character) {
            switch (character) {
                case '\'':
                    return false;
                default:
                    return char.IsPunctuation(character);
            }
        }

        public double TimeOfCharacter(char character) {
            if (char.IsWhiteSpace(character)) return 0;

            var timePerChar = 1.0 / charactersPerSeconds;
            if (IsMultiplyingPonctuation(character)) return timePerChar * ponctuationTimeMultiplier;

            return timePerChar;
        }

        public double DialogueDuration(double totalClipTime) {
            return totalClipTime - holdDuration;
        }

        public double TotalDurationOf(DialogueLine line) {
            var duration = TotalDurationOf(line.GetText());
            if (!line.noHoldTime) duration += holdDuration;

            duration += line.extraDuration;
            return duration;
        }

        public double TotalDurationOf(DialogueSpan span) {
            return TotalDurationOf(span.StrippedContent);
        }

        public double TotalDurationOf(string text) {
            double sum = 0;
            var timePerChar = 1.0 / charactersPerSeconds;
            foreach (var character in text) {
                if (char.IsWhiteSpace(character)) continue;

                if (IsMultiplyingPonctuation(character))
                    sum += timePerChar * ponctuationTimeMultiplier;
                else
                    sum += timePerChar;
            }

            return sum;
        }
    }
}
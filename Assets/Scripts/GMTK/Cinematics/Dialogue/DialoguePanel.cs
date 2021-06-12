using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GMTK.Entities.Common;
using GMTK.UI;
using Lunari.Tsuki.Runtime;
using Sirenix.OdinInspector;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace GMTK.Cinematics.Dialogue {
    public class DialoguePanel : DelegateLayoutElement {
        [Required] public View view;

        public View interactionView;

        [Required] public TMP_Text characterTitle;

        [Required] public TMP_Text content;

        public ContentSizeFitter contentSizeFitter;

        public float whenCharacterPresentExtraHeight;
        public float resizeDuration = 0.25F;

        [Required] public new AudioSource audio;

        public bool immediatelyHideOnStart = true;
        private Coroutine resizeRoutine;

        private List<Coroutine> toEnd;

        public override float preferredWidth {
            get {
                if (characterTitle == null) return base.preferredWidth;

                return Mathf.Max(base.preferredWidth, characterTitle.preferredWidth);
            }
        }

        public override float preferredHeight {
            get {
                var normal = base.preferredHeight;
                if (characterTitle != null && !characterTitle.text.IsEmpty())
                    normal += characterTitle.preferredHeight + whenCharacterPresentExtraHeight;

                return normal;
            }
        }

        private void Start() {
            if (immediatelyHideOnStart) {
                view.Hide(true);
                if (interactionView != null) interactionView.Hide(true);
            }
        }

        public void AdjustTo(DialogueLine line) {
            var character = line.character.defaultValue as Character;
            content.text = line.GetText();
            AdjustTo(character);
            var contentColor = content.color;
            contentColor.a = 0;
            content.color = contentColor;
            content.ForceMeshUpdate();
        }

        public void AdjustTo(Character character) {
            var rectT = (RectTransform) transform;
            var title = characterTitle;
            // if (title != null) {
            //     title.text = character != null ? character.alias : string.Empty;
            //     title.ForceMeshUpdate();
            // }

            rectT.DOKill();
            LayoutRebuilder.MarkLayoutForRebuild(rectT);
            var endValue = new Vector2(preferredWidth, preferredHeight);
            AdjustTo(endValue);
        }

        public void AdjustTo(Vector2 endValue) {
            var rectT = (RectTransform) transform;
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying) {
                rectT.sizeDelta = endValue;
            }
            else {
#endif
                if (contentSizeFitter != null) contentSizeFitter.enabled = false;

                Coroutines.ReplaceCoroutine(ref resizeRoutine, this, ResizeCoroutine());
#if UNITY_EDITOR
            }
#endif
        }

        private IEnumerator ResizeCoroutine() {
            if (contentSizeFitter != null) contentSizeFitter.enabled = false;

            float time = 0;
            var rectT = (RectTransform) transform;
            var initSize = rectT.sizeDelta;

            while (resizeDuration > time) {
                yield return null;
                time += Time.deltaTime;
                rectT.sizeDelta = Vector2.Lerp(initSize, new Vector2(preferredWidth, preferredHeight),
                    time / resizeDuration);
            }

            if (contentSizeFitter != null) contentSizeFitter.enabled = true;
        }

        public void EnqueueCoroutineToBeDeleted(Coroutine routine) {
            toEnd ??= new List<Coroutine>();
            toEnd.Add(routine);
        }

        public void ClearCoroutines() {
            if (toEnd == null) return;

            foreach (var coroutine in toEnd) StopCoroutine(coroutine);
        }
    }
}
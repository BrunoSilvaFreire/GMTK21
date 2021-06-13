using DG.Tweening;
using Plugins.DOTween.Modules;
#if UNITY_EDITOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;

namespace GMTK.UI {
    public class TransparentView : View {
        public const string TransparencyGroup = "Transparency & Transition Stuff";

#if UNITY_EDITOR
        [Required] [BoxGroup(TransparencyGroup)]
#endif
        public CanvasGroup canvasGroup;

#if UNITY_EDITOR
        [BoxGroup(TransparencyGroup)] 
#endif
        public float revealedOpacity = 1, concealedOpacity;

#if UNITY_EDITOR
        [BoxGroup(TransparencyGroup)] 
#endif
        public float revelationTransitionDuration = 0.5F, concealTransitionDuration;

        protected override void Conceal() {
            AnimateTo(concealedOpacity, concealTransitionDuration);
        }


        protected override void Reveal() {
            AnimateTo(revealedOpacity, revelationTransitionDuration);
        }

        private void AnimateTo(float target, float duration) {
            canvasGroup.DOKill();
            canvasGroup.DOFade(target, duration);
        }

        protected override void ImmediateConceal() {
            canvasGroup.DOKill();
            canvasGroup.alpha = concealedOpacity;
        }

        protected override void ImmediateReveal() {
            canvasGroup.DOKill();
            canvasGroup.alpha = revealedOpacity;
        }

        public override bool IsFullyShown() {
            return Mathf.Abs(revealedOpacity - canvasGroup.alpha) < 0.05;
        }
    }
}
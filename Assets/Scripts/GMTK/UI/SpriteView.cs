using DG.Tweening;
using Plugins.DOTween.Modules;
#if UNITY_EDITOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;

namespace GMTK.UI {
    public class SpriteView : View {
        public const string TransparencyGroup = "Transparency & Transition Stuff";
#if UNITY_EDITOR
        [Required] [BoxGroup(TransparencyGroup)]
#endif
        public SpriteRenderer spriteRenderer;

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
            spriteRenderer.DOKill();
            spriteRenderer.DOFade(target, duration);
        }

        protected override void ImmediateConceal() {
            spriteRenderer.DOKill();
            var col = spriteRenderer.color;
            col.a = concealedOpacity;
            spriteRenderer.color = col;
        }

        protected override void ImmediateReveal() {
            spriteRenderer.DOKill();
            var col = spriteRenderer.color;
            col.a = revealedOpacity;
            spriteRenderer.color = col;
        }

        public override bool IsFullyShown() {
            return Mathf.Abs(revealedOpacity - spriteRenderer.color.a) < 0.05;
        }
    }
}
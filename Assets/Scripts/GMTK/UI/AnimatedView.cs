using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor.Animations;
using UnityEngine;

#if UNITY_EDITOR

#endif

namespace GMTK.UI {
    public class AnimatedView : View {
        private static readonly int RevealKey = Animator.StringToHash("Visible");

        public Animator animator;

        [ValueDropdown(nameof(GetAllStates))] public string revealedState = "Base Layer.Revealed";

        [ValueDropdown(nameof(GetAllStates))] public string concealedState = "Base Layer.Concealed";
#if UNITY_EDITOR
        public IEnumerable<string> GetAllStates {
            get {
                if (animator == null) yield break;

                var controller = (AnimatorController) animator.runtimeAnimatorController;
                foreach (var layer in controller.layers)
                foreach (var state in layer.stateMachine.states)
                    yield return $"{layer.name}.{state.state.name}";
            }
        }
#endif
#if UNITY_EDITOR
        protected virtual void OnValidate() {
            if (animator != null)
                if (animator.runtimeAnimatorController is AnimatorController controller) {
                    if (controller.parameters.Any(parameter => parameter.nameHash == RevealKey)) return;

                    controller.AddParameter("Visible", AnimatorControllerParameterType.Bool);
                }
        }
#endif

        protected override void Conceal() {
            animator.SetBool(RevealKey, false);
        }

        protected override void Reveal() {
            animator.SetBool(RevealKey, true);
        }

        protected override void ImmediateConceal() {
            animator.Play(concealedState, -1, 1);
            Conceal();
        }

        protected override void ImmediateReveal() {
            animator.Play(revealedState, -1, 1);
            Reveal();
        }

        public override bool IsFullyShown() {
            var state = animator.GetCurrentAnimatorStateInfo(0);
            return state.normalizedTime > 0.95;
        }
    }
}
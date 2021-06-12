using System;
using System.Collections.Generic;
using GMTK.Cinematics.Dialogue;
using GMTK.UI;
using Lunari.Tsuki.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace GMTK.Master {
    public class Tutorial : MonoBehaviour {
        public bool autoBegin;
        public UnityEvent onCurrentChanged;

        [SerializeField]
        private int current = -1;

        public List<Step> steps;
        public Animator animator;
        public string animatorParameter;

        public int Current {
            get => current;
            set {
                if (current == value) return;

                SetShown(value, true);
                if (Active) {
                    SetShown(current, false);
                }

                current = value;
                var step = CurrentStep;
                step?.onCompleted.AddDisposableListener(() => { Current++; }).FireOnce().DisposeOn(onCurrentChanged);
                animator.SetInteger(animatorParameter, value);

                onCurrentChanged.Invoke();
            }
        }

        public Step CurrentStep => Active ? steps[current] : null;

        public bool Active => current >= 0 && current < steps.Count;

        private void Start() {
            if (autoBegin) Begin();
        }

        public void Begin() {
            if (Active) return;

            Current = 0;
        }

        public void End() {
            Current = -1;
        }

        private void SetShown(int step, bool shown) {
            if (step < 0 || step >= steps.Count) return;

            SetShown(steps[step], shown);
        }

        private void SetShown(Step step, bool shown) {
            SetShown(step.view, shown);
            // var highlighter = TutorialHighlight.Instance;
            // if (highlighter != null) {
            //     var stepToHighlight = step.toHighlight;
            //     if (shown) {
            //         if (stepToHighlight != null) highlighter.Focus(stepToHighlight, step.line);
            //     } else {
            //         if (highlighter.Highlighting == stepToHighlight) highlighter.Hide();
            //     }
            // }
        }

        private static void SetShown(View view, bool shown) {
            if (view == null) {
                return;
            }

            view.Shown = shown;
        }

        [Serializable]
        public class Step {
            public View view;
            public UnityEvent onCompleted;
            public void Complete() {
                onCompleted.Invoke();
            }
        }
    }
}
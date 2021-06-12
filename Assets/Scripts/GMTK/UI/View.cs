using System;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
#endif

namespace GMTK.UI {
    public abstract class View : MonoBehaviour {
        public const string ViewGroup = "View Stuff";
        [BoxGroup(ViewGroup)]
        public bool immediateSnapOnStart = true;

        [SerializeField]
        [HideInInspector]
        private bool shown;

        private void Start() {
            if (immediateSnapOnStart) {
                if (shown) {
                    Show(true);
                } else {
                    Hide(true);
                }
            }
        }

        [ShowInInspector]
        [BoxGroup(ViewGroup)]
        public bool Shown {
            get => shown;
            set {
                if (value == shown) return;
#if UNITY_EDITOR
                if (!EditorApplication.isPlaying) {
                    if (value)
                        Show(true);
                    else
                        Hide(true);
                    return;
                }
#endif
                if (value)
                    Show();
                else
                    Hide();
            }
        }

        public void Show(bool immediate = false) {
            shown = true;
            if (immediate)
                ImmediateReveal();
            else
                Reveal();
        }

        public void Hide(bool immediate = false) {
            shown = false;
            if (immediate)
                ImmediateConceal();
            else
                Conceal();
        }

        protected abstract void Conceal();
        protected abstract void Reveal();
        protected abstract void ImmediateConceal();
        protected abstract void ImmediateReveal();
        public abstract bool IsFullyShown();
    }
}
using System;
using GMTK.Game;
using TMPro;
using UnityEngine;

namespace GMTK.Master {
    public class IntroTutorialController : MonoBehaviour {
        private NeutronAttractor attractor;
        private bool firstClicked;
        public Tutorial tutorial;
        public FollowMouse mouse;

        private void Start() {
            attractor = Player.Instance.Pawn.GetTrait<NeutronAttractor>();
        }

        private void Update() {
            mouse.positionOverride = attractor.StartDraggingPosition;
            switch (tutorial.Current) {
                case 0:
                    if (!firstClicked) {
                        if (attractor.dragging) {
                            tutorial.CurrentStep.Complete();
                            firstClicked = true;
                            mouse.usePositionOverride = true;
                        }
                    }

                    break;
                case 1:
                    if (!attractor.dragging) {
                        tutorial.CurrentStep.Complete();
                    }
                    break;
                case 2:
                    if (attractor.Input.RightMouse) {
                        tutorial.End();
                    }
                    break;
            }
        }
    }
}
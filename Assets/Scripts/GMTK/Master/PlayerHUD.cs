using System;
using GMTK.Game;
using GMTK.Game.Traits;
using GMTK.UI;
using GraphVisualizer;
using Lunari.Tsuki.Entities;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.UI;

namespace GMTK.Master {
    public class PlayerHUD : Trait {
        private NeutronAttractor attractor;
        public GameObject anchorObject;
        public View anchorView;
        public LineRenderer line;
        public float offset;
        public float lineStartAlpha = 1;
        public float lineEndAlpha = 0.5F;
        private Color[] colors;
        public SpriteRenderer reference;
        public AudioSource neutronAudioSource;
        public AnimationCurve neutronAudioPitch;
        public TMP_Text scoreLabel;
        public int currentScore;
        public float lerpSpeed = 50;

        public override void Configure(TraitDescriptor descriptor) {
            descriptor.DependsOn(out attractor);
        }

        private void Start() {
            colors = new Color[2];
            colors[0] = Color.white;
            colors[1] = Color.white;
            Neutron.OnNumNeutronsChanged.AddListener(OnNumNeutronsChanged);
        }

        private void OnNumNeutronsChanged() {
            var pitch = neutronAudioPitch.Evaluate(Neutron.AllNeutrons.Count);
            neutronAudioSource.pitch = pitch;
            neutronAudioSource.Play();
        }

        private void Update() {
            UpdateForceVisualizer();
            UpdateNeutronCount();
        }

        private void UpdateNeutronCount() {
            var score = Neutron.AllNeutrons.Count;
            currentScore = (int) Mathf.Lerp(currentScore, score, lerpSpeed * Time.deltaTime);
            scoreLabel.text = currentScore.ToString();
        }

        private void UpdateForceVisualizer() {
            ref var firstColor = ref colors[0];
            ref var secondColor = ref colors[1];
            var p = reference.color.a;
            firstColor.a = Mathf.Lerp(0, lineStartAlpha, p);
            secondColor.a = Mathf.Lerp(0, lineEndAlpha, p);
            line.startColor = firstColor;
            line.endColor = secondColor;
            var dragging = attractor.dragging;
            anchorView.Shown = dragging;
            if (!dragging) {
                return;
            }

            var startDraggingPosition = attractor.StartDraggingPosition;
            var attractorCamera = attractor.camera;
            var cameraZ = attractorCamera.transform.position.z;
            startDraggingPosition.z = -cameraZ;
            var anchorPos = attractorCamera.ScreenToWorldPoint(startDraggingPosition);
            var dir = -attractor.GetForce();
            var normalizedDir = dir.normalized;
            var opposite = anchorPos - dir;
            var offsetVector = normalizedDir * offset;
            var arr = new[] {
                anchorPos + offsetVector,
                opposite + offsetVector
            };
            line.SetPositions(arr);
            anchorObject.transform.position = anchorPos;
        }
    }
}
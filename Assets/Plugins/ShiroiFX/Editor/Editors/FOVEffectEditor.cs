using Shiroi.FX.Effects.BuiltIn;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

namespace Shiroi.FX.Editor.Editors {
    [CustomEditor(typeof(FOVEffect))]
    public class FOVEffectEditor : UnityEditor.Editor {
        public static readonly GUIContent FOVTitle = new("Field Of View Effect");

        public static readonly GUIContent FOVSubtitle =
            new("Modifies the field of view of a camera, useful for creating impact");

        public static readonly GUIContent FOVContent = new(
            "Field of View",
            "The field of view to be set to the camera"
        );

        public static readonly GUIContent FOVModeContent = new(
            "Field of View Mode",
            "Wheter to use an animated or constant value."
        );

        public static readonly GUIContent DurationContent = new(
            "Duration",
            "How long to modify the FOV for"
        );

        public static readonly GUIContent ControllerConfigTitle = new(
            "Controller Config"
        );

        public static readonly GUIContent ControllerConfigSubtitle = new(
            "Configurations of the service used to play this effect"
        );

        public static readonly GUIContent ServicePriorityContent = new("Service priority");
        private FOVEffect effect;
        private EffectHeader header;

        private AnimBool usesConstantValue;

        private void OnEnable() {
            effect = (FOVEffect) target;
            header = new EffectHeader(typeof(FOVEffect));
            usesConstantValue = new AnimBool();
            usesConstantValue.valueChanged.AddListener(Repaint);
        }

        public override void OnInspectorGUI() {
            header.DoLayout();
            var skin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector);
            EditorGUILayout.BeginVertical(skin.box);
            ShiroiFXGUI.DrawTitle(FOVTitle, FOVSubtitle);
            DrawEffect();
            DrawControllerConfig();
            EditorGUILayout.EndVertical();
        }

        private void DrawControllerConfig() {
            ShiroiFXGUI.DrawTitle(ControllerConfigTitle, ControllerConfigSubtitle);
            effect.ServicePriority = (ushort) EditorGUILayout.IntSlider(
                ServicePriorityContent,
                effect.ServicePriority,
                ushort.MinValue,
                ushort.MaxValue);
        }

        private void DrawEffect() {
            //ShiroiFXGUI.DrawTitle(FOVContent, TimeScaleSubtitleHeader);
            ShiroiFXGUI.DrawAnimatedOrConstantValue(FOVContent, FOVModeContent, ref effect.Mode,
                ref usesConstantValue, ref effect.ConstantFOV, ref effect.AnimatedFOV);
            effect.Duration = EditorGUILayout.FloatField(DurationContent, effect.Duration);
        }
    }
}
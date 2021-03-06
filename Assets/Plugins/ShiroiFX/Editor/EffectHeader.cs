using System;
using System.Collections.Generic;
using System.Linq;
using Shiroi.FX.Effects.Requirements;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

namespace Shiroi.FX.Editor {
    public class EffectHeader {
        private static readonly GUIContent RequiredEffectContent =
            new("This effect requires the following features:");

        public static readonly GUIContent OptionalEffectContent = new("This effect optinally uses:");
        private readonly RequirementsDescription description;
        private readonly OptinalFeatureAttribute optinalFeature;
        private readonly RequiresFeatureAttribute requiredFeature;

        private bool drawOptinal = true;
        private bool drawRequired = true;

        public EffectHeader(Type type) {
            TryLoadAttribute(type, out requiredFeature);
            TryLoadAttribute(type, out optinalFeature);
            TryLoadAttribute(type, out description);
        }

        private static void TryLoadAttribute<T>(Type type, out T output) {
            var customAttributes = type.GetCustomAttributes(typeof(T), true) as T[];
            if (customAttributes == null || customAttributes.Length <= 0)
                output = default;
            else
                output = customAttributes.First();
        }

        public void DoLayout() {
            var hasAttributes = requiredFeature != null || optinalFeature != null;
            var skin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector);
            if (hasAttributes) EditorGUILayout.BeginVertical(skin.box);

            if (requiredFeature != null)
                DrawRequirement(RequiredEffectContent,
                    requiredFeature.FeatureTypes, 0, ref drawRequired);

            if (optinalFeature != null) {
                var offset = requiredFeature == null ? 0 : requiredFeature.TotalRequiredFeatures;
                DrawRequirement(OptionalEffectContent, optinalFeature.FeatureTypes, offset, ref drawOptinal);
            }


            if (hasAttributes) EditorGUILayout.EndVertical();
        }

        private void TryDrawDescription(int index) {
            if (description == null) return;

            var d = description.Descriptions;
            if (d.Length <= index) return;

            EditorGUILayout.LabelField(d[index], ShiroiFXEditorResources.Subtitle,
                //    GUILayout.ExpandHeight(true),
                GUILayout.ExpandWidth(true)
            );
        }


        private void DrawRequirement(GUIContent header, IList<Type> features, int descriptionOffset, ref bool option) {
            EditorGUI.indentLevel++;
            option = EditorGUILayout.Foldout(option, header, ShiroiFXEditorResources.DescriptionHeaderStyle);
            EditorGUI.indentLevel--;
            if (!option) return;

            for (var i = 0; i < features.Count; i++) {
                var feature = features[i];
                EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                EditorGUILayout.PrefixLabel(feature.GetNiceName());
                TryDrawDescription(i + descriptionOffset);
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}
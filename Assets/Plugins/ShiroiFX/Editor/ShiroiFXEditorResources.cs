﻿using UnityEditor;
using UnityEngine;

namespace Shiroi.FX.Editor {
    public static class ShiroiFXEditorResources {
        public const string BasePath = "Shiroi/FX/";
        public const string BaseCreatePath = "Assets/Create/" + BasePath;
        public static readonly GUIContent EditFX = new("Edit FX", "Selects the effect for editing");

        public static readonly GUIContent RemoveFX = new(
            "Remove FX",
            "Removes this effect from the composite effect"
        );

        public static readonly GUIContent NullFXError = new(
            "Null effect found!",
            "There is a null effect on the composite effect, please remove it."
        );

        public static readonly GUIContent AddEffect = new("Add Effect", "Adds a new effect to the list");
        public static readonly Color SubtitleColor = new(0.06f, 0.06f, 0.06f, 0.7f);

        public static readonly GUIStyle Subtitle = new() {
            fontSize = 10,
            wordWrap = true,
            normal = {
                textColor = SubtitleColor
            }
        };

        public static readonly GUIStyle DescriptionHeaderStyle = LoadDescriptionHeaderStyle();

        public static Color TitleLineColor = new(0f, 0f, 0f, 0.5f);

        private static GUIStyle LoadDescriptionHeaderStyle() {
            var style = EditorStyles.foldout;
            style.fontStyle = FontStyle.Bold;
            //style.contentOffset = new Vector2(10, 0);
            return style;
        }
    }
}
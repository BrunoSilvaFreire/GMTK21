using System;
using System.Collections.Generic;
using System.Linq;
using Shiroi.FX.Effects;
using UnityEditor;
using UnityEngine;
using Types = Lunari.Tsuki.Runtime.Types;

namespace Shiroi.FX.Editor.PopUp {
    public class EffectSelectorContent : PopupWindowContent {
        public const float Width = 300;
        private readonly Action<Type> onSelected;
        private readonly List<Type> types;

        public EffectSelectorContent(Action<Type> onSelected) {
            types = Types.GetAllTypesOf<Effect>().Where(type => type != typeof(Effect)).ToList();
            this.onSelected = onSelected;
        }

        public override Vector2 GetWindowSize() {
            return new(Width, types.Count * EditorGUIUtility.singleLineHeight);
        }

        public override void OnGUI(Rect rect) {
            for (var i = 0; i < types.Count; i++) {
                var type = types[i];
                if (GUI.Button(GetSubRect(rect, i), GetContent(type))) onSelected(type);
            }
        }

        private GUIContent GetContent(Type type) {
            return new(ObjectNames.NicifyVariableName(type.Name));
        }

        private static Rect GetSubRect(Rect rect, int index) {
            return new(rect.x, rect.y + index * EditorGUIUtility.singleLineHeight, Width,
                EditorGUIUtility.singleLineHeight);
        }
    }
}
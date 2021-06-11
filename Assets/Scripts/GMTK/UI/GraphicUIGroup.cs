using System.Collections.Generic;
using Lunari.Tsuki.Misc;
using UnityEngine;
using UnityEngine.UI;

namespace GMTK.UI {
    [ExecuteInEditMode]
    public class GraphicUIGroup : MonoBehaviour {
        public BooleanHistoric overrideColor;
        public Graphic[] graphics;
        public Color color;
        private readonly List<Color> backup = new();

        private void Update() {
            overrideColor.CopyCurrentToLast();
            if (!overrideColor) {
                if (overrideColor.JustDeactivated)
                    for (var i = 0; i < graphics.Length; i++)
                        graphics[i].color = backup[i];

                return;
            }

            if (overrideColor.JustActivated) {
                backup.Clear();
                foreach (var graphic in graphics) backup.Add(graphic.color);
            }

            foreach (var graphic in graphics) graphic.color = color;
        }

        public void EnableOverrideColor() {
            overrideColor.Current = true;
        }

        public void DisableOverrideColor() {
            overrideColor.Current = true;
        }
    }
}
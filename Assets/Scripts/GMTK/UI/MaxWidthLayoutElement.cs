using System;
using UnityEngine;
using UnityEngine.UI;

namespace GMTK.UI {
    public class MaxWidthLayoutElement : MonoBehaviour, ILayoutElement {
        public Component other;
        public float maxWidth;
        public int weight;

        private void Awake() {
            var rebuilder = other.gameObject.AddComponent<DelegateRebuilder>();
            rebuilder.onRebuild.AddListener(Rebuild);
        }

        private void OnValidate() {
            if (!(other is ILayoutElement)) other = other.gameObject.GetComponent<ILayoutElement>() as Component;
        }

        public void CalculateLayoutInputHorizontal() {
            if (other is HorizontalOrVerticalLayoutGroup g) g.CalculateLayoutInputHorizontal();
        }

        public void CalculateLayoutInputVertical() {
            if (other is HorizontalOrVerticalLayoutGroup g) g.CalculateLayoutInputVertical();
        }

        public float minWidth => DelegateOrDefault(element => element.minHeight);


        public float preferredWidth {
            get { return Mathf.Min(DelegateOrDefault(element => element.preferredWidth), maxWidth); }
        }

        public float flexibleWidth => DelegateOrDefault(element => element.flexibleHeight);

        public float minHeight => DelegateOrDefault(element => element.minHeight);

        public float preferredHeight => DelegateOrDefault(element => element.preferredHeight);

        public float flexibleHeight => DelegateOrDefault(element => element.flexibleHeight);

        public int layoutPriority => weight;

        private void Rebuild() {
            LayoutRebuilder.MarkLayoutForRebuild(transform as RectTransform);
        }

        private T DelegateOrDefault<T>(Func<ILayoutElement, T> func) {
            if (other is ILayoutElement e) return func(e);

            return default;
        }
    }
}
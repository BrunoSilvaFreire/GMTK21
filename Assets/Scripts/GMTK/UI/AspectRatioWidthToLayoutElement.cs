using UnityEngine;
using UnityEngine.UI;

namespace GMTK.UI {
    [ExecuteAlways]
    public class AspectRatioWidthToLayoutElement : MonoBehaviour {
        public AspectRatioFitter fitter;
        public LayoutElement element;

        private void Update() {
            if (fitter == null || element == null) return;

            var t = (RectTransform) fitter.transform;
            element.preferredWidth = t.rect.width;
        }
    }
}
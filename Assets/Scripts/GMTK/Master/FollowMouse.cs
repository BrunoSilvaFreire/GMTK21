using System;
using GMTK.Input;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GMTK.Master {
    public class FollowMouse : MonoBehaviour {
        [Required]
        public InputSource source;

        [Required]
        public new Camera camera;

        public float lerpAmount = 0.75F;
        public Vector3 offset;
        public float targetZ;
        public Vector3 positionOverride;
        public bool usePositionOverride;

        private void Update() {
            var mousePos = usePositionOverride ? positionOverride : source.GetMousePosition();
            mousePos.z = -camera.transform.position.z;
            mousePos += offset;
            var pos = mousePos;
            pos.z = targetZ;
            var rectTransform = (RectTransform) transform;
            rectTransform.anchoredPosition =
                Vector3.Lerp(rectTransform.anchoredPosition, pos, lerpAmount * Time.deltaTime);
        }
    }
}
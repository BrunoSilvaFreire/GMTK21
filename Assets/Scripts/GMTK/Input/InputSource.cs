using System;
using Lunari.Tsuki.Entities;
#if UNITY_EDITOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;
using UnityEngine.InputSystem;

namespace GMTK.Input {
    public class InputSource : Trait {
#if UNITY_EDITOR
        [Required]
#endif
        public Camera playerCamera;

#if UNITY_EDITOR
        [Required]
#endif
        public PlayerInput input;

        private InputAction click;
        private InputAction rightClick;

        private void Start() {
            click = input.actions["Click"];
            rightClick = input.actions["RightClick"];
        }

        public bool GetInteract() {
            //cringe
            return false;
        }

        public Vector3 GetMousePosition() {
            if (playerCamera == null) return Vector2.zero;


            return Mouse.current.position.ReadValue();
        }

        public bool GetMouseDown() {
            return click.WasPressedThisFrame();
        }

        public bool GetMouseUp() {
            return click.WasReleasedThisFrame();
        }
        
        public bool GetRightMouse() {
            return rightClick.IsPressed();
        }
    }
}
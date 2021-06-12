using System;
using Lunari.Tsuki.Entities;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GMTK.Input {
    public class InputSource : Trait {
        [Required]
        public Camera playerCamera;
    
        [Required]
        public PlayerInput input;

        private InputAction click;

        private void Start() {
            click = input.actions["Click"];
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
    }
}
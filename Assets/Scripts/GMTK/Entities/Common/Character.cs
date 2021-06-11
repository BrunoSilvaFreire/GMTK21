using System;
using System.Collections.Generic;
using Lunari.Tsuki.Entities;
using UnityEngine;
using UnityEngine.Events;

namespace GMTK.Entities.Common {
    [Serializable]
    public class CharacterEvent : UnityEvent<Character> {
    }


    public class Character : Trait {
        public static readonly HashSet<Character> AvailableCharacters = new HashSet<Character>();
        public static readonly UnityEvent onAvailableCharactersChanged = new UnityEvent();
        public static readonly CharacterEvent onCharacterBecameAvailable = new CharacterEvent();
        public static readonly CharacterEvent onCharacterBecameUnavailable = new CharacterEvent();
        public string alias;
        [ColorUsage(false, true)] public Color signatureColor;
        public Sprite icon;
        public bool controllable = true;
        public float voicePitch = 1;

        private void OnEnable() {
            AvailableCharacters.Add(this);
            onCharacterBecameAvailable.Invoke(this);
            onAvailableCharactersChanged.Invoke();
        }

        private void OnDisable() {
            AvailableCharacters.Remove(this);
            onCharacterBecameUnavailable.Invoke(this);
            onAvailableCharactersChanged.Invoke();
        }
    }
}
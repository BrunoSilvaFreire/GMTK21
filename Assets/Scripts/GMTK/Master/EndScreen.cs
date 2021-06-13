using System.Collections.Generic;
using System.Security.Cryptography;
using GMTK.Game;
using GMTK.Game.Traits;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

namespace GMTK.Master {
    public class EndScreen : MonoBehaviour {
        public PlayableDirector endCutscene;
        public TMP_Text finalText;
        private void Start() {
            Player.Instance.onGameFinished.AddListener(Finish);
        }

        private void Finish() {
            endCutscene.Play();
        }

        public void FinalizeEverything() {
            var instance = Player.Instance;
            finalText.text = "You killed " + instance.GetNumPeopleDead() + " people.";
            var neutrons = new List<Neutron>(Neutron.AllNeutrons);
            foreach (var allNeutron in neutrons) {
                allNeutron.gameObject.SetActive(false);
            }

            var happyAtoms = new List<HappyAtom>(HappyAtom.AllHappyAtoms);
            foreach (var allNeutron in happyAtoms) {
                allNeutron.gameObject.SetActive(false);
            }
        }
    }
}
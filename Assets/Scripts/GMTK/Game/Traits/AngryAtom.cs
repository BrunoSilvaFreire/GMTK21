using DG.Tweening;
using UnityEngine;

namespace GMTK.Game.Traits {
    public class AngryAtom : Atom {

        public float lifeTime = 4f;
        public float fadeTime = 0.5f;
        
        private void Start() {
            transform.DOScale(Vector3.zero, fadeTime).SetEase(Ease.InBounce).SetDelay(lifeTime).OnComplete(() => {
                Destroy(gameObject);
            });
        }
        protected override void OnCollisionWithNeutron(Collision collision) { }
    }
}
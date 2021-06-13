using System;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;
using Object = UnityEngine.Object;

namespace GMTK.Common {
    public class ExposedBindings : MonoBehaviour, IExposedPropertyTable {
        public List<ExposedBinding> bindings;

        public void SetReferenceValue(PropertyName id, Object value) {
            var binding = bindings.FirstOrDefault(b => b.Key == id);
            if (binding == null) {
                binding = new ExposedBinding(id, value);
                bindings.Add(binding);
            }
            else {
                binding.Value = value;
            }
        }

        public Object GetReferenceValue(PropertyName id, out bool idValid) {
            var found = bindings.FirstOrDefault(b => b.Key == id);
            idValid = found != null;
            return found?.Value;
        }

        public void ClearReferenceValue(PropertyName id) {
            bindings.RemoveAll(binding => binding.Key == id);
        }

        [Serializable]
        public class ExposedBinding {
            [SerializeField] private Object value;

            [SerializeField]
#if UNITY_EDITOR
            [DisplayAsString] 
#endif
            private PropertyName key;

            public ExposedBinding(PropertyName key, Object value) {
                this.key = key;
                this.value = value;
            }

            public PropertyName Key => key;

            public Object Value {
                get => value;
                set => this.value = value;
            }
        }
    }
}
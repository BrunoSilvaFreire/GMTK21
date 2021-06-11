using System;
using UnityEngine;

namespace GMTK.Common {
    public class MaterialPropertyBlockScope : IDisposable {
        public MaterialPropertyBlockScope(Renderer renderer) {
            Block = new MaterialPropertyBlock();
            Renderer = renderer;
            renderer.GetPropertyBlock(Block);
        }

        public MaterialPropertyBlock Block { get; }

        public Renderer Renderer { get; }

        public void Dispose() {
            Renderer.SetPropertyBlock(Block);
        }
    }
}
using System;
using TMPro;
using UnityEngine;

namespace GMTK.Cinematics.Dialogue {
    public class TextMeshProMeshScope : IDisposable {
        private readonly TMP_VertexDataUpdateFlags updateFlags;

        public TextMeshProMeshScope(TMP_VertexDataUpdateFlags updateFlags, TMP_Text text) {
            this.updateFlags = updateFlags;
            Text = text;
            Info = text.textInfo;
        }

        public TextMeshProMeshScope(TMP_VertexDataUpdateFlags updateFlags, TMP_TextInfo text) {
            this.updateFlags = updateFlags;
            Info = text;
        }

        public TMP_Text Text { get; }

        public TMP_TextInfo Info { get; }

        public void Dispose() {
            if (Text != null) Text.UpdateVertexData(updateFlags);
        }

        public ref TMP_MeshInfo MeshInfo(int index) {
            if (Text != null) return ref Text.textInfo.meshInfo[index];

            return ref Info.meshInfo[index];
        }

        public ref Vector3[] Vertices(int index) {
            return ref MeshInfo(index).vertices;
        }

        public ref Color32[] Colors(int index) {
            return ref MeshInfo(index).colors32;
        }

        public ref Color32 Color(int meshIndex, int colorIndex) {
            return ref MeshInfo(meshIndex).colors32[colorIndex];
        }


        public ref Vector3 Vertex(int meshIndex, int colorIndex) {
            return ref MeshInfo(meshIndex).vertices[colorIndex];
        }

        public void SetCharacterAlpha(int characterIndex, byte alpha) {
            ref var charInfo = ref Info.characterInfo[characterIndex];
            charInfo.vertex_BL.color.a = alpha;
            charInfo.vertex_TL.color.a = alpha;
            charInfo.vertex_TR.color.a = alpha;
            charInfo.vertex_BR.color.a = alpha;
            var characterInfo = Info.characterInfo[characterIndex];
            var materialIndex = characterInfo.materialReferenceIndex;
            for (var j = 0; j < 4; j++) {
                var k = characterInfo.vertexIndex + j;
                Color(materialIndex, k).a = alpha;
            }
        }

        public void AddXPosition(int characterIndex, float f) {
            AddPosition(characterIndex, f, 0);
        }

        public void AddYPosition(int characterIndex, float f) {
            AddPosition(characterIndex, 0, f);
        }

        public void AddPosition(int characterIndex, float x, float y) {
            ref var charInfo = ref Info.characterInfo[characterIndex];
            var materialIndex = charInfo.materialReferenceIndex;
            var vertices = new[] {
                charInfo.bottomLeft,
                charInfo.topLeft,
                charInfo.topRight,
                charInfo.bottomRight
            };
            for (var j = 0; j < 4; j++) {
                var k = charInfo.vertexIndex + j;
                ref var vec = ref Vertex(materialIndex, k);
                vec.y = vertices[j].y + y;
                vec.x = vertices[j].x + x;
            }
        }
    }
}
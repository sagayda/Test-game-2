using Map.View;
using UnityEngine;

namespace Map.Model
{
    public interface IMapTextureGeneratingStrategy
    {
        public MapSpritesWrapper GenerateAndWrapMapTextures(Transform parentForSprites);
    }
}

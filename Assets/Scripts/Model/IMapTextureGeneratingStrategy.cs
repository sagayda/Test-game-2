using Assets.Scripts.View;
using UnityEngine;

namespace Assets.Scripts.Model
{
    public interface IMapTextureGeneratingStrategy
    {
        public MapSpritesWrapper GenerateAndWrapMapTextures(Transform parentForSprites);
    }
}

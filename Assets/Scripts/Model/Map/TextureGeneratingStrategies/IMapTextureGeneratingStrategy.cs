using Assets.Scripts.View;
using UnityEngine;

namespace Assets.Scripts.Model.Map
{
    public interface IMapTextureGeneratingStrategy
    {
        public MapSpritesWrapper GenerateAndWrapMapTextures(Transform parentForSprites);
    }
}

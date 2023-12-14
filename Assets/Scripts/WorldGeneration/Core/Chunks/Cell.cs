using Assets.Scripts.WorldGeneration.Core.Maps;
using UnityEngine;
using WorldGeneration.Core;

namespace Assets.Scripts.WorldGeneration.Core.Chunks
{
    public class Cell : IMapArea
    {
        public Vector2Int Position { get; }
        public ValueMapPoint Values { get; }
        public WaterMapPoint Water { get; }

        public Cell(Vector2Int position)
        {
            Position = position;
            Values = new(position.x, position.y);
        }
    }
}

using WorldGeneration.Core.Maps;
using UnityEngine;

namespace WorldGeneration.Core.Chunks
{
    public class Cell : IMapArea
    {
        public Vector2Int Position { get; }
        public ValueMapPoint Values { get; }
        public WaterMapPoint Water { get; }

        public bool HasWater => Water != null;

        public Cell(Vector2Int position)
        {
            Position = position;
            Values = new();
        }
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

namespace WorldGeneration.Core.WaterBehavior
{
    public class WaterMap
    {
        private readonly WorldGenerator _worldGenerator;
        private readonly Dictionary<Vector2Int, WaterCell> _waterMap = new();

        public WaterMap(WorldGenerator worldGenerator)
        {
            if (worldGenerator == null)
                throw new ArgumentException("World generator can't be null!");

            _worldGenerator = worldGenerator;
        }

        public WaterCell this[Vector2Int position]
        {
            get
            {
                if(_waterMap.TryGetValue(position, out WaterCell cell))
                {
                    return cell;
                }

                WaterCell newCell = new(position, _worldGenerator.GetMapValue(position, MapValueType.Height));
                _waterMap.Add(position, newCell);

                return newCell;
            }
        }
    }
}

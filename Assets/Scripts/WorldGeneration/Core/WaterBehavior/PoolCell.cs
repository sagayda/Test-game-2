using UnityEngine;

namespace WorldGeneration.Core.WaterBehavior
{
    public class PoolCell : WaterCell
    {
        public PoolCell(Pool pool, Vector2Int position, float landLevel, float volume) : base(position, landLevel, volume)
        {
            Pool = pool;
        }

        public Pool Pool { get; private set; }
    }
}

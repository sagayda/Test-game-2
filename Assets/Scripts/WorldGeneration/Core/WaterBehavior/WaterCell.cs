using UnityEngine;

namespace WorldGeneration.Core.WaterBehavior
{
    public class WaterCell
    {
        public WaterCell(Vector2Int position, float landLevel)
        {
            Position = position;
            Volume = 0;
            LandLevel = landLevel;
        }

        public WaterCell(Vector2Int position, float landLevel, float volume)
        {
            Position = position;
            LandLevel= landLevel;
            Volume = volume;
        }

        //fix
        public WaterCell()
        {
            
        }

        public Vector2Int Position { get; }
        public float LandLevel { get; }
        public float Volume { get; set; }

        public override int GetHashCode()
        {
            return Position.GetHashCode();
        }
    }
}

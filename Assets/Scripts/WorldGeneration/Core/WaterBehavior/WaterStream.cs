using UnityEngine;

namespace WorldGeneration.Core.WaterBehavior
{
    public class WaterStream : WaterCell
    {
        public Vector2 Stream { get; set; }

        public WaterStream(Vector2Int position, float landLevel, Vector2 stream) : base(position, landLevel)
        {
            Stream = stream;
        }

        public WaterStream(Vector2Int position, float landLevel, Vector2 stream, float volume) : base(position, landLevel, volume)
        {
            Stream = stream;
        }

        //fix
        public WaterStream()
        {
            
        }
    }
}

using UnityEngine;

namespace WorldGeneration.Core.WaterBehavior
{
    public class WaterSourceOUTDATE : WaterStream
    {
        protected WaterSourceOUTDATE(Vector2Int position, float landLevel, Vector2 stream) : base(position, landLevel, stream) 
        {
            
        }

        public WaterSourceOUTDATE(Vector2Int position, float landLevel, float strength, Vector2 stream) : base(position, landLevel, stream) 
        {
            Strength = strength;
        }

        public WaterSourceOUTDATE(Vector2Int position, float landLevel, float strength, Vector2 stream, float volume) : base(position, landLevel, stream, volume)
        {
            Strength = strength;
        }

        //fix
        public WaterSourceOUTDATE(float a, Vector2 b)
        {
            
        }

        /// <summary>
        /// Water amount per iteration
        /// </summary>
        public bool HasLeakage { get; set; } = false;
        public virtual float Strength { get; private set; }
    }
}

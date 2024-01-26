using System;
using UnityEngine;

namespace WorldGeneration.Core.WaterBehavior
{
    public class CombinedWaterSource : WaterSourceOUTDATE
    {
        public CombinedWaterSource(Vector2Int position, float landLevel, Vector2 stream, params CombinedWaterSource[] nativeSource) : base(position, landLevel, stream)
        {
            if (nativeSource == null || nativeSource.Length <= 0)
                throw new ArgumentException("Invalid sources!");

            NativeSources = nativeSource;
        }

        public CombinedWaterSource(Vector2Int position, float landLevel, Vector2 stream, float volume, params CombinedWaterSource[] nativeSource) : base(position, landLevel, stream)
        {
            if (nativeSource == null || nativeSource.Length <= 0)
                throw new ArgumentException("Invalid sources!");

            NativeSources = nativeSource;
        }

        public CombinedWaterSource[] NativeSources { get; private set; }
        public override float Strength => GetStrength();

        private float GetStrength()
        {
            float strength = 0f;

            foreach (var source in NativeSources)
            {
                strength += source.Strength;
            }

            return strength;
        }
    }
}

using System;

namespace WorldGeneration.Core
{
    [Serializable]
    public class EaseInQuad : IEasingStrategy
    {
        public float Ease(float value)
        {
            return value * value;
        }
    }
}

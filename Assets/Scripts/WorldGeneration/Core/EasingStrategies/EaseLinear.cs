using System;

namespace WorldGeneration.Core
{
    [Serializable]
    public class EaseLinear : IEasingStrategy
    {
        public float Ease(float value)
        {
            return value;
        }
    }
}

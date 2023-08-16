using System;

namespace Assets.Scripts.Model
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

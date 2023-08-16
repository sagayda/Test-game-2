using System;

namespace Assets.Scripts.Model
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

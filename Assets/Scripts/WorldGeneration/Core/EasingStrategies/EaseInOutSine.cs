using System;
using UnityEngine;

namespace WorldGeneration.Core
{
    [Serializable]
    public class EaseInOutSine : IEasingStrategy
    {
        public float Ease(float value)
        {
            return -(Mathf.Cos(Mathf.PI * value) - 1) / 2;
        }
    }
}

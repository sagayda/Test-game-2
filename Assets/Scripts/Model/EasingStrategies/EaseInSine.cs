using System;
using UnityEngine;

namespace Assets.Scripts.Model
{
    [Serializable]
    public class EaseInSine : IEasingStrategy
    {
        public float Ease(float value)
        {
            return 1 - Mathf.Cos((value * Mathf.PI) / 2);
        }
    }
}

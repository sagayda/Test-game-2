using UnityEngine;

namespace Assets.Scripts.Model.WorldGeneration
{
    public class EasyInOutSine : IEasingStrategy
    {
        public float Ease(float value)
        {
            return -(Mathf.Cos(Mathf.PI * value) - 1) / 2;
        }
    }
}

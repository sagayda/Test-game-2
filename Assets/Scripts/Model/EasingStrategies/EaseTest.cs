using System;

namespace Assets.Scripts.Model
{
    public class EaseTest : IEasingStrategy
    {
        private float degree1 = 2f;
        private float degree2 = 1.5f;
        private float degree3 = 1f;
        private float degree4 = 2f;

        public float Ease(float value)
        {
            if (value < 0.5f)
            {
                value *= 2;
                float res = EaseByDegree(value, degree1, degree2);
                return res / 2f;
            }
            else
            {
                value *= 2;
                value -= 1;
                float res = EaseByDegree(value, degree3, degree4);
                res += 1;
                return res / 2f;
            }
        }

        private float EaseByDegree(float value, float degree1, float degree2)
        {
            return value > 0.5f ? 1f - MathF.Pow(-2f * value + 2f, degree2) / 2f : MathF.Pow(value, degree1) * MathF.Pow(2, degree1 - 1);
        }
    }
}

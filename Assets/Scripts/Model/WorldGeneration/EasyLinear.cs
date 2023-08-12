namespace Assets.Scripts.Model.WorldGeneration
{
    public class EasyLinear : IEasingStrategy
    {
        public float Ease(float value)
        {
            return value;
        }
    }
}

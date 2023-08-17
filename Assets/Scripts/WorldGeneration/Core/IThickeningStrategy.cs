namespace Assets.Scripts.WorldGeneration.Core
{
    public interface IThickeningStrategy
    {
        public float Thicken(float maxThickness, float minThickness, float t);
    }
}

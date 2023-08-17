namespace Assets.Scripts.WorldGeneration.Core
{
    public class LinearThicken : IThickeningStrategy
    {
        public float Thicken(float maxThickness, float minThickness, float t)
        {
            float thickness = maxThickness - minThickness;
            thickness *= t;
            return thickness + minThickness;
        }
    }
}

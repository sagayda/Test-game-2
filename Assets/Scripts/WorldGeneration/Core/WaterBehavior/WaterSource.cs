namespace Assets.Scripts.WorldGeneration.Core.WaterBehavior
{
    public class WaterSource
    {
        public float Strength { get; private set; }

        public WaterSource(float strength)
        {
            Strength = strength;
        }
    }
}

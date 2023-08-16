namespace Assets.Scripts.Model.WorldGeneration
{
    public class PerlinWormsParameters
    {
        public OctaveNoiseParameters Noise { get; private set; }

        public PerlinWormsParameters(OctaveNoiseParameters noiseParameters)
        {
            Noise = noiseParameters;
        }
    }
}

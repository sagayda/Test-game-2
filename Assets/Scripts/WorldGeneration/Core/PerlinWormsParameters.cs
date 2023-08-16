namespace WorldGeneration.Core
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

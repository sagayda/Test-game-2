using WorldGeneration.Core.Noise;

namespace WorldGeneration.Core
{
    public class PerlinWormsParameters
    {
        public FractalNoiseParameters Noise { get; private set; }

        public PerlinWormsParameters(FractalNoiseParameters noiseParameters)
        {
            Noise = noiseParameters;
        }
    }
}

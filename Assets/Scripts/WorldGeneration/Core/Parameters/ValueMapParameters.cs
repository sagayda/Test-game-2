using WorldGeneration.Core.Noise;

namespace WorldGeneration.Core.Parameters
{
    public interface IValueMapParameters : IParameters
    {
        public FractalNoiseParameters Noise { get; }
    }
}

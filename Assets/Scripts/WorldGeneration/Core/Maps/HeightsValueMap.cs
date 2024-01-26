using UnityEngine;
using WorldGeneration.Core.Noise;

namespace WorldGeneration.Core
{
    public class HeightsValueMap : IValueMap
    {
        private HeightsMapParameters _parameters;
        private FractalNoise _noiseProvider;

        public HeightsValueMap(HeightsMapParameters parameters)
        {
            _parameters = parameters;
            _noiseProvider = new(_parameters.Noise);
        }

        public int Seed
        {
            get => _noiseProvider.Seed;
            set => _noiseProvider.Seed = value;
        }
        public int Priority => 100;
        public MapValueType ValueType => MapValueType.Height;

        public void SetParameters(HeightsMapParameters parameters)
        {
            _parameters = parameters;
            _noiseProvider = new(_parameters.Noise);
        }

        public ValueMapPoint ComputeValue(ValueMapPoint point, Vector2 position)
        {
            float noise = _noiseProvider.Generate(position);
            return point.SetValue(MapValueType.Height, noise);
        }
    }
}

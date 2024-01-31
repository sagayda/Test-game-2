using UnityEngine;
using WorldGeneration.Core.Noise;

namespace WorldGeneration.Core.Maps
{
    public class ProgressValueMap : IValueMap
    {
        private ProgressMapParameters _parameters;
        private FractalNoise _noiseProvider;

        public ProgressValueMap(ProgressMapParameters parameters)
        {
            _parameters = parameters;
            _noiseProvider = new(_parameters.Noise);
        }

        public int Seed
        {
            get => _noiseProvider.Seed;
            set => _noiseProvider.Seed = value;
        }
        public int Priority => 20;
        public MapValueType ValueType => MapValueType.Progress;

        public void SetParameters(ProgressMapParameters parameters)
        {
            _parameters = parameters;
            _noiseProvider = new(_parameters.Noise);
        }

        public ValueMapPoint ComputeValue(ValueMapPoint mapPoint, Vector2 position)
        {
            float progress = _noiseProvider.Generate(position);

            return mapPoint.SetValue(MapValueType.Progress, progress);
        }
    }
}

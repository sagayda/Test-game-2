using UnityEngine;
using WorldGeneration.Core.Noise;

namespace WorldGeneration.Core.Maps
{
    public class PolutionValueMap : IValueMap
    {
        private PolutionMapParameters _parameters;
        private FractalNoise _noiseProvider;

        public PolutionValueMap(PolutionMapParameters parameters)
        {
            _parameters = parameters;
            _noiseProvider = new(_parameters.Noise);
        }

        public int Seed
        {
            get => _noiseProvider.Seed;
            set => _noiseProvider.Seed = value;
        }
        public int Priority => 1;
        public MapValueType ValueType => MapValueType.Polution;

        public void SetParameters(PolutionMapParameters parameters)
        {
            _parameters = parameters;
            _noiseProvider = new(_parameters.Noise);
        }

        public ValueMapPoint ComputeValue(ValueMapPoint mapPoint)
        {
            float progress = mapPoint[MapValueType.Progress];

            float polution = _noiseProvider.Generate(mapPoint.Position);

            float polutionMultiplyer = Mathf.Pow(progress * _parameters.ProgressImpactMultiplyer, _parameters.ProgressImpactStrength);
            polutionMultiplyer = Mathf.Clamp(polutionMultiplyer, _parameters.ProgressImpactBottom, _parameters.ProgressImpactTop);
            polution *= polutionMultiplyer;

            return mapPoint.SetValue(MapValueType.Polution, polution);
        }
    }
}

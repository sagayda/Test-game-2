using UnityEngine;
using WorldGeneration.Core;

namespace WorldGeneration.Editor
{
    [CreateAssetMenu(fileName = "PolutionParams", menuName = "World generator/Create polution params")]
    [ExecuteInEditMode]
    public class PolutionParametersBuilder : BaseParametersBuilder<PolutionNoiseParameters>
    {
        public float ProgressImpactStrength;
        public float ProgressImpactMultiplyer;
        public float ProgressImpactBottom;
        public float ProgressImpactTop;

        public override PolutionNoiseParameters LastBuilded { get => _lastBuilded; set => _lastBuilded = value; }
        private PolutionNoiseParameters _lastBuilded;

        private void OnValidate()
        {
            _lastBuilded = Build();
        }

        public override PolutionNoiseParameters Build()
        {
            return new PolutionNoiseParameters(BuildBase(), ProgressImpactStrength, ProgressImpactMultiplyer, ProgressImpactBottom, ProgressImpactTop);
        }

        public override void Load()
        {
            NoiseParametersSave parametersSave = new NoiseParametersSave();

            PolutionNoiseParameters loadedParams = parametersSave.LoadNoiseParameters<PolutionNoiseParameters>();

            LoadBase(loadedParams.Noise);

            ProgressImpactStrength = loadedParams.ProgressImpactStrength;
            ProgressImpactMultiplyer = loadedParams.ProgressImpactMultiplyer;
            ProgressImpactBottom = loadedParams.ProgressImpactBottom;
            ProgressImpactTop = loadedParams.ProgressImpactTop;

            _lastBuilded = loadedParams;
        }
    }
}

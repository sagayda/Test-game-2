using UnityEngine;
using WorldGeneration.Core;

namespace WorldGeneration.Editor
{
    [CreateAssetMenu(fileName = "TemperatureParams", menuName = "World generator/Create temperature params")]
    [ExecuteInEditMode]
    public class TemperatureParametersBuilder : BaseParametersBuilder<TemperatureNoiseParameters>
    {
        public float HeightImpactStrength;
        public float HeightImpactSmoothing;
        public float NoiseImpact;
        public float GlobalTemperature;

        public override TemperatureNoiseParameters LastBuilded { get => _lastBuilded; set => _lastBuilded = value; }
        private TemperatureNoiseParameters _lastBuilded;

        private void OnValidate()
        {
            _lastBuilded = Build();
        }

        public override TemperatureNoiseParameters Build()
        {
            return new TemperatureNoiseParameters(BuildBase(), HeightImpactStrength, HeightImpactSmoothing, NoiseImpact, GlobalTemperature);
        }

        public override void Load()
        {
            NoiseParametersSave parametersSave = new NoiseParametersSave();

            TemperatureNoiseParameters loadedParams = parametersSave.LoadNoiseParameters<TemperatureNoiseParameters>();

            LoadBase(loadedParams.Noise);

            HeightImpactStrength = loadedParams.HeightImpactStrength;
            HeightImpactSmoothing = loadedParams.HeightImpactSmoothing;
            NoiseImpact = loadedParams.NoiseImpact;
            GlobalTemperature = loadedParams.GlobalTemperature;

            _lastBuilded = loadedParams;
        }
    }
}

using UnityEngine;
using WorldGeneration.Core;

namespace WorldGeneration.Editor
{
    [CreateAssetMenu(fileName = "HeightParams", menuName = "World generator/Create hight params")]
    [ExecuteInEditMode]
    public class HeightParametersBuilder : BaseParametersBuilder<HeightsNoiseParameters>
    {
        [Range(0, 1)] public float WaterLevel;

        public override HeightsNoiseParameters LastBuilded { get => _lastBuilded; set => LastBuilded = value; }
        private HeightsNoiseParameters _lastBuilded;

        private void OnValidate()
        {
            _lastBuilded = Build();
        }

        public override HeightsNoiseParameters Build()
        {
            return new HeightsNoiseParameters(BuildBase(), WaterLevel);
        }

        public override void Load()
        {
            NoiseParametersSave parametersSave = new NoiseParametersSave();

            HeightsNoiseParameters loadedParams = parametersSave.LoadNoiseParameters<HeightsNoiseParameters>();

            LoadBase(loadedParams.Noise);

            WaterLevel = loadedParams.WaterLevel;

            _lastBuilded = loadedParams;
        }
    }
}

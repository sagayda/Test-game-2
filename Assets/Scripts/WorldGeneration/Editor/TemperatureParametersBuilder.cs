using NaughtyAttributes;
using UnityEngine;
using WorldGeneration.Core;

namespace WorldGeneration.Editor
{
    [CreateAssetMenu(fileName = "TemperatureParams", menuName = "World generator/Create temperature parameters")]
    [ExecuteInEditMode]
    public class TemperatureParametersBuilder : FractalNoiseParametersBuilder
    {
        private CompositeValueMap _compositeMap;

        [ShowIf("EnableVisualizing")]
        [BoxGroup("Visualizing")]
        public ParametersSave.SaveSlot SlotForGenerator;
        [Header("Temperature parameters")]
        public float HeightImpactStrength;
        public float HeightImpactSmoothing;
        public float NoiseImpact;
        public float EquatorCoordinate;
        public float GlobalTemperature;

        public TemperatureMapParameters GenerationParameters => Build();

        private TemperatureMapParameters Build()
        {
            return new TemperatureMapParameters(NoiseParameters, HeightImpactStrength, HeightImpactSmoothing, NoiseImpact, EquatorCoordinate, GlobalTemperature);
        }

        protected override float GetNoise(int x, int y)
        {
            return _compositeMap.ComputeValues(new Vector2(x, y))[MapValueType.Temperature];

            //return _worldGenerator.GetTemperatureValue(x, y);
        }

        protected override void SetPaintingParameters()
        {
            HeightsValueMap heightsMap = new(ParametersSave.LoadParametersOrDefault<HeightsMapParameters>(SlotForGenerator));
            TemperatureValueMap temperatureMap = new(GenerationParameters);

            _compositeMap = new(heightsMap, temperatureMap);

            //_worldGenerator = new(new(Seed,
            //  Width,
            //  Height,
            //  ParametersSave.LoadParametersOrDefault<ProgressGenerationParameters>(SlotForGenerator),
            //  ParametersSave.LoadParametersOrDefault<PolutionGenerationParameters>(SlotForGenerator),
            //  ParametersSave.LoadParametersOrDefault<HeightsGenerationParameters>(SlotForGenerator),
            //  GenerationParameters));
        }

        protected override void Save()
        {
            ParametersSave.SaveParameters(GenerationParameters, SaveSlot);
        }

        protected override void Load()
        {
            var loaded = ParametersSave.LoadParameters<TemperatureMapParameters>(SaveSlot);

            if (loaded.HasValue)
            {
                HeightImpactStrength = loaded.Value.HeightImpactStrength;
                HeightImpactSmoothing = loaded.Value.HeightImpactSmoothing;
                NoiseImpact = loaded.Value.NoiseImpactStrength;
                GlobalTemperature = loaded.Value.GlobalTemperature;

                XSeedStep = loaded.Value.Noise.XSeedStep;
                YSeedStep = loaded.Value.Noise.YSeedStep;
                Octaves = loaded.Value.Noise.Octaves;
                Amplitude = loaded.Value.Noise.Amplitude;
                Frequency = loaded.Value.Noise.Frequency;
                Persistance = loaded.Value.Noise.Persistance;
                Lacunarity = loaded.Value.Noise.Lacunarity;
            }
        }

        protected override Color GetColor(float noise)
        {
            Color color;

            if (noise <= 0.25f)
                color = StepwiseColorLerp(Color.white, Color.blue, 0, 0.25f, noise);
            else if (noise <= 0.5f)
                color = StepwiseColorLerp(Color.blue, Color.green, 0.25f, 0.5f, noise);
            else if (noise <= 0.75f)
                color = StepwiseColorLerp(Color.green, Color.yellow, 0.5f, 0.75f, noise);
            else
                color = StepwiseColorLerp(Color.yellow, Color.red, 0.75f, 1f, noise);

            return color;
        }
    }
}

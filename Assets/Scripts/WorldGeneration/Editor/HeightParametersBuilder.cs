using NaughtyAttributes;
using UnityEngine;
using WorldGeneration.Core;

namespace WorldGeneration.Editor
{
    [CreateAssetMenu(fileName = "HeightParams", menuName = "World generator/Create height parameters")]
    [ExecuteInEditMode]
    public class HeightParametersBuilder : FractalNoiseParametersBuilder
    {
        private CompositeValueMap _compositeMap;

        #region Editor fields
        [ShowIf("EnableVisualizing")]
        [BoxGroup("Visualizing")]
        public ParametersSave.SaveSlot SlotForGenerator;

        [Header("Height parameters")]
        [Range(0, 1)] public float WaterLevel;
        #endregion

        public HeightsMapParameters GenerationParameters => Build();

        private HeightsMapParameters Build()
        {
            return new(NoiseParameters, WaterLevel);
        }

        protected override float GetNoise(int x, int y)
        {
            return _compositeMap.ComputeValues(new(x,y))[MapValueType.Height];

            //return WorldGenerator.GetHeightValue(x, y);
        }

        protected override void SetPaintingParameters()
        {
            _compositeMap = new(new HeightsValueMap(GenerationParameters));

            _compositeMap.SetSeed(WorldGenerator.ComputeInt32Seed(Seed));


            //WorldGenerator.Parameters = new(Seed,
            //                          Width,
            //                          Height,
            //                          ParametersSave.LoadParametersOrDefault<ProgressGenerationParameters>(SlotForGenerator),
            //                          ParametersSave.LoadParametersOrDefault<PolutionGenerationParameters>(SlotForGenerator),
            //                          GenerationParameters,
            //                          ParametersSave.LoadParametersOrDefault<TemperatureGenerationParameters>(SlotForGenerator));
        }

        protected override void Save()
        {
            ParametersSave.SaveParameters(GenerationParameters, SaveSlot);
        }

        protected override void Load()
        {
            var loaded = ParametersSave.LoadParameters<HeightsMapParameters>(SaveSlot);

            if (loaded.HasValue)
            {
                WaterLevel = loaded.Value.WaterLevel;

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

            float landSize = 1 - WaterLevel;
            float landStep = landSize / 3;

            if (noise <= WaterLevel)
                color = StepwiseColorLerp(Color.black, new Color(0f, 0.4f, 1f), 0, WaterLevel, noise);
            else if (noise <= WaterLevel + landStep)
                color = StepwiseColorLerp(new Color(0.5f, 0.95f, 0f), new Color(0, 0.4f, 0), WaterLevel, WaterLevel + landStep, noise);
            else if (noise <= WaterLevel + landStep * 2)
                color = StepwiseColorLerp(new Color(0, 0.4f, 0), new Color(0.55f, 0.55f, 0), WaterLevel + landStep, WaterLevel + landStep * 2, noise);
            else
                color = StepwiseColorLerp(new Color(0.55f, 0.55f, 0), new Color(0.45f, 0f, 0f), WaterLevel + landStep * 2, 1, noise);

            return color;
        }
    }
}

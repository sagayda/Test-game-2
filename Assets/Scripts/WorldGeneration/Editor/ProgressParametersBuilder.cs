using NaughtyAttributes;
using UnityEngine;
using WorldGeneration.Core;

namespace WorldGeneration.Editor
{
    [CreateAssetMenu(fileName = "ProgressParams", menuName = "World generator/Create progress parameters")]
    [ExecuteInEditMode]
    public class ProgressParametersBuilder : FractalNoiseParametersBuilder
    {
        private CompositeValueMap _compositeMap;

        [ShowIf("EnableVisualizing")]
        [BoxGroup("Visualizing")]
        public ParametersSave.SaveSlot SlotForGenerator;

        public ProgressMapParameters GenerationParameters => Build();

        protected override float GetNoise(int x, int y)
        {
            return _compositeMap.ComputeValues(new(x,y))[MapValueType.Progress];
            //return _worldGenerator.GetProgressValue(x, y);
        }

        protected override void SetPaintingParameters()
        {
            _compositeMap = new(new ProgressValueMap(GenerationParameters));

            //_worldGenerator = new(new(Seed,
            //              Width,
            //              Height,
            //              GenerationParameters,
            //              ParametersSave.LoadParametersOrDefault<PolutionGenerationParameters>(SlotForGenerator),
            //              ParametersSave.LoadParametersOrDefault<HeightsGenerationParameters>(SlotForGenerator),
            //              ParametersSave.LoadParametersOrDefault<TemperatureGenerationParameters>(SlotForGenerator)));
        }

        private ProgressMapParameters Build()
        {
            return new ProgressMapParameters(NoiseParameters);
        }

        protected override void Save()
        {
            ParametersSave.SaveParameters(GenerationParameters, SaveSlot);
        }

        protected override void Load()
        {
            var loaded = ParametersSave.LoadParameters<ProgressMapParameters>(SaveSlot);

            if (loaded.HasValue)
            {
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
            return StepwiseColorLerp(Color.white, Color.red, 0, 1, noise);
        }
    }
}

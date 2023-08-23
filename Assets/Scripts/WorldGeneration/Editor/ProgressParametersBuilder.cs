using NaughtyAttributes;
using UnityEngine;
using WorldGeneration.Core;

namespace WorldGeneration.Editor
{
    [CreateAssetMenu(fileName = "ProgressParams", menuName = "World generator/Create progress parameters")]
    [ExecuteInEditMode]
    public class ProgressParametersBuilder : FractalNoiseParametersBuilder
    {
        private WorldGenerator _worldGenerator;

        [ShowIf("EnableVisualizing")]
        [BoxGroup("Visualizing")]
        public ParametersSave.SaveSlot SlotForGenerator;

        public ProgressGenerationParameters GenerationParameters => Build();

        protected override float GetNoise(int x, int y)
        {
            return _worldGenerator.GetProgressValue(x, y);
        }

        protected override void SetPaintingParameters()
        {
            _worldGenerator = new(new(Seed,
                          Width,
                          Height,
                          GenerationParameters,
                          ParametersSave.LoadParametersOrDefault<PolutionGenerationParameters>(SlotForGenerator),
                          ParametersSave.LoadParametersOrDefault<HeightsGenerationParameters>(SlotForGenerator),
                          ParametersSave.LoadParametersOrDefault<TemperatureGenerationParameters>(SlotForGenerator)));
        }

        private ProgressGenerationParameters Build()
        {
            return new ProgressGenerationParameters(NoiseParameters);
        }

        protected override void Save()
        {
            ParametersSave.SaveParameters(GenerationParameters, SaveSlot);
        }

        protected override void Load()
        {
            var loaded = ParametersSave.LoadParameters<ProgressGenerationParameters>(SaveSlot);

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

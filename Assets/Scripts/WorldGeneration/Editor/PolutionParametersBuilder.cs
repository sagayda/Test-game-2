using NaughtyAttributes;
using UnityEngine;
using WorldGeneration.Core;

namespace WorldGeneration.Editor
{
    [CreateAssetMenu(fileName = "PolutionParams", menuName = "World generator/Create polution parameters")]
    [ExecuteInEditMode]
    public class PolutionParametersBuilder : FractalNoiseParametersBuilder
    {
        private WorldGenerator _worldGenerator;

        #region Editor fields
        [ShowIf("EnableVisualizing")]
        [BoxGroup("Visualizing")]
        public ParametersSave.SaveSlot SlotForGenerator;
        [Header("Polution parameters")]
        public float ProgressImpactStrength;
        public float ProgressImpactMultiplyer;
        public float ProgressImpactBottom;
        public float ProgressImpactTop;
        #endregion

        public PolutionGenerationParameters GenerationParameters => Build();

        private PolutionGenerationParameters Build()
        {
            return new PolutionGenerationParameters(NoiseParameters, ProgressImpactStrength, ProgressImpactMultiplyer, ProgressImpactBottom, ProgressImpactTop);
        }

        protected override float GetNoise(int x, int y)
        {
            return _worldGenerator.GetPolutionValue(x, y);
        }

        protected override void SetPaintingParameters()
        {
            _worldGenerator = new(new(Seed, Width, Height,
                ParametersSave.LoadParametersOrDefault<ProgressGenerationParameters>(SlotForGenerator),
                GenerationParameters,
                ParametersSave.LoadParametersOrDefault<HeightsGenerationParameters>(SlotForGenerator),
                ParametersSave.LoadParametersOrDefault<TemperatureGenerationParameters>(SlotForGenerator)));
        }

        protected override void Save()
        {
            ParametersSave.SaveParameters(GenerationParameters, SaveSlot);
        }

        protected override void Load()
        {
            var loaded = ParametersSave.LoadParameters<PolutionGenerationParameters>(SaveSlot);

            if (loaded.HasValue)
            {
                ProgressImpactStrength = loaded.Value.ProgressImpactStrength;
                ProgressImpactMultiplyer = loaded.Value.ProgressImpactMultiplyer;
                ProgressImpactBottom = loaded.Value.ProgressImpactBottom;
                ProgressImpactTop = loaded.Value.ProgressImpactTop;

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
            return StepwiseColorLerp(Color.white, Color.green, 0, 1, noise);
        }
    }
}

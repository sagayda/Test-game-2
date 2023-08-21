using UnityEngine;
using WorldGeneration.Core;

namespace WorldGeneration.Editor
{
    [CreateAssetMenu(fileName = "PolutionParams", menuName = "World generator/Create polution parameters")]
    [ExecuteInEditMode]
    public class PolutionParametersBuilder : FractalNoiseParametersBuilder
    {
        [Header("Polution parameters")]
        public float ProgressImpactStrength;
        public float ProgressImpactMultiplyer;
        public float ProgressImpactBottom;
        public float ProgressImpactTop;

        public PolutionGenerationParameters GenerationParameters => Build();

        private PolutionGenerationParameters Build()
        {
            return new PolutionGenerationParameters(NoiseParameters, ProgressImpactStrength, ProgressImpactMultiplyer, ProgressImpactBottom, ProgressImpactTop);
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

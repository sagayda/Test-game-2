using System;

namespace WorldGeneration.Core
{
    [Serializable]
    public struct GeneratorParameters
    {
        public readonly string Seed;
        public readonly int IntSeed;
        public readonly uint WorldWidth;
        public readonly uint WorldHeight;

        public ProgressGenerationParameters Progress { get; set; }
        public PolutionGenerationParameters Polution { get; set; }
        public HeightsGenerationParameters Heights { get; set; }
        public TemperatureGenerationParameters Temperature { get; set; }

        public GeneratorParameters(string seed, uint width, uint height, ProgressGenerationParameters progress, PolutionGenerationParameters polution, HeightsGenerationParameters heights, TemperatureGenerationParameters temperature)
        {
            Seed = seed;
            IntSeed = WorldGenerator.ComputeInt32Seed(Seed);

            WorldWidth = width;
            WorldHeight = height;
            Progress = progress;
            Polution = polution;
            Heights = heights;
            Temperature = temperature;
        }

        public GeneratorParameters(string seed, uint width, uint height, ParametersSave.SaveSlot slotToLoad = ParametersSave.SaveSlot.Default)
        {
            Seed = seed;
            IntSeed = WorldGenerator.ComputeInt32Seed(Seed);

            WorldWidth = width;
            WorldHeight = height;

            Progress = ParametersSave.LoadParametersOrDefault<ProgressGenerationParameters>(slotToLoad);
            Polution = ParametersSave.LoadParametersOrDefault<PolutionGenerationParameters>(slotToLoad);
            Temperature = ParametersSave.LoadParametersOrDefault<TemperatureGenerationParameters>(slotToLoad);
            Heights = ParametersSave.LoadParametersOrDefault<HeightsGenerationParameters>(slotToLoad);
        }

    }
}

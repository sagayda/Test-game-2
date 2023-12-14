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

        public ProgressMapParameters Progress { get; set; }
        public PolutionMapParameters Polution { get; set; }
        public HeightsMapParameters Heights { get; set; }
        public TemperatureMapParameters Temperature { get; set; }

        public GeneratorParameters(string seed, uint width, uint height, ProgressMapParameters progress, PolutionMapParameters polution, HeightsMapParameters heights, TemperatureMapParameters temperature)
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

            Progress = ParametersSave.LoadParametersOrDefault<ProgressMapParameters>(slotToLoad);
            Polution = ParametersSave.LoadParametersOrDefault<PolutionMapParameters>(slotToLoad);
            Temperature = ParametersSave.LoadParametersOrDefault<TemperatureMapParameters>(slotToLoad);
            Heights = ParametersSave.LoadParametersOrDefault<HeightsMapParameters>(slotToLoad);
        }

    }
}

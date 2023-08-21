using System;
using System.Security.Cryptography;
using System.Text;
using WorldGeneration.Core.Noise;

namespace WorldGeneration.Core
{
    [Serializable]
    public readonly struct GeneratorParameters
    {
        public readonly string Seed;
        public readonly int IntSeed;
        public readonly uint WorldWidth;
        public readonly uint WorldHeight;
        public readonly ProgressGenerationParameters Progress;
        public readonly PolutionGenerationParameters Polution;
        public readonly HeightsGenerationParameters Heights;
        public readonly TemperatureGenerationParameters Temperature;

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

        public GeneratorParameters(string seed, uint width, uint height)
        {
            Seed = seed;
            IntSeed = WorldGenerator.ComputeInt32Seed(Seed);

            WorldWidth = width;
            WorldHeight = height;

            Progress = ParametersSave.LoadParameters<ProgressGenerationParameters>(ParametersSave.SaveSlot.Default).GetValueOrDefault();
            Polution = ParametersSave.LoadParameters<PolutionGenerationParameters>(ParametersSave.SaveSlot.Default).GetValueOrDefault();
            Temperature = ParametersSave.LoadParameters<TemperatureGenerationParameters>(ParametersSave.SaveSlot.Default).GetValueOrDefault();
            Heights = ParametersSave.LoadParameters<HeightsGenerationParameters>(ParametersSave.SaveSlot.Default).GetValueOrDefault();
        }

    }
}

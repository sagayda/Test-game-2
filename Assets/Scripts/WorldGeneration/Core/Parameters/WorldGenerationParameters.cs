using System;
using WorldGeneration.Core.Noise;

namespace WorldGeneration.Core
{
    [Serializable]
    public readonly struct HeightsGenerationParameters : IParameters
    {
        public readonly FractalNoiseParameters Noise;
        public readonly float WaterLevel;

        public HeightsGenerationParameters(FractalNoiseParameters noise, float waterLevel)
        {
            Noise = noise;

            WaterLevel = waterLevel;
        }
    }

    [Serializable]
    public readonly struct TemperatureGenerationParameters : IParameters
    {
        public readonly FractalNoiseParameters Noise;

        public readonly float HeightImpactStrength;
        public readonly float HeightImpactSmoothing;
        public readonly float NoiseImpactStrength;
        public readonly float GlobalTemperature;

        public TemperatureGenerationParameters(FractalNoiseParameters noise, float heightImpactStrength, float heightImpactSmoothing, float noiseImpactStrength, float globalTemperature)
        {
            Noise = noise;

            HeightImpactStrength = heightImpactStrength;
            HeightImpactSmoothing = heightImpactSmoothing;
            NoiseImpactStrength = noiseImpactStrength;
            GlobalTemperature = globalTemperature;
        }
    }

    [Serializable]
    public readonly struct PolutionGenerationParameters : IParameters
    {
        public readonly FractalNoiseParameters Noise;

        public readonly float ProgressImpactStrength;
        public readonly float ProgressImpactMultiplyer;
        public readonly float ProgressImpactBottom;
        public readonly float ProgressImpactTop;

        public PolutionGenerationParameters(FractalNoiseParameters noise, float progressImpactStrength, float progressImpactMultiplyer, float progressImpactBottom, float progressImpactTop)
        {
            Noise = noise;

            ProgressImpactStrength = progressImpactStrength;
            ProgressImpactMultiplyer = progressImpactMultiplyer;
            ProgressImpactBottom = progressImpactBottom;
            ProgressImpactTop = progressImpactTop;
        }
    }

    [Serializable]
    public readonly struct ProgressGenerationParameters : IParameters 
    {
        public readonly FractalNoiseParameters Noise;

        public ProgressGenerationParameters(FractalNoiseParameters noise)
        {
            Noise = noise;

        }
    }
}

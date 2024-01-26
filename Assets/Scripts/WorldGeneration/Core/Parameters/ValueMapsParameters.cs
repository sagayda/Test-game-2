using System;
using WorldGeneration.Core.Noise;
using WorldGeneration.Core.Parameters;

namespace WorldGeneration.Core
{
    [Serializable]
    public readonly struct HeightsMapParameters : IValueMapParameters
    {
        public readonly FractalNoiseParameters Noise { get; }
        public readonly float WaterLevel { get; }

        public HeightsMapParameters(FractalNoiseParameters noise, float waterLevel)
        {
            Noise = noise;

            WaterLevel = waterLevel;
        }
    }

    [Serializable]
    public readonly struct TemperatureMapParameters : IValueMapParameters
    {
        public readonly FractalNoiseParameters Noise { get; }

        public readonly float HeightImpactStrength { get; }
        public readonly float HeightImpactSmoothing { get; }
        public readonly float NoiseImpactStrength { get; }
        public readonly float EquatorCoordinate { get; }
        public readonly float GlobalTemperature { get; }

        public TemperatureMapParameters(FractalNoiseParameters noise, float heightImpactStrength, float heightImpactSmoothing, float noiseImpactStrength, float equatorCoordinate, float globalTemperature)
        {
            Noise = noise;

            HeightImpactStrength = heightImpactStrength;
            HeightImpactSmoothing = heightImpactSmoothing;
            NoiseImpactStrength = noiseImpactStrength;
            EquatorCoordinate = equatorCoordinate;
            GlobalTemperature = globalTemperature;
        }
    }

    [Serializable]
    public readonly struct PolutionMapParameters : IValueMapParameters
    {
        public readonly FractalNoiseParameters Noise { get; }

        public readonly float ProgressImpactStrength { get; }
        public readonly float ProgressImpactMultiplyer { get; }
        public readonly float ProgressImpactBottom { get; }
        public readonly float ProgressImpactTop { get; }

        public PolutionMapParameters(FractalNoiseParameters noise, float progressImpactStrength, float progressImpactMultiplyer, float progressImpactBottom, float progressImpactTop)
        {
            Noise = noise;

            ProgressImpactStrength = progressImpactStrength;
            ProgressImpactMultiplyer = progressImpactMultiplyer;
            ProgressImpactBottom = progressImpactBottom;
            ProgressImpactTop = progressImpactTop;
        }
    }

    [Serializable]
    public readonly struct ProgressMapParameters : IValueMapParameters
    {
        public readonly FractalNoiseParameters Noise { get; }

        public ProgressMapParameters(FractalNoiseParameters noise)
        {
            Noise = noise;
        }
    }
}

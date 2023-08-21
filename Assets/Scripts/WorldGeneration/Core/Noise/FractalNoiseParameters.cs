using System;
using UniversalTools;

namespace WorldGeneration.Core.Noise
{
    [Serializable]
    public readonly struct FractalNoiseParameters : IParameters
    {
        public readonly float XSeedStep;
        public readonly float YSeedStep;

        public readonly int Octaves;
        public readonly float Amplitude;
        public readonly float Frequency;
        public readonly float Persistance;
        public readonly float Lacunarity;

        public readonly EasingFunction.Function EasingFunction;

        public FractalNoiseParameters(int octaves, float amplitude, float frequency, float persistance, float lacunarity, EasingFunction.Ease easingMode = UniversalTools.EasingFunction.Ease.Linear)
        {
            XSeedStep = 0;
            YSeedStep = 0;

            Octaves = octaves;
            Amplitude = amplitude;
            Frequency = frequency;
            Persistance = persistance;
            Lacunarity = lacunarity;

            EasingFunction = UniversalTools.EasingFunction.GetEasingFunction(easingMode);
        }

        public FractalNoiseParameters(int octaves, float amplitude, float frequency, float persistance, float lacunarity, float xSeedStep, float ySeedStep, EasingFunction.Ease easingMode = UniversalTools.EasingFunction.Ease.Linear)
        {
            XSeedStep = xSeedStep;
            YSeedStep = ySeedStep;

            Octaves = octaves;
            Amplitude = amplitude;
            Frequency = frequency;
            Persistance = persistance;
            Lacunarity = lacunarity;

            EasingFunction = UniversalTools.EasingFunction.GetEasingFunction(easingMode);
        }

    }
}

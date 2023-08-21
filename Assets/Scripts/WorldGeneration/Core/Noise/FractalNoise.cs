using UnityEngine;

namespace WorldGeneration.Core.Noise
{
    public static class FractalNoise
    {
        public static int Seed
        {
            get { return Noise.Seed; }
            set { Noise.Seed = value; }
        }

        public static float Generate(float x, float y, FractalNoiseParameters parameters)
        {
            float amplitude = parameters.Amplitude;
            float frequency = parameters.Frequency;

            float resultNoise = 0f;

            for (int octave = 0; octave < parameters.Octaves; octave++)
            {
                resultNoise += Noise.Generate(
                    (x + parameters.XSeedStep) * frequency,
                    (y + parameters.YSeedStep) * frequency)
                    * amplitude;

                amplitude *= parameters.Persistance;
                frequency *= parameters.Lacunarity;
            }

            return Mathf.Clamp(parameters.EasingFunction(0, 1, resultNoise), 0, 1);
        }

        public static float Generate(Vector2 position, FractalNoiseParameters parameters)
        {
            return Generate(position.x, position.y, parameters);
        }
    }
}

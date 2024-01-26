using UnityEngine;

namespace WorldGeneration.Core.Noise
{
    public class FractalNoise
    {
        private readonly SimplexNoise _simplexNoise;
        private readonly FractalNoiseParameters _parameters;

        public FractalNoise(FractalNoiseParameters parameters)
        {
            _simplexNoise = new SimplexNoise();
            _parameters = parameters;
        }

        public FractalNoise(FractalNoiseParameters parameters, int seed)
        {
            _simplexNoise = new SimplexNoise(seed);
            _parameters = parameters;
        }

        public int Seed
        {
            get { return _simplexNoise.Seed; }
            set { _simplexNoise.Seed = value; }
        }

        public float Generate(float x, float y)
        {
            float amplitude = _parameters.Amplitude;
            float frequency = _parameters.Frequency;

            float resultNoise = 0f;

            for (int octave = 0; octave < _parameters.Octaves; octave++)
            {
                resultNoise += _simplexNoise.Generate(
                    (x + _parameters.XSeedStep) * frequency,
                    (y + _parameters.YSeedStep) * frequency)
                    * amplitude;

                amplitude *= _parameters.Persistance;
                frequency *= _parameters.Lacunarity;
            }

            return Mathf.Clamp(_parameters.EasingFunction(0, 1, resultNoise), 0, 1);
        }

        public float Generate(Vector2 position)
        {
            float amplitude = _parameters.Amplitude;
            float frequency = _parameters.Frequency;

            float resultNoise = 0f;

            for (int octave = 0; octave < _parameters.Octaves; octave++)
            {
                resultNoise += _simplexNoise.Generate(
                    (position.x + _parameters.XSeedStep) * frequency,
                    (position.y + _parameters.YSeedStep) * frequency)
                    * amplitude;

                amplitude *= _parameters.Persistance;
                frequency *= _parameters.Lacunarity;
            }

            return Mathf.Clamp(_parameters.EasingFunction(0, 1, resultNoise), 0, 1);
        }
    }
}

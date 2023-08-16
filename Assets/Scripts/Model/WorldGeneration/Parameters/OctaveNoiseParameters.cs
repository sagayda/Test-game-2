using System;

namespace Assets.Scripts.Model.WorldGeneration
{
    [Serializable]
    public class OctaveNoiseParameters
    {
        private readonly float _seedStep;
        private readonly float _zoom;
        private readonly float _persistance;
        private readonly float _lacunarity;
        private readonly int _ocataves;

        private readonly IEasingStrategy _easingStrategy;

        public OctaveNoiseParameters(float seedX, float seedY, float seedStep, float zoom, int octaves, float persistance, float lacunarity)
        {
            SeedX = seedX;
            SeedY = seedY;
            _seedStep = seedStep;
            _zoom = zoom;
            _persistance = persistance;
            _lacunarity = lacunarity;
            _ocataves = octaves;

            _easingStrategy = new EaseLinear();
        }

        public OctaveNoiseParameters(float seedX, float seedY, float seedStep, float zoom, int octaves, float persistance, float lacunarity, IEasingStrategy easingStrategy)
        {
            SeedX = seedX;
            SeedY = seedY;
            _seedStep = seedStep;
            _zoom = zoom;
            _persistance = persistance;
            _lacunarity = lacunarity;
            _ocataves = octaves;

            _easingStrategy = easingStrategy;
        }

        public OctaveNoiseParameters(float seedStep, float zoom, int octaves, float persistance, float lacunarity)
        {
            _seedStep = seedStep;
            _zoom = zoom;
            _persistance = persistance;
            _lacunarity = lacunarity;
            _ocataves = octaves;

            _easingStrategy = new EaseLinear();
        }

        public OctaveNoiseParameters(float seedStep, float zoom, int octaves, float persistance, float lacunarity, IEasingStrategy easingStrategy)
        {
            _seedStep = seedStep;
            _zoom = zoom;
            _persistance = persistance;
            _lacunarity = lacunarity;
            _ocataves = octaves;

            _easingStrategy = easingStrategy;
        }

        public float SeedX {get; private set;}
        public float SeedY { get; private set;}
        public float SeedStep => _seedStep;
        public float Zoom => _zoom;
        public int Octaves => _ocataves;
        public float Persistance => _persistance;
        public float Lacunarity => _lacunarity;
        public IEasingStrategy EasingStrategy => _easingStrategy;

        public void SetSeed(float seedX, float seedY)
        {
            SeedX = seedX;
            SeedY = seedY;
        }
    }
}

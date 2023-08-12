using System.Security.Cryptography;
using System.Text;
using System;
using UnityEngine;

namespace Assets.Scripts.Model.WorldGeneration
{
    public class OctaveNoiseParameters
    {
        private readonly float _seedX;
        private readonly float _seedY;
        private readonly float _seedStep;
        private readonly float _zoom;
        private readonly float _persistance;
        private readonly float _lacunarity;
        private readonly int _ocataves;

        private readonly IEasingStrategy _easingStrategy;

        public OctaveNoiseParameters(string seed, float seedStep, float zoom, int octaves, float persistance, float lacunarity)
        {
            _seedStep = seedStep;
            _zoom = zoom;
            _persistance = persistance;
            _lacunarity = lacunarity;
            _ocataves = octaves;

            _easingStrategy = new EasyLinear();

            #region seedConvertation
            using SHA256 sha256 = SHA256.Create();

            byte[] hashX = sha256.ComputeHash(Encoding.UTF8.GetBytes("x" + seed));
            byte[] hashY = sha256.ComputeHash(Encoding.UTF8.GetBytes("y" + seed));

            _seedX = BitConverter.ToInt16(hashX, 0);
            _seedY = BitConverter.ToInt16(hashY, 0);
            #endregion
        }

        public float SeedX => _seedX;
        public float SeedY => _seedY;
        public float SeedStep => _seedStep;
        public float Zoom => _zoom;
        public int Octaves => _ocataves;
        public float Persistance => _persistance;
        public float Lacunarity => _lacunarity;
        public IEasingStrategy EasingStrategy => _easingStrategy;
    }
}

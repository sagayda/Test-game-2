using System;
using System.Security.Cryptography;
using System.Text;

namespace WorldGeneration.Core
{
    [Serializable]
    public class GeneratorParameters
    {
        private readonly string _seed;
        private readonly float _seedX;
        private readonly float _seedY;

        private readonly uint _worldWidth;
        private readonly uint _worldHeight;

        private readonly ProgressNoiseParameters _progress;
        private readonly PolutionNoiseParameters _polution;
        private readonly HeightsNoiseParameters _heights;
        private readonly TemperatureNoiseParameters _temperature;

        public GeneratorParameters(string seed, uint width, uint height, ProgressNoiseParameters progress, PolutionNoiseParameters polution, HeightsNoiseParameters heights, TemperatureNoiseParameters temperature)
        {
            _seed = seed;
            ComputeSeed(_seed, out _seedX, out _seedY);

            _worldWidth = width;
            _worldHeight = height;
            _progress = progress;
            _polution = polution;
            _heights = heights;
            _temperature = temperature;

            SetSeedToParameters();
        }

        public GeneratorParameters(string seed, uint width, uint height)
        {
            _seed = seed;
            ComputeSeed(_seed, out _seedX, out _seedY);

            _worldWidth = width;
            _worldHeight = height;

            NoiseParametersSave parametersSave = new();

            _progress = parametersSave.LoadNoiseParameters<ProgressNoiseParameters>();
            _polution = parametersSave.LoadNoiseParameters<PolutionNoiseParameters>();
            _temperature = parametersSave.LoadNoiseParameters<TemperatureNoiseParameters>();
            _heights = parametersSave.LoadNoiseParameters<HeightsNoiseParameters>();

            SetSeedToParameters();
        }

        public string Seed => _seed;
        public float SeedX => _seedX;
        public float SeedY => _seedY;
        public uint WorldWidth => _worldWidth;
        public uint WorldHeight => _worldHeight;

        public ProgressNoiseParameters Progress
        {
            get
            {
                if (_progress == null)
                    throw new InvalidOperationException("World generator parameter 'Progress' is invalid!");

                return _progress;
            }
        }
        public PolutionNoiseParameters Polution
        {
            get
            {
                if (_polution == null)
                    throw new InvalidOperationException("World generator parameter 'Polution' is invalid!");

                return _polution;
            }
        }
        public HeightsNoiseParameters Height
        {
            get
            {
                if (_heights == null)
                    throw new InvalidOperationException("World generator parameter 'Height' is invalid!");

                return _heights;
            }
        }
        public TemperatureNoiseParameters Temperature
        {
            get
            {
                if (_temperature == null)
                    throw new InvalidOperationException("World generator parameter 'Temperature' is invalid!");

                return _temperature;
            }
        }

        private void SetSeedToParameters()
        {
            _progress.Noise.SetSeed(_seedX, _seedY);
            _polution.Noise.SetSeed(_seedX, _seedY);
            _heights.Noise.SetSeed(_seedX, _seedY);
            _temperature.Noise.SetSeed(_seedX, _seedY);
        }

        public static void ComputeSeed(string seed, out float seedX, out float seedY)
        {
            using SHA256 sha256 = SHA256.Create();

            byte[] hashX = sha256.ComputeHash(Encoding.UTF8.GetBytes("x" + seed));
            byte[] hashY = sha256.ComputeHash(Encoding.UTF8.GetBytes("y" + seed));

            seedX = BitConverter.ToInt16(hashX, 0);
            seedY = BitConverter.ToInt16(hashY, 0);
        }

    }
}

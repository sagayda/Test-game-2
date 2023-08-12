using System;

namespace Assets.Scripts.Model.WorldGeneration
{
    [Serializable]
    public class GeneratorParameters
    {
        private readonly long _seed;
        private readonly uint _worldWidth;
        private readonly uint _worldHeight;

        private readonly ProgressNoiseParameters _progress;
        private readonly PolutionNoiseParameters _polution;
        private readonly HeightsNoiseParameters _heights;
        private readonly TemperatureNoiseParameters _temperature;
        private readonly RiversNoiseParameters _rivers;

        public GeneratorParameters(long seed, uint width, uint height, ProgressNoiseParameters progress, PolutionNoiseParameters polution, HeightsNoiseParameters heights, TemperatureNoiseParameters temperature, RiversNoiseParameters rivers)
        {
            _seed = seed;
            _worldWidth = width;
            _worldHeight = height;
            _progress = progress;
            _polution = polution;
            _heights = heights;
            _temperature = temperature;
            _rivers = rivers;
        }

        public long Seed => _seed;
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
        public RiversNoiseParameters Rivers
        {
            get
            {
                if (_rivers == null)
                    throw new InvalidOperationException("World generator parameter 'Rivers' is invalid!");

                return _rivers;
            }
        }

    }
}

using System;
using UnityEngine;

namespace Assets.Scripts.Model.WorldGeneration
{
    [CreateAssetMenu(fileName = "GeneratorParameters", menuName = "World generator/Create generator parameters")]
    public class GeneratorParameters : ScriptableObject
    {
        [SerializeField] private string _charSeed;
        [SerializeField] private uint _worldWidth;
        [SerializeField] private uint _worldHeight;

        [SerializeField] private ProgressNoiseParameters _progress;
        [SerializeField] private PolutionNoiseParameters _polution;
        [SerializeField] private HeightNoiseParameters _height;
        [SerializeField] private TemperatureNoiseParameters _temperature;
        [SerializeField] private RiversNoiseParameters _rivers;

        private void OnValidate()
        {
            string seed = string.Empty;

            foreach (var ch in _charSeed)
            {
                seed += (int)ch;
            }

            Seed = 0;

            if (int.TryParse(seed, out int s))
                Seed = s;
        }

        public int Seed { get; private set; }
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
        public HeightNoiseParameters Height
        {
            get
            {
                if (_height == null)
                    throw new InvalidOperationException("World generator parameter 'Height' is invalid!");

                return _height;
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

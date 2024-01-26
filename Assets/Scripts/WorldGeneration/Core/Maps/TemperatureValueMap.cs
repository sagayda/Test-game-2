using System;
using UnityEngine;
using WorldGeneration.Core.Noise;

namespace WorldGeneration.Core
{
    public class TemperatureValueMap : IValueMap
    {
        private TemperatureMapParameters _parameters;
        private FractalNoise _noiseProvider;

        public TemperatureValueMap(TemperatureMapParameters parameters)
        {
            _parameters = parameters;
            _noiseProvider = new(_parameters.Noise);
        }

        public int Seed
        {
            get => _noiseProvider.Seed;
            set => _noiseProvider.Seed = value;
        }
        public int Priority => 50;
        public MapValueType ValueType => MapValueType.Temperature;

        public void SetParameters(TemperatureMapParameters parameters)
        {
            _parameters = parameters;
            _noiseProvider = new(_parameters.Noise);
        }

        public ValueMapPoint ComputeValue(ValueMapPoint mapPoint, Vector2 position)
        {
            float temperatureNoise = _noiseProvider.Generate(position);

            if (float.IsNaN(temperatureNoise))
                throw new ArgumentException("Trying to generate a temperature value for a point without a height value", nameof(mapPoint));

            //adaptation to latitude
            temperatureNoise -= 0.5f;

            //distance scaled to [0,1]
            float distanceFromEquator = Mathf.Abs((_parameters.EquatorCoordinate) - position.y) / _parameters.EquatorCoordinate;

            float temperature = distanceFromEquator;
            temperature += _parameters.GlobalTemperature;
            temperature += temperatureNoise * _parameters.NoiseImpactStrength;

            //reverse temperatures
            temperature = 1 - temperature;

            //height impact
            //if (true)
            //{
            //    float temperatureChange;

            //    //temperature change depending on distance from equator
            //    temperatureChange = height - Parameters.Heights.WaterLevel;
            //    //smoothing
            //    temperatureChange = Mathf.Pow(temperatureChange, Parameters.Temperature.HeightImpactSmoothing);
            //    temperatureChange /= 1 - Parameters.Heights.WaterLevel;
            //    //temperature 0..1 => 1..2 for better smoothing
            //    temperature += 1;
            //    //calculating temperature change strange
            //    temperatureChange = Mathf.Pow(Parameters.Temperature.HeightImpactStrength, temperatureChange);

            //    temperature -= temperatureChange;
            //}

            return mapPoint.SetValue(MapValueType.Temperature, temperature);
        }
    }
}

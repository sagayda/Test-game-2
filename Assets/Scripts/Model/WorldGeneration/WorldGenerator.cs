using System;
using Assets.Scripts.InGameScripts.World;
using Assets.Scripts.InGameScripts.World.Absctract;
using UnityEngine;

namespace Assets.Scripts.Model.WorldGeneration
{
    public class WorldGenerator
    {
        private GeneratorParameters _parameters;

        public WorldGenerator(GeneratorParameters parameters)
        {
            Parameters = parameters;
        }

        private GeneratorParameters Parameters
        {
            get
            {
                if (_parameters == null)
                    throw new InvalidOperationException("World generator parameters is not set!");

                return _parameters;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(Parameters), "World generator parameters can not be null!");

                _parameters = value;
            }
        }

        public uint WorldWidth => Parameters.WorldWidth;
        public uint WorldHeight => Parameters.WorldHeight;
        public float WaterLevel => Parameters.Height.WaterLevel;

        public float GetProgressValue(int x, int y)
        {
            return GetBaseNoiseValue(x, y, Parameters.Progress.Noise);
        }

        public float GetPolutionValue(int x, int y)
        {
            float noiseProgress = GetProgressValue(x, y);

            float noisePolution = GetBaseNoiseValue(x, y, Parameters.Polution.Noise);

            float polutionMultiplyer = Mathf.Pow(noiseProgress * Parameters.Polution.ProgressImpactMultiplyer, Parameters.Polution.ProgressImpactStrength);
            polutionMultiplyer = Mathf.Clamp(polutionMultiplyer, Parameters.Polution.ProgressImpactBottom, Parameters.Polution.ProgressImpactTop);

            noisePolution *= polutionMultiplyer;

            return noisePolution;
        }

        public float GetHeightValue(int x, int y)
        {
            float noise = GetBaseNoiseValue(x, y, Parameters.Height.Noise);

            return noise;
        }

        public float GetTemperatureValue(int x, int y)
        {
            float noiseHight = GetHeightValue(x, y);

            float rawNoiseTemperature = GetBaseNoiseValue(x, y, Parameters.Temperature.Noise);

            //adaptation to latitude
            rawNoiseTemperature -= 0.5f;

            float distanceFromEquator = Mathf.Abs((Parameters.WorldHeight / 2f) - y);
            distanceFromEquator /= Parameters.WorldHeight / 2f;

            float temperatureNoise = distanceFromEquator;
            temperatureNoise += Parameters.Temperature.GlobalTemperature;

            temperatureNoise += rawNoiseTemperature * Parameters.Temperature.NoiseImpact;

            temperatureNoise = 1 - temperatureNoise;

            if (noiseHight > Parameters.Height.WaterLevel)
            {
                float temperatureChange;

                //temperature change depending on distance from equator
                temperatureChange = noiseHight - Parameters.Height.WaterLevel;
                //smoothing
                temperatureChange = Mathf.Pow(temperatureChange, Parameters.Temperature.HeightImpactSmoothing);
                temperatureChange /= 1 - Parameters.Height.WaterLevel;
                //temperature 0..1 => 1..2 for better smoothing
                temperatureNoise += 1;
                //calculating temperature change strange
                temperatureChange = Mathf.Pow(Parameters.Temperature.HeightImpactStrength, temperatureChange);

                temperatureNoise -= temperatureChange;
            }

            return temperatureNoise;
        }

        private float GetBaseNoiseValue(float x, float y, OctaveNoiseParameters parameters)
        {
            return OctavePerlinNoise.Noise(x, y, parameters);
        }

        public Location[,] CreateWorld()
        {
            Location[,] map = new Location[WorldWidth, WorldHeight];

            for (int x = 0; x < WorldWidth; x++)
            {
                for (int y = 0; y < WorldHeight; y++)
                {
                    if (x == 0 && y == 0)
                    {
                        map[x, y] = new Location_Wasteland(x, y);
                        continue;
                    }

                    map[x, y] = GetLocation(x, y);
                }
            }

            return map;
        }

        #region Map generation methods
        private Location GetLocation(int x, int y)
        {
            float height = GetHeightValue(x, y);


            if (height < WaterLevel)
            {
                return GetWaterLocation(x, y, height);
            }
            else
            {
                    return GetLandLocation(x, y, height);
            }
        }

        private Location GetWaterLocation(int x, int y, float height)
        {
            float temperature = GetTemperatureValue(x, y);

            if (temperature < 0.15f)
            {
                return new Location_ArcticDesert(x, y);
            }

            if (height > WaterLevel / 2f)
            {
                return new Location_Ocean(x, y);
            }
            else
            {
                return new Location_DeepOcean(x, y);
            }
        }

        private Location GetLandLocation(int x, int y, float height)
        {
            float temperature = GetTemperatureValue(x, y);

            if (temperature < 0.15f)
            {
                return new Location_ArcticDesert(x, y);
            }

            if (height < WaterLevel + 0.02f)
            {
                return new Location_SandBeach(x, y);
            }

            float heightStep = (1f - WaterLevel) / 4f;

            if (height < WaterLevel + heightStep * 2)
            {
                return new Location_Plain(x, y);
            }
            else if (height < WaterLevel + heightStep * 3)
            {
                return new Location_Foothills(x, y);
            }
            else
            {
                return new Location_Mountains(x, y);
            }
        }
        #endregion

    }
}

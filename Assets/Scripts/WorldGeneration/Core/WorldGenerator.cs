using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using WorldGeneration.Core.Locations;
using WorldGeneration.Core.Noise;

namespace WorldGeneration.Core
{
    public static class WorldGenerator
    {
        private static FractalNoise _heightsNoise;
        private static FractalNoise _temperatureNoise;
        private static FractalNoise _polutionNoise;
        private static FractalNoise _progressNoise;
        private static GeneratorParameters _parameters;

        static WorldGenerator()
        {
            _parameters = ParametersSave.Default;
            _heightsNoise = new(_parameters.Heights.Noise, _parameters.IntSeed);
            _temperatureNoise = new(_parameters.Temperature.Noise, _parameters.IntSeed);
            _polutionNoise = new(_parameters.Polution.Noise, _parameters.IntSeed);
            _progressNoise = new(_parameters.Progress.Noise, _parameters.IntSeed);
        }

        public static GeneratorParameters Parameters
        {
            get
            {
                return _parameters;
            }
            set
            {
                _parameters = value;
                UpdateParameters();
            }
        }

        public static uint WorldWidth => Parameters.WorldWidth;
        public static uint WorldHeight => Parameters.WorldHeight;
        public static float WaterLevel => Parameters.Heights.WaterLevel;

        public static float GetProgressValue(int x, int y)
        {
            return _progressNoise.Generate(x, y);
        }

        public static float GetPolutionValue(int x, int y)
        {
            float noiseProgress = GetProgressValue(x, y);

            float noisePolution = _polutionNoise.Generate(x, y);

            float polutionMultiplyer = Mathf.Pow(noiseProgress * Parameters.Polution.ProgressImpactMultiplyer, Parameters.Polution.ProgressImpactStrength);
            polutionMultiplyer = Mathf.Clamp(polutionMultiplyer, Parameters.Polution.ProgressImpactBottom, Parameters.Polution.ProgressImpactTop);

            noisePolution *= polutionMultiplyer;

            return noisePolution;
        }

        public static float GetHeightValue(float x, float y)
        {
            float noise = _heightsNoise.Generate(x, y);

            return noise;
        }

        public static float GetTemperatureValue(int x, int y)
        {
            float noiseHight = GetHeightValue(x, y);

            float rawNoiseTemperature = _temperatureNoise.Generate(x, y);

            //adaptation to latitude
            rawNoiseTemperature -= 0.5f;

            float distanceFromEquator = Mathf.Abs((Parameters.WorldHeight / 2f) - y);
            distanceFromEquator /= Parameters.WorldHeight / 2f;

            float temperatureNoise = distanceFromEquator;
            temperatureNoise += Parameters.Temperature.GlobalTemperature;

            temperatureNoise += rawNoiseTemperature * Parameters.Temperature.NoiseImpactStrength;

            temperatureNoise = 1 - temperatureNoise;

            if (noiseHight > Parameters.Heights.WaterLevel)
            {
                float temperatureChange;

                //temperature change depending on distance from equator
                temperatureChange = noiseHight - Parameters.Heights.WaterLevel;
                //smoothing
                temperatureChange = Mathf.Pow(temperatureChange, Parameters.Temperature.HeightImpactSmoothing);
                temperatureChange /= 1 - Parameters.Heights.WaterLevel;
                //temperature 0..1 => 1..2 for better smoothing
                temperatureNoise += 1;
                //calculating temperature change strange
                temperatureChange = Mathf.Pow(Parameters.Temperature.HeightImpactStrength, temperatureChange);

                temperatureNoise -= temperatureChange;
            }

            return temperatureNoise;
        }

        public static GameWorld GetGameWorld()
        {
            return null;
        }

        public static int ComputeInt32Seed(string seed)
        {
            using SHA256 sha256 = SHA256.Create();

            byte[] hashX = sha256.ComputeHash(Encoding.UTF8.GetBytes("x" + seed));

            return BitConverter.ToInt32(hashX, 0);
        }

        private static void UpdateParameters()
        {
            _heightsNoise = new(_parameters.Heights.Noise, _parameters.IntSeed);
            _temperatureNoise = new(_parameters.Temperature.Noise, _parameters.IntSeed);
            _polutionNoise = new(_parameters.Polution.Noise, _parameters.IntSeed);
            _progressNoise = new(_parameters.Progress.Noise, _parameters.IntSeed);
        }

        //public Location[,] CreateWorld()
        //{
        //    Location[,] map = new Location[WorldWidth, WorldHeight];

        //    for (int x = 0; x < WorldWidth; x++)
        //    {
        //        for (int y = 0; y < WorldHeight; y++)
        //        {
        //            if (x == 0 && y == 0)
        //            {
        //                map[x, y] = new Location_Wasteland(x, y);
        //                continue;
        //            }

        //            map[x, y] = GetLocation(x, y);
        //        }
        //    }

        //    return map;
        //}

        //#region Map generation methods
        //private Location GetLocation(int x, int y)
        //{
        //    float height = GetHeightValue(x, y);


        //    if (height < WaterLevel)
        //    {
        //        return GetWaterLocation(x, y, height);
        //    }
        //    else
        //    {
        //        return GetLandLocation(x, y, height);
        //    }
        //}

        //private Location GetWaterLocation(int x, int y, float height)
        //{
        //    float temperature = GetTemperatureValue(x, y);

        //    if (temperature < 0.15f)
        //    {
        //        return new Location_ArcticDesert(x, y);
        //    }

        //    if (height > WaterLevel / 2f)
        //    {
        //        return new Location_Ocean(x, y);
        //    }
        //    else
        //    {
        //        return new Location_DeepOcean(x, y);
        //    }
        //}

        //private Location GetLandLocation(int x, int y, float height)
        //{
        //    float temperature = GetTemperatureValue(x, y);

        //    if (temperature < 0.15f)
        //    {
        //        return new Location_ArcticDesert(x, y);
        //    }

        //    if (height < WaterLevel + 0.02f)
        //    {
        //        return new Location_SandBeach(x, y);
        //    }

        //    float heightStep = (1f - WaterLevel) / 4f;

        //    if (height < WaterLevel + heightStep * 2)
        //    {
        //        return new Location_Plain(x, y);
        //    }
        //    else if (height < WaterLevel + heightStep * 3)
        //    {
        //        return new Location_Foothills(x, y);
        //    }
        //    else
        //    {
        //        return new Location_Mountains(x, y);
        //    }
        //}
        //#endregion

    }
}

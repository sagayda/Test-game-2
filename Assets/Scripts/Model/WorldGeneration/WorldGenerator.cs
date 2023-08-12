using System;
using System.Security.Cryptography;
using System.Text;
using Assets.Scripts.InGameScripts.World;
using Assets.Scripts.InGameScripts.World.Absctract;
using UnityEngine;

namespace Assets.Scripts.Model.WorldGeneration
{
    public static class WorldGenerator
    {
        private static GeneratorParameters _parameters;

        private static GeneratorParameters Parameters
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

        public static uint WorldWidth => Parameters.WorldWidth;
        public static uint WorldHeight => Parameters.WorldHeight;
        public static float WaterLevel => Parameters.Height.WaterLevel;

        public static void SetParameters(GeneratorParameters parameters)
        {
            Parameters = parameters;
        }

        public static float GetProgressValue(int x, int y)
        {
            return GetBaseNoiseValue(x, y, Parameters.Progress.BaseNoise);
        }

        public static float GetPolutionValue(int x, int y)
        {
            float noiseProgress = GetProgressValue(x, y);

            float noisePolution = GetBaseNoiseValue(x, y, Parameters.Polution.BaseNoise);

            float polutionMultiplyer = Mathf.Pow(noiseProgress * Parameters.Polution.ProgressImpactMultiplyer, Parameters.Polution.ProgressImpactStrength);
            polutionMultiplyer = Mathf.Clamp(polutionMultiplyer, Parameters.Polution.ProgressImpactBottom, Parameters.Polution.ProgressImpactTop);

            noisePolution *= polutionMultiplyer;

            return noisePolution;
        }

        public static float GetHeightValue(int x, int y)
        {
            //main noise
            float noise = GetBaseNoiseValue(x, y, Parameters.Height.BaseNoise);

            //additional noise
            float additionalNoise = GetBaseNoiseValue(x, y, Parameters.Height.AdditionalNoise);

            noise = (noise * Parameters.Height.LayersMixStrength + additionalNoise) / (Parameters.Height.LayersMixStrength + 1f);

            //rivers noise
            float riversNoise = GetRiversValue(x, y);

            //pressing out rivers
            if (noise > Parameters.Height.WaterLevel)
                noise -= (riversNoise * ((noise - Parameters.Height.WaterLevel) / Parameters.Height.RiversSideLevel));

            return noise;
        }

        public static float GetTemperatureValue(int x, int y)
        {
            float noiseHight = GetHeightValue(x, y);

            float rawNoiseTemperature = GetBaseNoiseValue(x, y, Parameters.Temperature.BaseNoise);

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

        //public static float GetRiversValue(int x, int y)
        //{
        //    float rawNoise = Mathf.PerlinNoise((x + Parameters.Seed + Parameters.Rivers.BaseNoise.SeedStep) / Parameters.Rivers.BaseNoise.Zoom, (y + Parameters.Seed + Parameters.Rivers.BaseNoise.SeedStep) / Parameters.Rivers.BaseNoise.Zoom);
        //    rawNoise += GetBaseNoiseValue(x, y, Parameters.Rivers.AdditionalNoise);
        //    rawNoise /= 2f;

        //    rawNoise -= Parameters.Rivers.Level;

        //    float noiseMax;

        //    if (Parameters.Rivers.Level < 0.5f)
        //        noiseMax = Parameters.Rivers.Level;
        //    else
        //        noiseMax = 1f - Parameters.Rivers.Level;

        //    float noise = rawNoise;
        //    noise = Mathf.Abs(noise);
        //    noise = Mathf.Clamp(noise, 0, noiseMax);

        //    noise *= 1f / noiseMax;

        //    noise = 1 - noise;
        //    noise = Mathf.Pow(noise, Parameters.Rivers.Sharpness);
        //    noise *= Parameters.Rivers.BaseNoise.Density;

        //    return noise;
        //}

        public static float GetRiversValue(int x, int y)
        {
            float rawNoise = Mathf.PerlinNoise((x + Parameters.Seed + Parameters.Rivers.BaseNoise.SeedStep) / Parameters.Rivers.BaseNoise.Zoom, (y + Parameters.Seed + Parameters.Rivers.BaseNoise.SeedStep) / Parameters.Rivers.BaseNoise.Zoom);
            rawNoise -= GetBaseNoiseValue(x, y, Parameters.Rivers.AdditionalNoise);

            //rawNoise -= Mathf.PerlinNoise((x + Parameters.Seed + Parameters.Rivers.AdditionalNoise.SeedStep) / Parameters.Rivers.AdditionalNoise.Zoom, (y + Parameters.Seed + Parameters.Rivers.AdditionalNoise.SeedStep) / Parameters.Rivers.AdditionalNoise.Zoom);
            rawNoise = Mathf.Clamp(rawNoise, 0, 1);

            rawNoise -= Parameters.Rivers.Level;

            float noiseMax;

            if (Parameters.Rivers.Level < 0.5f)
                noiseMax = Parameters.Rivers.Level;
            else
                noiseMax = 1f - Parameters.Rivers.Level;

            float noise = rawNoise;
            noise = Mathf.Abs(noise);
            noise = Mathf.Clamp(noise, 0, noiseMax);

            noise *= 1f / noiseMax;

            noise = 1 - noise;
            noise = Mathf.Pow(noise, Parameters.Rivers.Sharpness);
            noise *= Parameters.Rivers.BaseNoise.Density;

            return noise;
        }

        private static float GetBaseNoiseValue(int x, int y, NoiseParameters parameters)
        {
            float noise = Mathf.PerlinNoise((x + Parameters.Seed + parameters.SeedStep) / parameters.Zoom, (y + Parameters.Seed + parameters.SeedStep) / parameters.Zoom);
            noise *= parameters.Density;
            return noise;
        }

        private static void ComputeSeed(string seed, out float seedX, out float seedY)
        {
            using SHA256 sha256 = SHA256.Create();

            byte[] hashX = sha256.ComputeHash(Encoding.UTF8.GetBytes("x" + seed));
            byte[] hashY = sha256.ComputeHash(Encoding.UTF8.GetBytes("y" + seed));

            seedX = BitConverter.ToInt16(hashX, 0);
            seedY = BitConverter.ToInt16(hashY, 0);
        }

        public static Location[,] CreateWorld()
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
        private static Location GetLocation(int x, int y)
        {
            float height = GetHeightValue(x, y);
            float river = GetRiversValue(x, y);


            if (height < WaterLevel)
            {
                return GetWaterLocation(x, y, height);
            }
            else
            {

                if (river > 0.8f)
                {
                    return GetWaterLocation(x, y, height, true);
                }
                else
                {
                    return GetLandLocation(x, y, height);
                }
            }
        }

        private static Location GetWaterLocation(int x, int y, float height, bool isRiver = false)
        {


            float temperature = GetTemperatureValue(x, y);

            if (temperature < 0.15f)
            {
                return new Location_ArcticDesert(x, y);
            }

            if (isRiver)
            {
                return new Location_River(x, y);
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

        private static Location GetLandLocation(int x, int y, float height)
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

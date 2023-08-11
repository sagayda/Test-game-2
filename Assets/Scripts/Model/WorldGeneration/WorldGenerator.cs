using System;
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
                if(value == null) 
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
            return GetBaseNoiseValue(x, y, Parameters.Progress);
        }

        public static float GetPolutionValue(int x, int y)
        {
            float noiseProgress = GetProgressValue(x, y);

            float noisePolution = GetBaseNoiseValue(x,y, Parameters.Polution);

            float polutionMultiplyer = Mathf.Pow(noiseProgress * Parameters.Polution.ProgressImpactMultiplyer, Parameters.Polution.ProgressImpactStrength);
            polutionMultiplyer = Mathf.Clamp(polutionMultiplyer, Parameters.Polution.ProgressImpactBottom, Parameters.Polution.ProgressImpactTop);
            
            noisePolution *= polutionMultiplyer;

            return noisePolution;
        }

        public static float GetHeightValue(int x, int y)
        {
            //main noise
            float noise = GetBaseNoiseValue(x, y, Parameters.Height);

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

            float rawNoiseTemperature = GetBaseNoiseValue(x,y, Parameters.Temperature);

            //adaptation to latitude
            rawNoiseTemperature -= 0.5f;

            float distanceFromEquator = Mathf.Abs((Parameters.WorldHeight / 2f) - y);
            distanceFromEquator /= Parameters.WorldHeight / 2f;

            float temperatureNoise = distanceFromEquator;
            temperatureNoise += Parameters.Temperature.GlobalTemperature;

            temperatureNoise += rawNoiseTemperature * Parameters.Temperature.NoiseOnTemperatureImpact;

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

        public static float GetRiversValue(int x, int y)
        {
            float rawNoise = Mathf.PerlinNoise((x + Parameters.Seed + Parameters.Rivers.SeedStep) / Parameters.Rivers.Zoom, (y + Parameters.Seed + Parameters.Rivers.SeedStep) / Parameters.Rivers.Zoom);

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
            noise *= Parameters.Rivers.Density;

            return noise;
        }

        private static float GetBaseNoiseValue(int x, int y, NoiseParameters parameters)
        {
            float noise = Mathf.PerlinNoise((x + Parameters.Seed + parameters.SeedStep) / parameters.Zoom, (y + Parameters.Seed + parameters.SeedStep) / parameters.Zoom);
            noise *= parameters.Density;
            return noise;
        }
    }
}

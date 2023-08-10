using Assets.Scripts.InGameScripts.World;
using Assets.Scripts.InGameScripts.World.Absctract;
using System;
using System.Drawing;
using UnityEngine;

namespace Assets.Scripts
{
    public class WorldGenerator : MonoBehaviour
    {
        public string TextSeed = String.Empty;

        public int WorldSeed { get => worldSeed; }
        public int WorldWidth { get => worldWidth; }
        public int WorldHeight { get => worldHeight;}
        public float WaterLevel { get => waterLevel; }

        [Header("World settings")]
        [SerializeField] private int worldSeed;
        [SerializeField] private int worldWidth;
        [SerializeField] private int worldHeight;

        [Header("Progress map settings")]
        [SerializeField] float progressZoom = 30f;
        [SerializeField] int progressSeedStep = 1000;
        [SerializeField][Range(0,10)] float progressDensity = 1.0f;

        [Header("Polution map settings")]
        [SerializeField] float polutionZoom = 30f;
        [SerializeField] int polutionSeedStep = 2000;
        [SerializeField][Range(0, 10)] float polutionDensity = 1.0f;

        [SerializeField] float PoPImpactStrength = 1f;
        [SerializeField] float PoPImpactMultiplyer = 1f;
        [SerializeField] float PoPImpactButtom = 0.1f;
        [SerializeField][Range(0, 10)] float PoPImpactTop = 2f;

        [Header("Height map settings")]
        [SerializeField] float heightZoom = 30f;
        [SerializeField] int heightSeedStep = 3000;
        [SerializeField][Range(0, 10)] float heightDensity = 1.0f;
        [Space]
        [SerializeField] float additionalHeightZoom = 30f;
        [SerializeField] int additionalHeightSeedStep = 4000;
        [SerializeField][Range(0, 10)] float additionalHeightDensity = 1.0f;
        [SerializeField][Range(0, 100)] float layersMixStrength = 0.5f;
        [Space]
        [SerializeField][Range(0, 1)] float waterLevel = 0.35f;
        [SerializeField] float riversSideLevel = 0.9f;

        [Header("Rivers map settings")]
        [SerializeField] float riversZoom = 30f;
        [SerializeField] int riversSeedStep = 4000;
        [SerializeField][Range(0, 10)] float riversDensity = 1.0f;
        [SerializeField][Range(0,1)] float riversLevel = 0.5f;
        [SerializeField][Range(0, 20)] float riversSharpness = 1f;

        [Header("Temperature map settings")]
        [SerializeField] float temperatureZoom = 30f;
        [SerializeField] int temperatureSeedStep = 3000;
        [SerializeField][Range(0, 10)] float temperatureDensity = 1.0f;

        [SerializeField][Range(0.01f,200)] float HoTImpactStrength = 30f;
        [SerializeField][Range(0.01f,10)] float HoTImpactSmoothing = 3.2f;
        [SerializeField][Range(0, 5)] float NoiseOnTemperatureImpact = 0.5f;
        [SerializeField][Range(-1, 1)] float Temperature = 0f;

        [Space]
        [Header("Testing map settings")]
        [SerializeField] float testingZoomX = 30f;
        [SerializeField] float testingZoomY = 30f;
        [SerializeField] int testingSeedStep = 3000;
        [SerializeField][Range(0, 10)] float testingDensity = 1.0f;
        [SerializeField] float testingSmoothing = 1f;
        [SerializeField][Range(0,1)] float testingMainLevel = 0.5f;

        [Space]
        [Header("Testing map settings")]
        [SerializeField] float testing1Zoom = 30f;
        [SerializeField] int testing1SeedStep = 3000;
        [SerializeField][Range(0, 10)] float testing1Density = 1.0f;

        private void Awake()
        {
            int seed = 0;

            foreach (var item in TextSeed)
            {
                seed += item;
            }

            worldSeed = seed;
        }

        public float GetProgressValue(int x, int y)
        {
            float noise = Mathf.PerlinNoise((x + worldSeed + progressSeedStep) / progressZoom, (y + worldSeed + progressSeedStep) / progressZoom);
            noise *= progressDensity;
            return noise;
        }

        public float GetPolutionValue(int x, int y)
        {
            float noiseProgress = GetProgressValue(x, y);

            float noisePolution = Mathf.PerlinNoise((x + worldSeed + polutionSeedStep) / polutionZoom, (y + worldSeed + polutionSeedStep) / polutionZoom);
            noisePolution *= polutionDensity;

            float polutionMultiplyer = Mathf.Pow(noiseProgress * PoPImpactMultiplyer, PoPImpactStrength);
            polutionMultiplyer = Mathf.Clamp(polutionMultiplyer, PoPImpactButtom, PoPImpactTop);
            noisePolution *= polutionMultiplyer;

            return noisePolution;
        }

        public float GetHeightValue(int x, int y)
        {
            //main noise
            float noise = Mathf.PerlinNoise((x + worldSeed + heightSeedStep) / heightZoom, (y + worldSeed + heightSeedStep) / heightZoom);
            noise *= heightDensity;

            //additional noise
            float additionalNoise = Mathf.PerlinNoise((x + worldSeed + additionalHeightSeedStep) / additionalHeightZoom, (y + worldSeed + additionalHeightSeedStep) / additionalHeightZoom);
            additionalNoise *= additionalHeightDensity;
            noise = (noise * layersMixStrength + additionalNoise) / (layersMixStrength + 1f);

            //rivers noise
            float riversNoise = GetRiversValue(x, y);

            //float riverSideLevel = 0.9f;
            
            if(noise > waterLevel)
                noise = noise - (riversNoise * ((noise - waterLevel) / riversSideLevel));

            return noise;
        }

        public float GetTemperatureValue(int x, int y)
        {
            float noiseHight = GetHeightValue(x, y);

            float rawNoiseTemperature = Mathf.PerlinNoise((x + worldSeed + temperatureSeedStep) / temperatureZoom, (y + worldSeed + temperatureSeedStep) / temperatureZoom);

            rawNoiseTemperature -= 0.5f;

            float distanceFromEquator = Mathf.Abs((worldHeight / 2) - y);
            distanceFromEquator /= worldHeight / 2;

            float temperatureNoise = distanceFromEquator;
            temperatureNoise += Temperature;
            temperatureNoise += rawNoiseTemperature * NoiseOnTemperatureImpact;

            temperatureNoise = 1 - temperatureNoise;

            if (noiseHight > waterLevel)
            {
                float temperatureChange;

                //temperature change depending on distance from equator
                temperatureChange = noiseHight - waterLevel;
                //smoothing
                temperatureChange = Mathf.Pow(temperatureChange, HoTImpactSmoothing);
                temperatureChange /= 1 - waterLevel;
                //temperature 0..1 => 1..2 for better smoothing
                temperatureNoise += 1;
                //calculating temperature change strange
                temperatureChange = Mathf.Pow(HoTImpactStrength, temperatureChange);

                temperatureNoise -= temperatureChange;
            }

            return temperatureNoise;
        }

        public float GetRiversValue(int x, int y)
        {
            float rawNoise = Mathf.PerlinNoise((x + worldSeed + riversSeedStep) / riversZoom, (y + worldSeed + riversSeedStep) / riversZoom);

            rawNoise -= riversLevel;

            float noiseMax;

            if (riversLevel < 0.5f)
                noiseMax = riversLevel;
            else
                noiseMax = 1f - riversLevel;

            float noise = rawNoise;
            noise = Mathf.Abs(noise);
            noise = Mathf.Clamp(noise, 0, noiseMax);

            noise *= 1f / noiseMax;

            noise = 1 - noise;
            noise = Mathf.Pow(noise, riversSharpness);
            noise *= riversDensity;

            return noise;
        }

        public float GetTestingValue(int x, int y)
        {
            float rawNoise = Mathf.PerlinNoise((x + worldSeed + testingSeedStep) / testingZoomX, (y + worldSeed + testingSeedStep) / testingZoomY);

            rawNoise -= testingMainLevel;

            float noiseMax;

            if (testingMainLevel < 0.5f)
                noiseMax = testingMainLevel;
            else
                noiseMax = 1f - testingMainLevel;

            float noise = rawNoise;
            noise = Mathf.Abs(noise);
            noise = Mathf.Clamp(noise, 0, noiseMax);

            noise *= 1f / noiseMax;

            noise = 1 - noise;
            noise = Mathf.Pow(noise, testingSmoothing);
            noise *= testingDensity;

            return noise;
        }

        public float GetTesting1Value(int x, int y)
        {
            float noise = Mathf.PerlinNoise((x + worldSeed + testing1SeedStep) / testing1Zoom, (y + worldSeed + testing1SeedStep) / testing1Zoom);
            noise *= testing1Density;
            return noise;
        }

        public Location[,] CreateWorld()
        {
            Location[,] map = new Location[worldWidth, worldHeight];

            for (int x = 0; x < worldWidth; x++)
            {
                for (int y = 0; y < worldHeight; y++)
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
            float river = GetRiversValue(x, y);


            if (height < waterLevel)
            {
                return GetWaterLocation(x, y, height);
            }
            else
            {

                if(river > 0.8f)
                {
                    return GetWaterLocation(x, y, height, true);
                }
                else
                {
                    return GetLandLocation(x, y, height);
                }
            }
        }

        private Location GetWaterLocation(int x, int y, float height, bool isRiver = false)
        {


            float temperature = GetTemperatureValue(x, y);

            if(temperature < 0.15f)
            {
                return new Location_ArcticDesert(x, y);
            }

            if (isRiver)
            {
                return new Location_River(x, y);
            }

            if (height > waterLevel / 2f)
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

            if(temperature < 0.15f)
            {
                return new Location_ArcticDesert(x, y);
            }

            if (height < waterLevel + 0.02f)
            {
                return new Location_SandBeach(x, y);
            }

            float heightStep = (1f - waterLevel) / 4f;

            if(height < waterLevel + heightStep * 2)
            {
                return new Location_Plain(x, y);
            }
            else if (height < waterLevel + heightStep * 3)
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

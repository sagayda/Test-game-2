using UnityEngine;

namespace Assets.Scripts
{
    public class WorldGenerator : MonoBehaviour
    {
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

        [SerializeField] float additionalHeightZoom = 30f;
        [SerializeField] int additionalHeightSeedStep = 4000;
        [SerializeField][Range(0, 10)] float additionalHeightDensity = 1.0f;

        [SerializeField][Range(0, 1)] float waterLevel = 0.35f;
        [SerializeField][Range(0, 100)] float layersMixStrength = 0.5f;

        [Header("Temperature map settings")]
        [SerializeField] float temperatureZoom = 30f;
        [SerializeField] int temperatureSeedStep = 3000;
        [SerializeField][Range(0, 10)] float temperatureDensity = 1.0f;

        [SerializeField] float HoTImpactStrength = 30f;
        [SerializeField] float HoTImpactSmoothing = 3.2f;
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
            float noise = Mathf.PerlinNoise((x + worldSeed + heightSeedStep) / heightZoom, (y + worldSeed + heightSeedStep) / heightZoom);
            noise *= heightDensity;

            float additionalNoise = Mathf.PerlinNoise((x + worldSeed + additionalHeightSeedStep) / additionalHeightZoom, (y + worldSeed + additionalHeightSeedStep) / additionalHeightZoom);
            additionalNoise *= additionalHeightDensity;

            noise = (noise * layersMixStrength + additionalNoise) / (layersMixStrength + 1f);

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

        public float GetTestingValue(int x, int y)
        {
            float rawNoise = Mathf.PerlinNoise((x + worldSeed + testingSeedStep) / testingZoomX, (y + worldSeed + testingSeedStep) / testingZoomY);
            rawNoise *= testingDensity;

            rawNoise -= testingMainLevel;
            //rawNoise = Mathf.Abs(rawNoise);

            float noise = testingMainLevel - rawNoise;
            noise *= 1 / testingMainLevel;

            //noise = 1 - noise;

            noise = Mathf.Pow(noise, testingSmoothing);


            return noise;
        }

        public float GetTesting1Value(int x, int y)
        {
            float noise = Mathf.PerlinNoise((x + worldSeed + testing1SeedStep) / testing1Zoom, (y + worldSeed + testing1SeedStep) / testing1Zoom);
            noise *= testing1Density;
            return noise;
        }
    }
}

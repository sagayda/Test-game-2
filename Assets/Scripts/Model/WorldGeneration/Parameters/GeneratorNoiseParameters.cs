using System;

namespace Assets.Scripts.Model.WorldGeneration
{
    [Serializable]
    public class NoiseParameters : ISavableGeneratorParameter
    {
        private readonly int _seedStep;
        private readonly float _zoom;
        private readonly float _density;

        public NoiseParameters(int seedStep, float zoom, float density)
        {
            _seedStep = seedStep;
            _zoom = zoom;
            _density = density;
        }

        public int SeedStep => _seedStep;
        public float Zoom => _zoom;
        public float Density => _density;
    } 

    [Serializable]
    public class HeightsNoiseParameters : ISavableGeneratorParameter
    {
        private readonly NoiseParameters _baseNoise;
        private readonly NoiseParameters _additionalNoise;

        private readonly float _layersMixStrength;
        private readonly float _waterLevel;
        private readonly float _riversSideLevel;

        public HeightsNoiseParameters(NoiseParameters baseNoise, NoiseParameters additionalNoise, float layersMixStrength, float waterLevel, float riverSideLevel)
        {
            _baseNoise = baseNoise;
            _additionalNoise = additionalNoise;
            _layersMixStrength = layersMixStrength;
            _waterLevel = waterLevel;
            _riversSideLevel = riverSideLevel;
        }

        public NoiseParameters BaseNoise => _baseNoise;
        public NoiseParameters AdditionalNoise => _additionalNoise;
        public float LayersMixStrength => _layersMixStrength;
        public float WaterLevel => _waterLevel;
        public float RiversSideLevel => _riversSideLevel;
    }

    [Serializable]
    public class TemperatureNoiseParameters : ISavableGeneratorParameter
    {
        private readonly NoiseParameters _baseNoise;

        private readonly float _heightImpactStrength;
        private readonly float _heightImpactSmoothing;
        private readonly float _noiseImpact;
        private readonly float _globalTemperature;

        public TemperatureNoiseParameters(NoiseParameters noiseBase, float heightImpactStrength, float heightImpactSmoothing, float noiseImpact, float globalTemperature)
        {
            _baseNoise = noiseBase;
            _heightImpactStrength = heightImpactStrength;
            _heightImpactSmoothing = heightImpactSmoothing;
            _noiseImpact = noiseImpact;
            _globalTemperature = globalTemperature;
        }

        public NoiseParameters BaseNoise => _baseNoise;
        public float HeightImpactStrength => _heightImpactStrength;
        public float HeightImpactSmoothing => _heightImpactSmoothing;
        public float NoiseImpact => _noiseImpact;
        public float GlobalTemperature => _globalTemperature;
    }

    [Serializable]
    public class PolutionNoiseParameters : ISavableGeneratorParameter
    {
        private readonly NoiseParameters _baseNoise;

        private readonly float _progressImpactStrength;
        private readonly float _progressImpactMultiplyer;
        private readonly float _progressImpactBottom;
        private readonly float _progressImpactTop;

        public PolutionNoiseParameters(NoiseParameters noiseBase, float progressImpactStrength, float progressImpactMultiplyer, float progressImpactBottom, float progressImpactTop)
        {
            _baseNoise = noiseBase;
            _progressImpactStrength = progressImpactStrength;
            _progressImpactMultiplyer = progressImpactMultiplyer;
            _progressImpactBottom = progressImpactBottom;
            _progressImpactTop = progressImpactTop;
        }

        public NoiseParameters BaseNoise => _baseNoise;
        public float ProgressImpactStrength => _progressImpactStrength;
        public float ProgressImpactMultiplyer => _progressImpactMultiplyer;
        public float ProgressImpactBottom => _progressImpactBottom;
        public float ProgressImpactTop => _progressImpactTop;
    }

    [Serializable]
    public class ProgressNoiseParameters : ISavableGeneratorParameter
    {
        private readonly NoiseParameters _baseNoise;

        public ProgressNoiseParameters(NoiseParameters noiseBase)
        {
            _baseNoise = noiseBase;
        }

        public NoiseParameters BaseNoise => _baseNoise;
    }

    [Serializable]
    public class RiversNoiseParameters : ISavableGeneratorParameter
    {
        private readonly NoiseParameters _baseNoise;
        private readonly NoiseParameters _additionalNoise;

        private readonly float _level = 0.5f;
        private readonly float _sharpness = 1f;

        public RiversNoiseParameters(NoiseParameters noiseBase, NoiseParameters additionalNoise, float level, float sharpness)
        {
            _baseNoise = noiseBase;
            _additionalNoise = additionalNoise;
            _level = level;
            _sharpness = sharpness;
        }

        public NoiseParameters BaseNoise => _baseNoise;
        public NoiseParameters AdditionalNoise => _additionalNoise;
        public float Sharpness => _sharpness;
        public float Level => _level;
    }
}

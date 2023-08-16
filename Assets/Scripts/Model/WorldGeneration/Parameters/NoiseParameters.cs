using System;

namespace Assets.Scripts.Model.WorldGeneration
{
    [Serializable]
    public class HeightsNoiseParameters : INoiseParameters
    {
        private readonly OctaveNoiseParameters _noiseParameters;

        private readonly float _waterLevel;

        public HeightsNoiseParameters(OctaveNoiseParameters noiseParameters, float waterLevel)
        {
            _noiseParameters = noiseParameters;
            _waterLevel = waterLevel;
        }

        public OctaveNoiseParameters Noise => _noiseParameters;
        public float WaterLevel => _waterLevel;
    }

    [Serializable]
    public class TemperatureNoiseParameters : INoiseParameters
    {
        private readonly OctaveNoiseParameters _noiseParameters;

        private readonly float _heightImpactStrength;
        private readonly float _heightImpactSmoothing;
        private readonly float _noiseImpact;
        private readonly float _globalTemperature;

        public TemperatureNoiseParameters(OctaveNoiseParameters noiseParameters, float heightImpactStrength, float heightImpactSmoothing, float noiseImpact, float globalTemperature)
        {
            _noiseParameters = noiseParameters;
            _heightImpactStrength = heightImpactStrength;
            _heightImpactSmoothing = heightImpactSmoothing;
            _noiseImpact = noiseImpact;
            _globalTemperature = globalTemperature;
        }

        public OctaveNoiseParameters Noise => _noiseParameters;
        public float HeightImpactStrength => _heightImpactStrength;
        public float HeightImpactSmoothing => _heightImpactSmoothing;
        public float NoiseImpact => _noiseImpact;
        public float GlobalTemperature => _globalTemperature;
    }

    [Serializable]
    public class PolutionNoiseParameters : INoiseParameters
    {
        private readonly OctaveNoiseParameters _noiseParameters;

        private readonly float _progressImpactStrength;
        private readonly float _progressImpactMultiplyer;
        private readonly float _progressImpactBottom;
        private readonly float _progressImpactTop;

        public PolutionNoiseParameters(OctaveNoiseParameters noiseParameters, float progressImpactStrength, float progressImpactMultiplyer, float progressImpactBottom, float progressImpactTop)
        {
            _noiseParameters = noiseParameters;
            _progressImpactStrength = progressImpactStrength;
            _progressImpactMultiplyer = progressImpactMultiplyer;
            _progressImpactBottom = progressImpactBottom;
            _progressImpactTop = progressImpactTop;
        }

        public OctaveNoiseParameters Noise => _noiseParameters;
        public float ProgressImpactStrength => _progressImpactStrength;
        public float ProgressImpactMultiplyer => _progressImpactMultiplyer;
        public float ProgressImpactBottom => _progressImpactBottom;
        public float ProgressImpactTop => _progressImpactTop;
    }

    [Serializable]
    public class ProgressNoiseParameters : INoiseParameters
    {
        private readonly OctaveNoiseParameters _noiseParameters;

        public ProgressNoiseParameters(OctaveNoiseParameters noiseParameters)
        {
            _noiseParameters = noiseParameters;
        }

        public OctaveNoiseParameters Noise => _noiseParameters;
    }

}

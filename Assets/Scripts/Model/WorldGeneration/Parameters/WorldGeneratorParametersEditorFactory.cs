using UnityEngine;

namespace Assets.Scripts.Model.WorldGeneration
{
    /// <summary>
    /// Класс, для создания и сохранения параметров генератора мира с помощью редактора Юнити
    /// А ещё он сделан максимально криво
    /// </summary>
    public class WorldGeneratorParametersEditorFactory : MonoBehaviour
    {
        [Header("Main parameters")]
        public long Seed;
        public uint Width;
        public uint Height;
        [Space(10)]
        [Header("Height parameters")]
        public int h_seedStep;
        public float h_zoom;
        public float h_density;
        [Space(5)]
        public int h_a_seedStep;
        public float h_a_zoom;
        public float h_a_density;
        [Space(5)]
        public float h_layersMixStrength;
        public float h_waterLevel;
        public float h_riversSideLevel;
        [Space(10)]
        [Header("Temperature parameters")]
        public int t_seedStep;
        public float t_zoom;
        public float t_density;
        [Space(5)]
        public float t_heightImpactStrength;
        public float t_heightImpactSmoothing;
        public float t_noiseImpact;
        public float t_globalTemperature;
        [Space(10)]
        [Header("Polution parameters")]
        public int po_seedStep;
        public float po_zoom;
        public float po_density;
        [Space(5)]
        public float po_progressImpactStrength;
        public float po_progressImpactMultiplyer;
        public float po_progressImpactBottom;
        public float po_progressImpactTop;
        [Space(10)]
        [Header("Progress parameters")]
        public int pr_seedStep;
        public float pr_zoom;
        public float pr_density;
        [Space(10)]
        [Header("Rivers parameters")]
        public int r_seedStep;
        public float r_zoom;
        public float r_density;
        [Space(5)]
        public int r_a_seedStep;
        public float r_a_zoom;
        public float r_a_density;
        [Space(5)]
        public float r_level = 0.5f;
        public float r_sharpness = 1f;

        private GeneratorParameters _generatorParameters;
        private HeightsNoiseParameters _heightsNoiseParameters;
        private TemperatureNoiseParameters _temperatureNoiseParameters;
        private PolutionNoiseParameters _polutionNoiseParameters;
        private ProgressNoiseParameters _progressNoiseParameters;
        private RiversNoiseParameters _riversNoiseParameters;

        private WorldGeneratorParametersFactory _parametersFactory = new();

        private void OnValidate()
        {
            Validate();
        }

        private void Validate()
        {

            NoiseParameters baseHeight = new(h_seedStep, h_zoom, h_density);
            NoiseParameters additionalHeight = new(h_a_seedStep, h_a_zoom, h_a_density);
            _heightsNoiseParameters = new(baseHeight, additionalHeight, h_layersMixStrength, h_waterLevel, h_riversSideLevel);

            NoiseParameters baseTemperature = new(t_seedStep, t_zoom, t_density);
            _temperatureNoiseParameters = new(baseTemperature, t_heightImpactStrength, t_heightImpactSmoothing, t_noiseImpact, t_globalTemperature);

            NoiseParameters basePolution = new(po_seedStep, po_zoom, po_density);
            _polutionNoiseParameters = new(basePolution, po_progressImpactStrength, po_progressImpactMultiplyer, po_progressImpactBottom, po_progressImpactTop);

            NoiseParameters baseProgress = new(pr_seedStep, pr_zoom, pr_density);
            _progressNoiseParameters = new(baseProgress);

            NoiseParameters baseRivers = new(r_seedStep, r_zoom, r_density);
            NoiseParameters additionalRivers = new(r_a_seedStep, r_a_zoom, r_a_density);
            _riversNoiseParameters = new(baseRivers, additionalRivers, r_level, r_sharpness);

            _generatorParameters = new(Seed, Width, Height, _progressNoiseParameters, _polutionNoiseParameters, _heightsNoiseParameters, _temperatureNoiseParameters, _riversNoiseParameters);

            Debug.Log($"validate {_generatorParameters.WorldWidth}");

            WorldGenerator.SetParameters(_generatorParameters);
        }

        [ContextMenu("SaveAll")]
        public void SaveAll()
        {
            _parametersFactory.SaveAsDefault(_generatorParameters);
        }

        [ContextMenu("SaveHeight")]
        public void SaveHeight()
        {
            _parametersFactory.SaveNoiseParameters(_heightsNoiseParameters);    
        }

        [ContextMenu("LoadDefault")]
        public void LoadDefault()
        {
            GeneratorParameters defaultParams = _parametersFactory.LoadDefault();

            var rivers = defaultParams.Rivers;

            r_seedStep = rivers.BaseNoise.SeedStep;
            r_zoom = rivers.BaseNoise.Zoom;
            r_density = rivers.BaseNoise.Density;

            r_a_seedStep = rivers.AdditionalNoise.SeedStep;
            r_a_zoom = rivers.AdditionalNoise.Zoom;
            r_a_density = rivers.AdditionalNoise.Density;

            r_level = rivers.Level;
            r_sharpness = rivers.Sharpness;

            WorldGenerator.SetParameters(defaultParams);
        }
    }
}

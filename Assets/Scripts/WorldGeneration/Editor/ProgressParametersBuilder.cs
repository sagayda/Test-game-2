using UnityEngine;
using WorldGeneration.Core;

namespace WorldGeneration.Editor
{
    [CreateAssetMenu(fileName = "ProgressParams", menuName = "World generator/Create progress params")]
    [ExecuteInEditMode]
    public class ProgressParametersBuilder : BaseParametersBuilder<ProgressNoiseParameters>
    {
        public override ProgressNoiseParameters LastBuilded { get => _lastBuilded; set => _lastBuilded = value; }
        private ProgressNoiseParameters _lastBuilded;

        private void OnValidate()
        {
            _lastBuilded = Build();
        }

        public override ProgressNoiseParameters Build()
        {
            return new ProgressNoiseParameters(BuildBase());
        }

        public override void Load()
        {
            NoiseParametersSave parametersSave = new NoiseParametersSave();

            ProgressNoiseParameters loadedParams = parametersSave.LoadNoiseParameters<ProgressNoiseParameters>();

            LoadBase(loadedParams.Noise);

            _lastBuilded = loadedParams;
        }
    }
}

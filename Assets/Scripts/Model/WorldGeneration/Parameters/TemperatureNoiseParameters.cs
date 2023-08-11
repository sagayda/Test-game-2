using UnityEngine;

namespace Assets.Scripts.Model.WorldGeneration
{
    [CreateAssetMenu(fileName = "TemperatureNoiseParameters", menuName = "World generator/Create temperature parameters")]
    public class TemperatureNoiseParameters : NoiseParameters
    {
        [SerializeField][Range(0.01f, 200)] private float _heightImpactStrength;
        [SerializeField][Range(0.01f, 10)] private float _heightImpactSmoothing;
        //Это что?
        [SerializeField][Range(0, 5)] private float _noiseOnTemperatureImpact;
        [SerializeField][Range(-1, 1)] private float _globalTemperature;

        public float HeightImpactStrength => _heightImpactStrength;
        public float HeightImpactSmoothing => _heightImpactSmoothing;
        public float NoiseOnTemperatureImpact => _noiseOnTemperatureImpact;
        public float GlobalTemperature => _globalTemperature;
    }
}

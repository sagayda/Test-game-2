using UnityEngine;

namespace Assets.Scripts.Model.WorldGeneration
{
    [CreateAssetMenu(fileName = "PolutionNoiseParameters", menuName = "World generator/Create polution parameters")]
    public class PolutionNoiseParameters : NoiseParameters
    {
        [SerializeField] private float _progressImpactStrength;
        [SerializeField] private float _progressImpactMultiplyer;
        [SerializeField] private float _progressImpactBottom;
        [SerializeField][Range(0, 10)] private float _progressImpactTop;

        public float ProgressImpactStrength => _progressImpactStrength;
        public float ProgressImpactMultiplyer => _progressImpactMultiplyer;
        public float ProgressImpactBottom => _progressImpactBottom;
        public float ProgressImpactTop => _progressImpactTop;
    }
}

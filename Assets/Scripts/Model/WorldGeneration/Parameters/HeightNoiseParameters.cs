using UnityEngine;

namespace Assets.Scripts.Model.WorldGeneration
{
    [CreateAssetMenu(fileName = "HeightNoiseParameters", menuName = "World generator/Create height parameters")]
    public class HeightNoiseParameters : NoiseParameters
    {
        [SerializeField] private NoiseParameters _additionalNoise;

        [SerializeField][Range(0, 100)] private float _layersMixStrength;
        [Space]
        [SerializeField][Range(0, 1)] private float _waterLevel;
        [SerializeField] private float _riversSideLevel;

        public NoiseParameters AdditionalNoise => _additionalNoise;
        public float LayersMixStrength => _layersMixStrength;
        public float WaterLevel => _waterLevel;
        public float RiversSideLevel => _riversSideLevel;
    }
}

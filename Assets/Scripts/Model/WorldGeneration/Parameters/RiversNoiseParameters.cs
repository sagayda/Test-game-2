using UnityEngine;

namespace Assets.Scripts.Model.WorldGeneration
{
    [CreateAssetMenu(fileName = "RiversNoiseParameters", menuName = "World generator/Create rivers parameters")]
    public class RiversNoiseParameters : NoiseParameters
    {
        [SerializeField][Range(0, 1)] private float _level = 0.5f;
        [SerializeField][Range(0, 20)] private float _sharpness = 1f;

        public float Sharpness => _sharpness;
        public float Level => _level;
    }
}

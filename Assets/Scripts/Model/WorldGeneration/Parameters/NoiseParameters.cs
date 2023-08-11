using UnityEngine;

namespace Assets.Scripts.Model.WorldGeneration
{
    /// <summary>
    /// Базовый класс параметров для генерации карты на основе шума Перлина
    /// </summary>
    [CreateAssetMenu(fileName = "NoiseParameters", menuName = "World generator/Create base noise parameters")]
    public class NoiseParameters : ScriptableObject
    {
        [SerializeField] private float _zoom;
        [SerializeField] private int _seedStep;
        [SerializeField][Range(0, 10)] private float _density;

        public float Zoom => _zoom;
        public float SeedStep => _seedStep;
        public float Density => _density;
    }
}

using NaughtyAttributes;
using UnityEngine;
using WorldGeneration.Core;

namespace WorldGeneration.Editor
{
    [ExecuteInEditMode]
    public abstract class BaseParametersBuilder<T> : ScriptableObject where T : INoiseParameters
    {
        [Header("NoiseSettings")]
        [ContextMenuItem("Save", nameof(Save))]
        [ContextMenuItem("Load", nameof(Load))]
        public string Seed;
        public float SeedStep;
        [Range(0.0001f, 0.1f)] public float Zoom;
        [Range(1, 16)] public int Octaves;
        [Range(0.1f, 2f)] public float Persistance;
        [Range(1f, 4f)] public float Lacunarity;
        public EasingEnum Easing;

        public abstract T LastBuilded { get; set; }
        protected OctaveNoiseParameters _lastBuildedBase;

        private void OnValidate()
        {
            _lastBuildedBase = BuildBase();
        }

        public abstract T Build();

        public OctaveNoiseParameters BuildBase()
        {
            IEasingStrategy easingStrategy = Easing switch
            {
                EasingEnum.Linear => new EaseLinear(),
                EasingEnum.InOutSine => new EaseInOutSine(),
                EasingEnum.InQuad => new EaseInQuad(),
                EasingEnum.InSine => new EaseInSine(),
                _ => new EaseLinear(),
            };
            GeneratorParameters.ComputeSeed(Seed, out float seedX, out float seedY);

            return new OctaveNoiseParameters(seedX, seedY, SeedStep, Zoom, Octaves, Persistance, Lacunarity, easingStrategy);
        }

        [Button]
        public void Save()
        {
            NoiseParametersSave parametersSave = new NoiseParametersSave();

            parametersSave.SaveNoiseParameters(LastBuilded);
        }

        [Button]
        public virtual void Load()
        {
            NoiseParametersSave parametersSave = new NoiseParametersSave();

            LoadBase(parametersSave.LoadNoiseParameters<T>().Noise);
        }

        protected void LoadBase(OctaveNoiseParameters parameters)
        {
            Zoom = parameters.Zoom;
            Octaves = parameters.Octaves;
            Persistance = parameters.Persistance;
            Lacunarity = parameters.Lacunarity;

            _lastBuildedBase = parameters;
        }
    }

    public enum EasingEnum
    {
        Linear,
        InOutSine,
        InQuad,
        InSine,
    }
}

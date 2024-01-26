using NaughtyAttributes;
using UnityEngine;
using UniversalTools;
using WorldGeneration.Core;
using WorldGeneration.Core.Noise;

namespace WorldGeneration.Editor
{
    [CreateAssetMenu(fileName = "NoiseParams", menuName = "World generator/Create noise parameters")]
    [ExecuteInEditMode]
    public class FractalNoiseParametersBuilder : ScriptableObject
    {
        private FractalNoise _noiseProvider;

        private float _lastPaintTime;
        private const float _minTimeBetweenPaint = 0.25f;

        #region Editor fields
        public ParametersSave.SaveSlot SaveSlot;
        public bool EnableVisualizing = false;
        [BoxGroup("Visualizing")]
        [ShowIf("EnableVisualizing")]
        [ShowAssetPreview(2048, 2048)]
        public Sprite Renderer;
        [BoxGroup("Visualizing")]
        [ShowIf("EnableVisualizing")]
        public string Seed;
        [BoxGroup("Visualizing")]
        [ShowIf("EnableVisualizing")]
        [Min(1)]
        public int ColorLerpRate = 5;
        [ShowIf("EnableVisualizing")]
        [BoxGroup("Visualizing")]
        [Range(16, 1024)]
        public uint Width = 16;
        [ShowIf("EnableVisualizing")]
        [BoxGroup("Visualizing")]
        [Range(16, 1024)]
        public uint Height = 16;

        [Header("NoiseSettings")]
        public float XSeedStep;
        public float YSeedStep;
        [Space(10)]
        [Range(1, 16)] public int Octaves;
        [Range(0f, 20f)] public float Amplitude;
        [Range(0.0001f, 0.1f)] public float Frequency;
        [Range(0.1f, 2f)] public float Persistance;
        [Range(1f, 4f)] public float Lacunarity;
        public EasingFunction.Ease EaseFunction;
        #endregion

        public FractalNoiseParameters NoiseParameters => Build();

        private void OnValidate()
        {
            if (EnableVisualizing)
            {
                if (Time.timeSinceLevelLoad - _lastPaintTime >= _minTimeBetweenPaint)
                {
                    _lastPaintTime = Time.timeSinceLevelLoad;
                    Renderer = Paint();
                }
            }
        }

        private FractalNoiseParameters Build()
        {
            return new FractalNoiseParameters(Octaves, Amplitude, Frequency, Persistance, Lacunarity, XSeedStep, YSeedStep, EaseFunction);
        }

        [Button("Save")]
        private void OnSaveButtonClick()
        {
            Save();
        }

        [Button("Load")]
        private void OnLoadButtonClick()
        {
            Load();

            if (EnableVisualizing)
                Renderer = Paint();
        }

        private Sprite Paint()
        {
            Texture2D texture = new((int)Width, (int)Height);

            SetPaintingParameters();

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    float noise = GetNoise(x, y);

                    Color color = GetColor(noise);

                    texture.SetPixel(x, y, color);
                }
            }

            texture.filterMode = FilterMode.Point;
            texture.Apply();

            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, Width, Height), new Vector2(0, 0));
            return sprite;
        }

        protected virtual void Save()
        {
            ParametersSave.SaveParameters(NoiseParameters, SaveSlot);
        }

        protected virtual void Load()
        {
            var loaded = ParametersSave.LoadParameters<FractalNoiseParameters>(SaveSlot);

            if (loaded.HasValue)
            {
                XSeedStep = loaded.Value.XSeedStep;
                YSeedStep = loaded.Value.YSeedStep;
                Octaves = loaded.Value.Octaves;
                Amplitude = loaded.Value.Amplitude;
                Frequency = loaded.Value.Frequency;
                Persistance = loaded.Value.Persistance;
                Lacunarity = loaded.Value.Lacunarity;
            }
        }

        protected virtual void SetPaintingParameters()
        {
            _noiseProvider = new(NoiseParameters, WorldGenerator.ComputeInt32Seed(Seed));
        }

        protected virtual float GetNoise(int x, int y)
        {
            return _noiseProvider.Generate(x,y);
        }

        protected virtual Color GetColor(float noise)
        {
            return StepwiseColorLerp(Color.white, Color.black, 0f, 1f, noise);
        }

        protected Color StepwiseColorLerp(Color color1, Color color2, float minT, float maxT, float t)
        {
            t -= minT;
            t *= 1f / (maxT - minT);

            t = RoundLerpRate(t, ColorLerpRate);

            return Color.Lerp(color1, color2, t);
        }

        protected static float RoundLerpRate(float t, int lerpRate)
        {
            float step = 1f / lerpRate;

            float diff = t % step;
            return t - diff;
        }
    }
}

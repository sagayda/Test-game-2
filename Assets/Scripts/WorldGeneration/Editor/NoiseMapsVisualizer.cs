using UnityEngine;
using WorldGeneration.Core;

namespace WorldGeneration.Editor
{
    public class NoiseMapsVisualizer : MonoBehaviour
    {
        public string Seed;
        public uint Width;
        public uint Height;
        [Range(0, 1)] public float ColorStep;
        [Space(10)]
        public HeightParametersBuilder HeightBuilder;
        public SpriteRenderer HeightRenderer;
        [Space(10)]
        public TemperatureParametersBuilder TemperatureBuilder;
        public SpriteRenderer TemperatureRenderer;
        [Space(10)]
        public ProgressParametersBuilder ProgressBuilder;
        public SpriteRenderer ProgressRenderer;
        [Space(10)]
        public PolutionParametersBuilder PolutionBuilder;
        public SpriteRenderer PolutionRenderer;
        [Space(20)]
        public bool RepaintHeight = false;
        public bool RepaintTemperature = false;
        public bool RepaintProgress = false;
        public bool RepaintPolution = false;

        private void Update()
        {
            if (RepaintHeight)
                PaintHeight();

            if (RepaintTemperature)
                PaintTemperature();

            if (RepaintProgress)
                PaintProgress();

            if (RepaintPolution)
                PaintPolution();

        }

        private void PaintHeight()
        {
            Texture2D texture = new((int)Width, (int)Height);

            WorldGenerator worldGenerator = BuildWorldGenerator();

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    float noise = worldGenerator.GetHeightValue(x, y);
                    float waterLevel = worldGenerator.WaterLevel;

                    if (noise < 0 || noise > 1)
                        Debug.Log($"X:{x} Y:{y} noise : {noise}");

                    Color color;

                    float landSize = 1 - waterLevel;
                    float landStep = landSize / 3;

                    if (noise <= waterLevel)
                        color = FastColorLerp(Color.black, new Color(0f, 0.4f, 1f), 0, waterLevel, noise);
                    else if (noise <= waterLevel + landStep)
                        color = FastColorLerp(new Color(0.5f, 0.95f, 0f), new Color(0, 0.4f, 0), waterLevel, waterLevel + landStep, noise);
                    else if (noise <= waterLevel + landStep * 2)
                        color = FastColorLerp(new Color(0, 0.4f, 0), new Color(0.55f, 0.55f, 0), waterLevel + landStep, waterLevel + landStep * 2, noise);
                    else
                        color = FastColorLerp(new Color(0.55f, 0.55f, 0), new Color(0.45f, 0f, 0f), waterLevel + landStep * 2, 1, noise);

                    texture.SetPixel(x, y, color);
                }
            }

            texture.Apply();
            texture.filterMode = FilterMode.Point;

            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, Width, Height), new Vector2(0, 0));
            HeightRenderer.sprite = sprite;
        }

        private void PaintTemperature()
        {
            Texture2D texture = new((int)Width, (int)Height);

            WorldGenerator worldGenerator = BuildWorldGenerator();

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    float noise = worldGenerator.GetTemperatureValue(x, y);

                    Color color;
                    if (noise <= 0.25f)
                        color = FastColorLerp(Color.white, Color.blue, 0, 0.25f, noise);
                    else if (noise <= 0.5f)
                        color = FastColorLerp(Color.blue, Color.green, 0.25f, 0.5f, noise);
                    else if (noise <= 0.75f)
                        color = FastColorLerp(Color.green, Color.yellow, 0.5f, 0.75f, noise);
                    else
                        color = FastColorLerp(Color.yellow, Color.red, 0.75f, 1f, noise);

                    texture.SetPixel(x, y, color);
                }
            }

            texture.filterMode = FilterMode.Point;
            texture.Apply();

            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, Width, Height), new Vector2(0, 0));
            TemperatureRenderer.sprite = sprite;
        }

        private void PaintProgress()
        {
            Texture2D texture = new((int)Width, (int)Height);

            WorldGenerator worldGenerator = BuildWorldGenerator();

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    float noise = worldGenerator.GetProgressValue(x, y);

                    Color color = FastColorLerp(Color.white, Color.red, 0f, 1f, noise);
                    texture.SetPixel(x, y, color);
                }
            }

            texture.filterMode = FilterMode.Point;
            texture.Apply();

            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, Width, Height), new Vector2(0, 0));
            ProgressRenderer.sprite = sprite;
        }

        private void PaintPolution()
        {
            Texture2D texture = new((int)Width, (int)Height);

            WorldGenerator worldGenerator = BuildWorldGenerator();

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    float noise = worldGenerator.GetPolutionValue(x, y);

                    Color color = FastColorLerp(Color.white, new Color(0, 0.7f, 0), 0f, 1f, noise);
                    texture.SetPixel(x, y, color);
                }
            }

            texture.filterMode = FilterMode.Point;
            texture.Apply();

            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, Width, Height), new Vector2(0, 0));
            PolutionRenderer.sprite = sprite;
        }

        private WorldGenerator BuildWorldGenerator()
        {
            return new(new(Seed, Width, Height, ProgressBuilder.Build(), PolutionBuilder.Build(), HeightBuilder.Build(), TemperatureBuilder.Build()));

        }

        private Color FastColorLerp(Color a, Color b, float min, float max, float noise)
        {
            noise -= min;
            noise *= 1f / (max - min);
            noise = RoundNoiseToStep(noise);

            return Color.Lerp(a, b, noise);
        }

        private float RoundNoiseToStep(float t)
        {
            float diff = t % ColorStep;
            return t - diff;
        }

        [ContextMenu("Save parameters as default")]
        private void SaveGeneratorParametersAsDefault()
        {
            NoiseParametersSave parametersSave = new NoiseParametersSave();

            GeneratorParameters generatorParameters = new(Seed, Width, Height, ProgressBuilder.Build(), PolutionBuilder.Build(), HeightBuilder.Build(), TemperatureBuilder.Build());

            parametersSave.SaveAsDefault(generatorParameters);
        }
    }
}

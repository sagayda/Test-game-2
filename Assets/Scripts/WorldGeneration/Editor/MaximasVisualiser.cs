using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using WorldGeneration.Core;
using WorldGeneration.Core.Outdate;

namespace WorldGeneration.Editor
{
    public class MaximasVisualiser : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _maximasRenderer;
        [SerializeField] private SpriteRenderer _minimasRenderer;
        [SerializeField] private SpriteRenderer _riversRenderer;
        [SerializeField] private SpriteRenderer _heightsRenderer;
        [SerializeField] private SpriteRenderer _riversPointsRenderer;

        //private WorldGenerator _generator = new(new ("12", 256,256));
        private WorldGenerator _generator = new("0", 256, 256, 0.4f, CompositeValueMap.CreateDefault(0));

        private RiversGeneratorParameters riversGeneratorParameters => new(_generator, 0, 256, 0.8f, 0.75f, 4);

        private RiversGenerator _riversGenerator;

        private void Awake()
        {
            _riversGenerator = new(riversGeneratorParameters);

            _maximasRenderer.sprite = PaintMaximas();
            _minimasRenderer.sprite = PaintMinimas();
            _heightsRenderer.sprite = PaintHeights();
            //_riversRenderer.sprite = PaintRivers();
        }

        private Sprite PaintHeights()
        {
            Texture2D texture = new((int)_generator.Width, (int)_generator.Height);

            for (int x = 0; x < _generator.Width; x++)
            {
                for (int y = 0; y < _generator.Height; y++)
                {
                    float noise = _generator.GetMapValue(x, y, MapValueType.Height);

                    Color color = GetColor(noise);

                    texture.SetPixel(x, y, color);
                }
            }

            texture.filterMode = FilterMode.Point;
            texture.Apply();

            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, _generator.Width, _generator.Height), new Vector2(0, 0));
            return sprite;
        }

        private Sprite PaintMaximas()
        {
            RiversGenerator riversGenerator = new(riversGeneratorParameters);

            var maximas = riversGenerator.FindLocalMaximas(0.8f, 1f);

            Texture2D texture = new((int)_generator.Width, (int)_generator.Height);

            for (int i = 0; i < _generator.Width; i++)
            {
                for (int j = 0; j < _generator.Height; j++)
                {
                    texture.SetPixel(i, j, new Color(0, 0, 0, 0));
                }
            }

            foreach (var maxima in maximas)
            {
                texture.SetPixel(maxima.x, maxima.y, Color.red);
            }

            texture.filterMode = FilterMode.Point;
            texture.Apply();

            return Sprite.Create(texture, new Rect(0, 0, _generator.Width, _generator.Height), new Vector2(0, 0));
        }

        private Sprite PaintMinimas()
        {
            RiversGenerator riversGenerator = new(riversGeneratorParameters);

            var maximas = riversGenerator.FindLocalMinimas(0f, 0.75f);

            Texture2D texture = new((int)_generator.Width, (int)_generator.Height);

            for (int i = 0; i < _generator.Width; i++)
            {
                for (int j = 0; j < _generator.Height; j++)
                {
                    texture.SetPixel(i, j, new Color(0, 0, 0, 0));
                }
            }

            foreach (var maxima in maximas)
            {
                texture.SetPixel(maxima.x, maxima.y, Color.yellow);
            }

            texture.filterMode = FilterMode.Point;
            texture.Apply();

            return Sprite.Create(texture, new Rect(0, 0, _generator.Width, _generator.Height), new Vector2(0, 0));
        }

        private Sprite PaintRivers()
        {
            //RiversGenerator riversGenerator = new(riversGeneratorParameters);
            //riversGenerator.PerlinWorms = new(new(_progressParametersBuilder.Build().Noise));

            //riversGenerator.GenerateRivers();
            //riversGenerator.GenerateRivers2();

            Texture2D texture = new((int)_generator.Width, (int)_generator.Height);

            for (int i = 0; i < _generator.Width; i++)
            {
                for (int j = 0; j < _generator.Height; j++)
                {
                    if (_riversGenerator.RiverMap2[i, j] > 0)
                        texture.SetPixel(i, j, Color.blue);
                    else if (_riversGenerator.RiverMap2[i, j] < 0)
                        texture.SetPixel(i, j, Color.magenta);
                    else
                        texture.SetPixel(i, j, new Color(0, 0, 0, 0));
                }
            }

            texture.filterMode = FilterMode.Point;
            texture.Apply();

            //_riversPointsRenderer.sprite = PaintRiversPoints(riversGenerator.riversPoints);

            return Sprite.Create(texture, new Rect(0, 0, _generator.Width, _generator.Height), new Vector2(0, 0));
        }

        private Sprite PaintRiversPoints(List<Vector2Int> points)
        {
            Texture2D texture = new((int)_generator.Width, (int)_generator.Height);

            for (int i = 0; i < _generator.Width; i++)
            {
                for (int j = 0; j < _generator.Height; j++)
                {
                    texture.SetPixel(i, j, new Color(0, 0, 0, 0));
                }
            }

            foreach (var maxima in points)
            {
                texture.SetPixel(maxima.x, maxima.y, Color.magenta);
            }

            texture.filterMode = FilterMode.Point;
            texture.Apply();

            return Sprite.Create(texture, new Rect(0, 0, _generator.Width, _generator.Height), new Vector2(0, 0));
        }

        protected Color GetColor(float noise)
        {
            Color color;

            float landSize = 1 - _generator.OceanLevel;
            float landStep = landSize / 3;

            if (noise <= _generator.OceanLevel)
                color = StepwiseColorLerp(Color.black, new Color(0f, 0.4f, 1f), 0, _generator.OceanLevel, noise);
            else if (noise <= _generator.OceanLevel + landStep)
                color = StepwiseColorLerp(new Color(0.5f, 0.95f, 0f), new Color(0, 0.4f, 0), _generator.OceanLevel, _generator.OceanLevel + landStep, noise);
            else if (noise <= _generator.OceanLevel + landStep * 2)
                color = StepwiseColorLerp(new Color(0, 0.4f, 0), new Color(0.55f, 0.55f, 0), _generator.OceanLevel + landStep, _generator.OceanLevel + landStep * 2, noise);
            else
                color = StepwiseColorLerp(new Color(0.55f, 0.55f, 0), new Color(0.45f, 0f, 0f), _generator.OceanLevel + landStep * 2, 1, noise);

            return color;
        }

        protected Color StepwiseColorLerp(Color color1, Color color2, float minT, float maxT, float t)
        {
            t -= minT;
            t *= 1f / (maxT - minT);

            t = RoundLerpRate(t, 4);

            return Color.Lerp(color1, color2, t);
        }

        protected static float RoundLerpRate(float t, int lerpRate)
        {
            float step = 1f / lerpRate;

            float diff = t % step;
            return t - diff;
        }

        [Button("Create sources")]
        private void OnCreateSourcesButtonClick()
        {
            _riversGenerator.FindMaximasAndCreateSources();
        }

        [Button("Iterate sources")]
        private void OnIterateSourcesButtonCLick()
        {
            _riversGenerator.IterateAllSources();
            _riversRenderer.sprite = PaintRivers();
        }
    }
}

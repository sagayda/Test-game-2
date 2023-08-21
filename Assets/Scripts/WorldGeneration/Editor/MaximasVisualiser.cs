using System.Collections.Generic;
using Assets.Scripts.WorldGeneration.Core;
using UnityEngine;
using WorldGeneration.Core;

namespace WorldGeneration.Editor
{
    public class MaximasVisualiser : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _maximasRenderer;
        [SerializeField] private SpriteRenderer _minimasRenderer;
        [SerializeField] private SpriteRenderer _riversRenderer;
        [SerializeField] private SpriteRenderer _riversPointsRenderer;

        private WorldGenerator _generator = new(new ("12", 256,256));
        private RiversGeneratorParameters riversGeneratorParameters => new(_generator, 1646, 256, 0.8f, 0.75f, 4);


        private void Awake()
        {
            _maximasRenderer.sprite = PaintMaximas();
            _minimasRenderer.sprite = PaintMinimas();
            //_riversRenderer.sprite = PaintRivers();
        }

        private Sprite PaintMaximas()
        {

            RiversGenerator riversGenerator = new(riversGeneratorParameters);

            var maximas = riversGenerator.FindLocalMaximas(0.8f, 1f);

            Texture2D texture = new((int)_generator.WorldWidth, (int)_generator.WorldHeight);

            for (int i = 0; i < _generator.WorldWidth; i++)
            {
                for (int j = 0; j < _generator.WorldHeight; j++)
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

            return Sprite.Create(texture, new Rect(0, 0, _generator.WorldWidth, _generator.WorldHeight), new Vector2(0, 0));
        }

        private Sprite PaintMinimas()
        {
            RiversGenerator riversGenerator = new(riversGeneratorParameters);

            var maximas = riversGenerator.FindLocalMinimas(0f, 0.75f);

            Texture2D texture = new((int)_generator.WorldWidth, (int)_generator.WorldHeight);

            for (int i = 0; i < _generator.WorldWidth; i++)
            {
                for (int j = 0; j < _generator.WorldHeight; j++)
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

            return Sprite.Create(texture, new Rect(0, 0, _generator.WorldWidth, _generator.WorldHeight), new Vector2(0, 0));
        }

        private Sprite PaintRivers()
        {
            RiversGenerator riversGenerator = new(riversGeneratorParameters);
            //riversGenerator.PerlinWorms = new(new(_progressParametersBuilder.Build().Noise));

            //riversGenerator.GenerateRivers();
            riversGenerator.GenerateRivers2();

            Texture2D texture = new((int)_generator.WorldWidth, (int)_generator.WorldHeight);

            for (int i = 0; i < _generator.WorldWidth; i++)
            {
                for (int j = 0; j < _generator.WorldHeight; j++)
                {
                    if (riversGenerator.RiverMap2[i, j] > 0)
                        texture.SetPixel(i, j, Color.blue);
                    else if (riversGenerator.RiverMap2[i, j] < 0)
                        texture.SetPixel(i, j, Color.magenta);
                    else
                        texture.SetPixel(i, j, new Color(0, 0, 0, 0));
                }
            }

            texture.filterMode = FilterMode.Point;
            texture.Apply();

            _riversPointsRenderer.sprite = PaintRiversPoints(riversGenerator.riversPoints);

            return Sprite.Create(texture, new Rect(0, 0, _generator.WorldWidth, _generator.WorldHeight), new Vector2(0, 0));
        }

        private Sprite PaintRiversPoints(List<Vector2Int> points)
        {
            Texture2D texture = new((int)_generator.WorldWidth, (int)_generator.WorldHeight);

            for (int i = 0; i < _generator.WorldWidth; i++)
            {
                for (int j = 0; j < _generator.WorldHeight; j++)
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

            return Sprite.Create(texture, new Rect(0, 0, _generator.WorldWidth, _generator.WorldHeight), new Vector2(0, 0));
        }
    }
}

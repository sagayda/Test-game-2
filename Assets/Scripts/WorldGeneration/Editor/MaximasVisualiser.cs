using UnityEngine;
using WorldGeneration.Core;

namespace WorldGeneration.Editor
{
    public class MaximasVisualiser : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _maximasRenderer;
        [SerializeField] private SpriteRenderer _minimasRenderer;
        [SerializeField] private SpriteRenderer _riversRenderer;
        [SerializeField] ProgressParametersBuilder _progressParametersBuilder;

        private WorldGenerator _generator = new(new NoiseParametersSave().Default);

        private void Awake()
        {
            _maximasRenderer.sprite = PaintMaximas();
            _minimasRenderer.sprite = PaintMinimas();
            _riversRenderer.sprite = PaintRivers();
        }

        private Sprite PaintMaximas()
        {
            RiversGenerator riversGenerator = new(_generator, _progressParametersBuilder.LastBuilded.Noise);

            var maximas = riversGenerator.FindLocalMaximas();

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
            RiversGenerator riversGenerator = new(_generator, _progressParametersBuilder.LastBuilded.Noise);

            var maximas = riversGenerator.FindLocalMinimas();

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
            RiversGenerator riversGenerator = new(_generator, _progressParametersBuilder.LastBuilded.Noise);
            riversGenerator.GenerateRivers();

            Texture2D texture = new((int)_generator.WorldWidth, (int)_generator.WorldHeight);

            for (int i = 0; i < _generator.WorldWidth; i++)
            {
                for (int j = 0; j < _generator.WorldHeight; j++)
                {
                    if (riversGenerator.RiverMap[i, j] == 0)
                        texture.SetPixel(i, j, new Color(0, 0, 0, 0));
                    else
                        texture.SetPixel(i, j, Color.blue);
                }
            }

            texture.filterMode = FilterMode.Point;
            texture.Apply();

            return Sprite.Create(texture, new Rect(0, 0, _generator.WorldWidth, _generator.WorldHeight), new Vector2(0, 0));
        }
    }
}

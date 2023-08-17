using Assets.Scripts.WorldGeneration.Core;
using UnityEngine;
using WorldGeneration.Core;
using Core;

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

            BoundedVector2 test1 = new BoundedVector2(new Vector2(1, 2), new Vector2(0, 0), new Vector2(2, 2));

            test1.ChangeBounds(new(10, 10), new(20, 20));
            Debug.Log(test1);

            //test1.Value = 1;
            //Debug.Log(test1);
            //test1.Value = -1;
            //Debug.Log(test1);
            //test1.Value = 50;
            //Debug.Log(test1);

            //Debug.Log(test1.IsOnMaximum);
            //Debug.Log(test1.IsOnMinimum);
            //test1.Minimise();
            //Debug.Log(test1);
        }

        private Sprite PaintMaximas()
        {
            RiversGeneratorParameters riversGeneratorParameters = new(_generator, 1646, 256, 0.8f, 0.75f, 2);

            RiversGenerator riversGenerator = new(riversGeneratorParameters);

            var maximas = riversGenerator.FindLocalMaximas(0.8f,1f);

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
            RiversGeneratorParameters riversGeneratorParameters = new(_generator, 1646, 256, 0.8f, 0.75f, 2);

            RiversGenerator riversGenerator = new(riversGeneratorParameters);

            var maximas = riversGenerator.FindLocalMinimas(0f,0.75f);

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
            RiversGeneratorParameters riversGeneratorParameters = new(_generator, 1646, 256, 0.8f, 0.75f, 2);
            RiversGenerator riversGenerator = new(riversGeneratorParameters);
            riversGenerator.PerlinWorms = new(new(_progressParametersBuilder.Build().Noise));

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

using Assets.Scripts.Model.WorldGeneration;
using UnityEngine;

namespace Assets.Scripts.Model.Tester
{
    public class WorldMapsVisualizer : MonoBehaviour
    {
        [SerializeField] private GeneratorParameters _generatorParameters;

        [SerializeField] private SpriteRenderer _spriteRendererProgress;
        [SerializeField] private SpriteRenderer _spriteRendererPolution;
        [SerializeField] private SpriteRenderer _spriteRendererHeight;
        [SerializeField] private SpriteRenderer _spriteRendererTemperature;
        [SerializeField] private SpriteRenderer _spriteRendererRivers;
        [SerializeField] private SpriteRenderer _spriteRendererCombined;
        [SerializeField] private int _combinedMapCellSize;
        [Header("Update")]
        [SerializeField] private bool _updateMaps = true;
        [SerializeField] private bool _updateProgress = false;
        [SerializeField] private bool _updatePolution = false;
        [SerializeField] private bool _updateHeight = false;
        [SerializeField] private bool _updateTemperature = false;
        [SerializeField] private bool _updateRivers = false;

        private int width => (int)WorldGenerator.WorldWidth;
        private int height => (int)WorldGenerator.WorldHeight;
        private float waterLevel => (int)WorldGenerator.WaterLevel;

        private void OnValidate()
        {
            //if(_generatorParameters != null)
            //    WorldGenerator.SetParameters(_generatorParameters);
        }

        private void Update()
        {
            if (_updateMaps)
            {
                if (_updateProgress)
                    PaintProgressMap();

                if (_updatePolution)
                    PaintPolutionMap();

                if (_updateHeight)
                    PaintHeightMap();

                if (_updateTemperature)
                    PaintTemperatureMap();

                if (_updateRivers)
                    PaintRiversMap();
            }
        }

        private void PaintProgressMap()
        {
            Texture2D texture = new(width, height);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float noise = WorldGenerator.GetProgressValue(x, y);

                    Color color = Color.Lerp(Color.white, Color.red, noise);
                    texture.SetPixel(x, y, color);
                }
            }

            texture.Apply();

            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, width, height), new Vector2(0, 0));
            _spriteRendererProgress.sprite = sprite;
        }

        private void PaintPolutionMap()
        {
            Texture2D texture = new(width, height);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float noise = WorldGenerator.GetPolutionValue(x, y);

                    Color color = Color.Lerp(Color.white, new Color(0, 0.7f, 0), noise);
                    texture.SetPixel(x, y, color);
                }
            }

            texture.Apply();

            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, width, height), new Vector2(0, 0));
            _spriteRendererPolution.sprite = sprite;
        }

        private void PaintHeightMap()
        {
            Texture2D texture = new(width, height);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float noise = WorldGenerator.GetHeightValue(x, y);

                    if (noise < 0 || noise > 1)
                        Debug.Log($"X:{x} Y:{y} noise : {noise}");

                    float landSize = 1 - waterLevel;
                    float landStep = landSize / 3;

                    Color color;
                    if (noise <= waterLevel)
                        color = Color.Lerp(Color.black, new Color(0f, 0.4f, 1f), noise * (1 / waterLevel));
                    else if (noise <= waterLevel + landStep)
                        color = Color.Lerp(Color.cyan, new Color(0, 0.8f, 0), (noise - waterLevel + 0.01f) * 4);
                    else if (noise <= waterLevel + landStep * 2)
                        color = Color.Lerp(new Color(0, 0.8f, 0), Color.yellow, (noise - (waterLevel + landStep - 0.05f)) * 4);
                    else
                        color = Color.Lerp(Color.yellow, Color.red, (noise - (waterLevel + landStep * 1.90f)) * 4);

                    texture.SetPixel(x, y, color);
                }
            }

            texture.Apply();

            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, width, height), new Vector2(0, 0));
            _spriteRendererHeight.sprite = sprite;
        }

        private void PaintTemperatureMap()
        {
            Texture2D texture = new(width, height);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float noise = WorldGenerator.GetTemperatureValue(x, y);

                    Color color;
                    if (noise <= 0.25f)
                        color = Color.Lerp(Color.white, Color.blue, noise * 4);
                    else if (noise <= 0.5f)
                        color = Color.Lerp(Color.blue, Color.green, (noise - 0.25f) * 4);
                    else if (noise <= 0.75f)
                        color = Color.Lerp(Color.green, Color.yellow, (noise - 0.5f) * 4);
                    else
                        color = Color.Lerp(Color.yellow, Color.red, (noise - 0.75f) * 4);

                    texture.SetPixel(x, y, color);
                }
            }

            texture.Apply();

            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, width, height), new Vector2(0, 0));
            _spriteRendererTemperature.sprite = sprite;
        }

        private void PaintRiversMap()
        {
            Texture2D texture = new(width, height);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float noise = WorldGenerator.GetRiversValue(x, y);

                    Color color = new Color(noise, noise, noise);
                    texture.SetPixel(x, y, color);
                }
            }

            texture.Apply();

            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, width, height), new Vector2(0, 0));
            _spriteRendererRivers.sprite = sprite;
        }
    }
}

using UnityEngine;

namespace Assets.Scripts
{
    public class WorldMapsVisualizer : MonoBehaviour
    {
        [SerializeField] private WorldGenerator worldGenerator;

        [SerializeField] private SpriteRenderer spriteRendererProgress;
        [SerializeField] private SpriteRenderer spriteRendererPolution;
        [SerializeField] private SpriteRenderer spriteRendererHeight;
        [SerializeField] private SpriteRenderer spriteRendererTemperature;
        [SerializeField] private SpriteRenderer spriteRendererTesting;
        [SerializeField] private SpriteRenderer spriteRendererTesting1;

        [SerializeField] private bool updateMaps = true;
        [SerializeField] private bool updateProgress = false;
        [SerializeField] private bool updatePolution = false;
        [SerializeField] private bool updateHeight = false;
        [SerializeField] private bool updateTemperature = false;
        [SerializeField] private bool updateTesting = false;
        [SerializeField] private bool updateTesting1 = false;


        private int width => worldGenerator.WorldWidth;
        private int height => worldGenerator.WorldHeight;
        private float waterLevel => worldGenerator.WaterLevel;

        private void Update()
        {
            if(updateMaps)
            {
                if(updateProgress)
                    PaintProgressMap();

                if(updatePolution)
                    PaintPolutionMap();

                if(updateHeight)
                    PaintHeightMap();

                if(updateTemperature)
                    PaintTemperatureMap();

                if(updateTesting)
                    PaintTestingMap();

                if(updateTesting1)
                    PaintTesting1Map();
            }
        }

        private void PaintProgressMap()
        {
            Texture2D texture = new(width, height);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float noise = worldGenerator.GetProgressValue(x, y);

                    Color color = Color.Lerp(Color.white, Color.red, noise);
                    texture.SetPixel(x, y, color);
                }
            }

            texture.Apply();

            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, width, height), new Vector2(0, 0));
            spriteRendererProgress.sprite = sprite;
        }

        private void PaintPolutionMap()
        {
            Texture2D texture = new(width, height);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float noise = worldGenerator.GetPolutionValue(x, y);

                    Color color = Color.Lerp(Color.white, new Color(0, 0.7f, 0), noise);
                    texture.SetPixel(x, y, color);
                }
            }

            texture.Apply();

            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, width, height), new Vector2(0, 0));
            spriteRendererPolution.sprite = sprite;
        }

        private void PaintHeightMap()
        {
            Texture2D texture = new(width, height);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float noise = worldGenerator.GetHeightValue(x, y);


                    float landSize = 1 - waterLevel;
                    float landStep = landSize / 3;

                    Color color;
                    if (noise <= waterLevel)
                        color = Color.Lerp(Color.black, new Color(0f, 0.4f, 1f), noise * (1 / waterLevel));
                    else if (noise <= waterLevel + landStep)
                        color = Color.Lerp(Color.cyan, new Color(0,0.8f,0), (noise - waterLevel +0.01f) * 4);
                    else if (noise <= waterLevel + landStep * 2)
                        color = Color.Lerp(new Color(0, 0.8f, 0), Color.yellow, (noise - (waterLevel + landStep - 0.05f )) * 4);
                    else
                        color = Color.Lerp(Color.yellow, Color.red, (noise - (waterLevel + landStep * 1.90f)) * 4);

                    texture.SetPixel(x, y, color);
                }
            }

            texture.Apply();

            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, width, height), new Vector2(0, 0));
            spriteRendererHeight.sprite = sprite;
        }

        private void PaintTemperatureMap()
        {
            Texture2D texture = new(width, height);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float noise = worldGenerator.GetTemperatureValue(x, y);

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
            spriteRendererTemperature.sprite = sprite;
        }

        private void PaintTestingMap()
        {
            Texture2D texture = new(width, height);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float noise = worldGenerator.GetTestingValue(x, y);

                    Color color = new Color(noise, noise,noise);
                    texture.SetPixel(x, y, color);
                }
            }

            texture.Apply();

            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, width, height), new Vector2(0, 0));
            spriteRendererTesting.sprite = sprite;
        }

        private void PaintTesting1Map()
        {

        }

    }
}

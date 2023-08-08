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
        [SerializeField] private SpriteRenderer spriteRendererRivers;
        [SerializeField] private SpriteRenderer spriteRendererTesting;
        [SerializeField] private SpriteRenderer spriteRendererTesting1;
        [Space]
        [SerializeField] private SpriteRenderer spriteRendererCombined;
        [SerializeField] private int combinedMapCellSize;
        [Header("Update")]
        [SerializeField] private bool updateMaps = true;
        [SerializeField] private bool updateProgress = false;
        [SerializeField] private bool updatePolution = false;
        [SerializeField] private bool updateHeight = false;
        [SerializeField] private bool updateTemperature = false;
        [SerializeField] private bool updateRivers = false;
        [SerializeField] private bool updateTesting = false;
        [SerializeField] private bool updateTesting1 = false;

        [SerializeField] private bool updateCombined = false;


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

                if(updateRivers)
                    PaintRiversMap();

                if(updateTesting)
                    PaintTestingMap();

                if(updateTesting1)
                    PaintTesting1Map();

                if (updateCombined)
                {
                    PaintCombinedMap();
                    updateCombined = false;
                }
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

                    if (noise < 0 || noise > 1)
                        Debug.Log($"X:{x} Y:{y} noise : {noise}");

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

        private void PaintRiversMap()
        {
            Texture2D texture = new(width, height);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float noise = worldGenerator.GetRiversValue(x, y);

                    Color color = new Color(noise, noise, noise);
                    texture.SetPixel(x, y, color);
                }
            }

            texture.Apply();

            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, width, height), new Vector2(0, 0));
            spriteRendererRivers.sprite = sprite;
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

        private void PaintCombinedMap()
        {
            Texture2D texture = new(width * combinedMapCellSize, height* combinedMapCellSize);

            for (int x = 0; x < worldGenerator.WorldWidth; x++)
            {
                for (int y = 0; y < worldGenerator.WorldHeight; y++)
                {
                    float heightNoise = worldGenerator.GetHeightValue(x, y);
                    float riversNoise = worldGenerator.GetRiversValue(x, y);
                    float temperatureNoise = worldGenerator.GetTemperatureValue(x, y);
                    float progressNoise = worldGenerator.GetProgressValue(x,y);
                    float polutionNoise = worldGenerator.GetPolutionValue(x, y);

                    if(heightNoise > waterLevel)
                    {
                        SetOcean(texture, x, y);
                    }
                    else
                    {
                        if(riversNoise > 0.8f)
                            SetRiver(texture, x, y);
                        else
                            SetLand(texture, x, y);
                    }


                    //if(heightNoise > waterLevel)
                    //{
                    //    if(riversNoise > 0.8)
                    //    {
                    //        SetCell(texture, x, y, new Color(0.35f,0.35f,1));
                    //        continue;
                    //    }

                    //    if (heightNoise < waterLevel + ((1f - waterLevel) / 3f))
                    //    {

                    //        if(temperatureNoise > 0.85f)
                    //        {
                    //            SetCell(texture, x, y, Color.yellow);
                    //        }
                    //        else if (temperatureNoise > 0.3f)
                    //        {
                    //            SetCell(texture, x, y, Color.green);
                    //        }
                    //        else
                    //        {
                    //            SetCell(texture, x, y, Color.white);

                    //        }

                    //    }
                    //    else if (heightNoise < waterLevel + ((1f - waterLevel) / 3f) * 2)
                    //    {

                    //        if(temperatureNoise > 0.85f)
                    //        {
                    //            SetCell(texture, x, y, new Color (0.6f,0.4f,0.05f));
                    //        }
                    //        else if (temperatureNoise > 0.3f)
                    //        {
                    //            SetCell(texture, x, y, new Color(0, 0.8f, 0));
                    //        }
                    //        else
                    //        {
                    //            SetCell(texture, x, y, new Color(0.7f, 0.7f, 0.7f));

                    //        }

                    //    }
                    //    else
                    //    {
                    //        SetCell(texture, x,y, Color.white);
                    //    }
                    //}
                    //else
                    //{
                    //    if(heightNoise > waterLevel / 2f)
                    //    {
                    //        SetCell(texture, x, y, Color.blue);
                    //    }
                    //    else
                    //    {
                    //        SetCell(texture, x, y, new Color(0,0,0.7f));
                    //    }
                    //}

                }
            }

            texture.Apply();

            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, width * combinedMapCellSize, height * combinedMapCellSize), new Vector2(0, 0));
            spriteRendererCombined.sprite = sprite;

            #region ocean
            void SetOcean(Texture2D texture, int x, int y)
            {
                float heightNoise = worldGenerator.GetHeightValue(x, y);


                if (heightNoise < waterLevel / 3f)
                {
                    SetDeepOcean(texture, x, y);
                }
                else if (heightNoise > (waterLevel / 3f) * 2)
                {
                    SetMiddleOcean(texture, x, y);
                }
                else
                {
                    SetCoast(texture, x, y);
                }
            }
            void SetCoast(Texture2D texture, int x, int y)
            {

            }
            void SetMiddleOcean(Texture2D texture, int x, int y)
            {

            }
            void SetDeepOcean(Texture2D texture, int x, int y)
            {

            }
            #endregion

            void SetRiver(Texture2D texture, int x,int y)
            {

            }

            void SetLand(Texture2D texture, int x, int y)
            {

            }

            void SetCell(Texture2D texture, int x, int y, Color color)
            {
                for (int i = x * combinedMapCellSize; i < (x * combinedMapCellSize) + combinedMapCellSize; i++)
                {
                    for (int j = y * combinedMapCellSize; j < (y * combinedMapCellSize) + combinedMapCellSize; j++)
                    {
                        texture.SetPixel(i, j, color);
                    }
                }

                //texture.SetPixel(x * combinedMapCellSize, y * combinedMapCellSize, Color.black);
            }

            void SetCombinedCell(Texture2D texture, int x, int y, Color color, Color additionalColor, float frequency)
            {
                for (int i = x * combinedMapCellSize; i < (x * combinedMapCellSize) + combinedMapCellSize; i++)
                {
                    for (int j = y * combinedMapCellSize; j < (y * combinedMapCellSize) + combinedMapCellSize; j++)
                    {
                        if(Random.Range(0, 1) < frequency)
                        {
                            texture.SetPixel(i, j, additionalColor);

                        }
                        else
                        {
                            texture.SetPixel(i, j, color);
                        }
                    }
                }

                //texture.SetPixel(x * combinedMapCellSize, y * combinedMapCellSize, Color.black);
            }
        }

    }
}

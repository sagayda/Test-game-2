using UnityEngine;

public class NoiseTest : MonoBehaviour
{
    [SerializeField] int X;
    [SerializeField] int Y;
    [SerializeField] float ZoomOne;
    [SerializeField] float ZoomTwo;
    [SerializeField] int Seed;
    [SerializeField] SpriteRenderer SpriteRenderer;
    [SerializeField] int Variant;
    [SerializeField] int Step = 1000;
    [SerializeField] float ZoomStep = 0.4f;
    [SerializeField] float Amplitude = 5f;
    [SerializeField] float LerpCoeff = 0.5f;
    [SerializeField] float PoPImpactStrength = 1f;
    [SerializeField] float PoPImpactMultiplyer = 1f;
    [SerializeField] float PolutionDensity = 1.0f;
    [SerializeField] float PoPImpactButtom = 0.1f;
    [SerializeField] float PoPImpactTop = 2f;

    [Header("Heat and temperature maps")]
    [SerializeField] float HoTImpactStrength = 30f;
    [SerializeField] float HoTImpactSmoothing = 3.2f;
    [SerializeField][Range(0, 5)] float NoiseOnTemperatureImpact = 0.5f;
    [SerializeField][Range(0, 1)] float WaterLevel = 0.35f;
    [SerializeField][Range(-1,1)] float Temperature = 0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        switch (Variant)
        {
            case 1:
                VarianOne();
                break;
            case 2:
                VariantTwo();
                break;
            case 3:
                VariantThree();
                break;
            case 4:
                VariantFour();
                break;
            case 5:
                VariantFive();
                break;
            case 6:
                VariantSix();
                break;
            case 7:
                VariantSeven();
                break;
            case 8:
                VariantEight();
                break;
            case 9:
                VariantNine();
                break;
            case 10:
                VariantTen();
                break;
            case 11:
                VariantEleven();
                break;
            case 12:
                VariantTwelve();
                break;
            default:
                break;
        }
    }

    private void VarianOne()
    {
        Texture2D texture = new(X, Y);

        for (int i = 0; i < X; i++)
        {
            for (int j = 0; j < Y; j++)
            {
                float noise = Mathf.PerlinNoise(((i) + Seed) / ZoomOne, ((j) + Seed) / ZoomOne);

                texture.SetPixel(i, j, new Color(noise, noise, noise, 1));
            }
        }

        texture.Apply();

        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, X, Y), new Vector2(0, 0));
        SpriteRenderer.sprite = sprite;
    }

    private void VariantTwo()
    {
        Texture2D texture = new(X, Y);

        int step = 1000;

        for (int i = 0; i < X; i++)
        {
            for (int j = 0; j < Y; j++)
            {
                float noise = Mathf.PerlinNoise(((i) + Seed) / ZoomOne, ((j) + Seed) / ZoomOne);
                noise = Mathf.PerlinNoise(((i) + Seed + step) / ZoomOne, ((j) + Seed + step) / ZoomOne);

                noise /= 2f;

                texture.SetPixel(i, j, new Color(noise, noise, noise, 1));
            }
        }

        texture.Apply();

        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, X, Y), new Vector2(0, 0));
        SpriteRenderer.sprite = sprite;
    }

    private void VariantThree()
    {
        Texture2D texture = new(X, Y);

        for (int i = 0; i < X; i++)
        {
            for (int j = 0; j < Y; j++)
            {
                float noise = Mathf.PerlinNoise(((i) + Seed) / ZoomOne, ((j) + Seed) / ZoomOne);

                if (noise >= 0.4 && noise <= 0.5)
                    texture.SetPixel(i, j, new Color(1, 1, 1, 1));
                else
                    texture.SetPixel(i, j, new Color(0, 0, 0, 1));
            }
        }

        texture.Apply();

        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, X, Y), new Vector2(0, 0));
        SpriteRenderer.sprite = sprite;
    }

    private void VariantFour()
    {
        Texture2D texture = new(X, Y);

        int step = 1000;

        for (int i = 0; i < X; i++)
        {
            for (int j = 0; j < Y; j++)
            {
                float noise = Mathf.PerlinNoise(((i) + Seed) / ZoomOne, ((j) + Seed) / ZoomOne);
                float noise1 = Mathf.PerlinNoise(((i) + Seed + step) / ZoomOne, ((j) + Seed + step) / ZoomOne);
                float noise2 = Mathf.PerlinNoise(((i) + Seed + step + step) / ZoomOne, ((j) + Seed + step + step) / ZoomOne);

                texture.SetPixel(i, j, new Color(noise, noise1, noise2, 1));
            }
        }

        texture.Apply();

        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, X, Y), new Vector2(0, 0));
        SpriteRenderer.sprite = sprite;
    }

    private void VariantFive()
    {
        Texture2D texture = new(X, Y);

        int step = 1000;

        for (int i = 0; i < X; i++)
        {
            for (int j = 0; j < Y; j++)
            {
                float noise = Mathf.PerlinNoise(((i) + Seed) / ZoomOne, ((j) + Seed) / ZoomOne);
                float noise1 = Mathf.PerlinNoise(((i) + Seed + step) / ZoomOne, ((j) + Seed + step) / ZoomOne);
                float noise2 = Mathf.PerlinNoise(((i) + Seed + step + step) / ZoomOne, ((j) + Seed + step + step) / ZoomOne);

                float resultNoise = noise + noise1 + noise2;

                if (resultNoise >= 0 && resultNoise <= 0.7)
                    texture.SetPixel(i, j, Color.black);
                else if (resultNoise > 0.7 && resultNoise <= 1.5)
                    texture.SetPixel(i, j, Color.blue);
                else if (resultNoise > 1.5 && resultNoise <= 2)
                    texture.SetPixel(i, j, Color.green);
                else if (resultNoise > 2 && resultNoise <= 2.7)
                    texture.SetPixel(i, j, Color.yellow);
                else
                    texture.SetPixel(i, j, Color.white);
            }
        }

        texture.Apply();

        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, X, Y), new Vector2(0, 0));
        SpriteRenderer.sprite = sprite;
    }

    private void VariantSix()
    {
        Texture2D texture = new(X, Y);

        int step = 1000;

        for (int i = 0; i < X; i++)
        {
            for (int j = 0; j < Y; j++)
            {
                float noise = Mathf.PerlinNoise(((i) + Seed) / ZoomOne, ((j) + Seed) / ZoomOne);

                noise += Mathf.PerlinNoise(((i) + Seed) / (ZoomOne * ZoomStep), ((j) + Seed) / (ZoomOne * ZoomStep)) / Amplitude;

                noise /= 1f + (1f / Amplitude);

                noise += Mathf.PerlinNoise(((i) + Seed) / (ZoomOne * ZoomStep * ZoomStep), ((j) + Seed) / (ZoomOne * ZoomStep * ZoomStep)) / (Amplitude * Amplitude);

                noise /= 1f + (1f / (Amplitude * Amplitude));

                texture.SetPixel(i, j, new Color(noise, noise, noise, 1));
            }
        }

        texture.Apply();

        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, X, Y), new Vector2(0, 0));
        SpriteRenderer.sprite = sprite;

    }

    //Polution on progress
    private void VariantSeven()
    {
        Texture2D texture = new(X, Y);

        for (int i = 0; i < X; i++)
        {
            for (int j = 0; j < Y; j++)
            {
                float noiseProgress = Mathf.PerlinNoise((i + Seed) / ZoomOne, (j + Seed) / ZoomOne);

                float noisePolution = Mathf.PerlinNoise((i + Seed + Step) / ZoomOne, (j + Seed + Step) / ZoomOne);
                
                noisePolution *= PolutionDensity;

                //чем выше прогресс - тем сильнее повышается загрязнение
                float polutionMultiplyer = Mathf.Pow(noiseProgress * PoPImpactMultiplyer, PoPImpactStrength);
                polutionMultiplyer = Mathf.Clamp(polutionMultiplyer, PoPImpactButtom, PoPImpactTop);
                noisePolution *= polutionMultiplyer;

                Color color = Color.Lerp(Color.white, new Color(0,0.7f,0), noisePolution);

                texture.SetPixel(i, j, color);
            }
        }

        texture.Apply();

        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, X, Y), new Vector2(0, 0));
        SpriteRenderer.sprite = sprite;
    }

    //Progress
    private void VariantEight()
    {
        Texture2D texture = new(X, Y);

        for (int i = 0; i < X; i++)
        {
            for (int j = 0; j < Y; j++)
            {
                float noise = Mathf.PerlinNoise(((i) + Seed) / ZoomOne, ((j) + Seed) / ZoomOne);

                Color color = Color.Lerp(Color.white, Color.red, noise);

                texture.SetPixel(i, j, color);
            }
        }

        texture.Apply();

        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, X, Y), new Vector2(0, 0));
        SpriteRenderer.sprite = sprite;
    }

    //Polution
    private void VariantNine()
    {
        Texture2D texture = new(X, Y);

        for (int i = 0; i < X; i++)
        {
            for (int j = 0; j < Y; j++)
            {
                float noise = Mathf.PerlinNoise(((i) + Seed + Step) / ZoomOne, ((j) + Seed + Step) / ZoomOne);
                noise *= PolutionDensity;

                Color color = Color.Lerp(Color.white, new Color(0,0.7f,0), noise);

                texture.SetPixel(i, j, color);
            }
        }

        texture.Apply();

        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, X, Y), new Vector2(0, 0));
        SpriteRenderer.sprite = sprite;
    }

    //Hight
    private void VariantTen()
    {
        Texture2D texture = new(X, Y);

        for (int i = 0; i < X; i++)
        {
            for (int j = 0; j < Y; j++)
            {
                float noise = Mathf.PerlinNoise(((i) + Seed) / ZoomOne, ((j) + Seed) / ZoomOne);

                //Coloring
                float landSize = 1 - WaterLevel;
                float landStep = landSize / 3;

                Color color;
                if (noise <= WaterLevel)
                    color = Color.Lerp(Color.black, new Color(0f, 0.4f, 1f), noise * (1 / WaterLevel));
                else if (noise <= WaterLevel + landStep)
                    color = Color.Lerp(Color.cyan, Color.green, (noise - WaterLevel) * 4);
                else if (noise <= WaterLevel + landStep * 2)
                    color = Color.Lerp(Color.green, Color.yellow, (noise - (WaterLevel + landStep)) * 4);
                else
                    color = Color.Lerp(Color.yellow, Color.red, (noise - (WaterLevel + landStep * 2)) * 4);


                texture.SetPixel(i, j, color);
            }
        }

        texture.Apply();

        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, X, Y), new Vector2(0, 0));
        SpriteRenderer.sprite = sprite;
    }

    //Temperature
    private void VariantEleven()
    {
        Texture2D texture = new(X, Y);

        for (int i = 0; i < X; i++)
        {
            for (int j = 0; j < Y; j++)
            {
                float noise = Mathf.PerlinNoise((i + Seed + Step) / ZoomTwo, (j + Seed + Step) / ZoomTwo);

                // 0..1 => -0.5..0.5
                noise -= 0.5f;

                //distance from equator 0..1
                float distanceFromEquator = Mathf.Abs((Y / 2) - j);
                distanceFromEquator /= (Y / 2);

                float temperature = distanceFromEquator;
                temperature += Temperature;
                temperature += noise * NoiseOnTemperatureImpact;
                //reverce temperature
                temperature = 1 - temperature;

                //Coloring
                Color color;
                if (temperature <= 0.25f)
                    color = Color.Lerp(Color.white, Color.blue, temperature * 4);
                else if (temperature <= 0.5f)
                    color = Color.Lerp(Color.blue, Color.green, (temperature - 0.25f) * 4);
                else if (temperature <= 0.75f)
                    color = Color.Lerp(Color.green, Color.yellow, (temperature - 0.5f) * 4);
                else
                    color = Color.Lerp(Color.yellow, Color.red, (temperature - 0.75f) * 4);

                texture.SetPixel(i, j, color);
            }
        }

        texture.Apply();

        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, X, Y), new Vector2(0, 0));
        SpriteRenderer.sprite = sprite;
    }

    //Temperature on hight
    private void VariantTwelve()
    {
        Texture2D texture = new(X, Y);

        for (int i = 0; i < X; i++)
        {
            for (int j = 0; j < Y; j++)
            {
                float noiseHight = Mathf.PerlinNoise(((i) + Seed) / ZoomOne, ((j) + Seed) / ZoomOne);

                float noiseTemperature = Mathf.PerlinNoise(((i) + Seed + Step) / ZoomTwo, ((j) + Seed + Step) / ZoomTwo);
                noiseTemperature -= 0.5f;

                float distanceFromEquator = Mathf.Abs((Y / 2) - j);
                distanceFromEquator /= (Y / 2);

                float temperature = distanceFromEquator;
                temperature += Temperature;
                temperature += noiseTemperature * NoiseOnTemperatureImpact;

                temperature = 1 - temperature;


                if (noiseHight > WaterLevel)
                {
                    float temperatureChange;

                    //temperature change depending on distance from equator
                    temperatureChange = noiseHight - WaterLevel;
                    //smoothing
                    temperatureChange = Mathf.Pow(temperatureChange, HoTImpactSmoothing);
                    temperatureChange /= (1 - WaterLevel);
                    //temperature 0..1 => 1..2 for better smoothing
                    temperature += 1;
                    //calculating temperature change strange
                    temperatureChange = Mathf.Pow(HoTImpactStrength, temperatureChange); 

                    temperature -= temperatureChange;
                }

                //coloring
                Color color;
                if (temperature <= 0.25f)
                    color = Color.Lerp(Color.white, Color.blue, temperature * 4);
                else if (temperature <= 0.5f)
                    color = Color.Lerp(Color.blue, Color.green, (temperature - 0.25f) * 4);
                else if (temperature <= 0.75f)
                    color = Color.Lerp(Color.green, Color.yellow, (temperature - 0.5f) * 4);
                else
                    color = Color.Lerp(Color.yellow, Color.red, (temperature - 0.75f) * 4);


                texture.SetPixel(i, j, color);
            }
        }

        texture.Apply();

        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, X, Y), new Vector2(0, 0));
        SpriteRenderer.sprite = sprite;
    }
}

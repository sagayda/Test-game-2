using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseTest : MonoBehaviour
{
    [SerializeField] int X;
    [SerializeField] int Y;
    [SerializeField] float Zoom;
    [SerializeField] int Seed;
    [SerializeField] SpriteRenderer SpriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Texture2D texture = new(X, Y);

        for (int i = 0; i < X; i++)
        {
            for (int j = 0; j < Y; j++)
            {
                float noise = Mathf.PerlinNoise(((i) + Seed) / Zoom, ((j) + Seed) / Zoom);

                texture.SetPixel(i, j, new Color(noise, noise, noise, 1));
            }
        }

        texture.Apply();

        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, X, Y), new Vector2(0, 0));
        SpriteRenderer.sprite = sprite;
    }
}

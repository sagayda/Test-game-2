using UnityEngine;

namespace Assets.Scripts.View
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class MapSprite : MonoBehaviour
    {
        public int ScaleLevel { get; private set; }
        public SpriteRenderer SpriteRenderer { get; private set; }
        public Texture2D Texture => SpriteRenderer.sprite.texture;

        public void Init(int scaleLevel, Texture2D texture)
        {
            ScaleLevel = scaleLevel;

            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            SpriteRenderer = GetComponent<SpriteRenderer>();
            SpriteRenderer.sprite = sprite;
            SpriteRenderer.flipY = true;
        }

        public void Enable()
        {
            gameObject.SetActive(true);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}

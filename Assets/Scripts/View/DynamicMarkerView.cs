using UnityEngine;

namespace Assets.Scripts.View
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class DynamicMarkerView : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        public void Init(Sprite sprite, Vector2 position)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.sprite = sprite;
            gameObject.transform.position = position;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map.View
{
    public class MapSpritesWrapper : IEnumerable<MapSprite>
    {
        private readonly MapSprite[] _sprites;
        private readonly Transform _parent;

        public MapSpritesWrapper(Transform parent, int maxScaleLevel)
        {
            _sprites = new MapSprite[maxScaleLevel];
            _parent = parent;
        }

        public MapSprite this[int scaleLevel]
        {
            get => _sprites[scaleLevel - 1];
        }

        public void AddSprite(Texture2D texture, int scaleLevel)
        {
            if (scaleLevel <= 0 || scaleLevel > _sprites.Length)
                throw new ArgumentOutOfRangeException("Scale level must be greater than zero and not greater than the maximum value");

            int i = scaleLevel - 1;

            GameObject gameObject = new();
            gameObject.transform.SetParent(_parent);
            gameObject.name = $"[{texture.width}x{texture.height}] Scale: {scaleLevel}";

            _sprites[i] = gameObject.AddComponent<MapSprite>();
            _sprites[i].Init(scaleLevel, texture);

            int largerEdge = _sprites[i].Texture.width >= _sprites[i].Texture.height ? _sprites[i].Texture.width : _sprites[i].Texture.height;
            _sprites[i].gameObject.transform.localScale = new Vector2(100f / largerEdge * 100f, 100f / largerEdge * 100f);
        }

        public IEnumerator<MapSprite> GetEnumerator()
        {
            for (int i = 0; i < _sprites.Length; i++)
            {
                yield return _sprites[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

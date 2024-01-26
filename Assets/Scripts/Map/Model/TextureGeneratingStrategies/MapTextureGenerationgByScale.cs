using System;
using System.Collections.Generic;
using System.Linq;
using Map.View;
using UnityEngine;
using WorldGeneration.Core.Locations;

namespace Map.Model
{
    public class MapTextureGenerationgByScale : IMapTextureGeneratingStrategy
    {
        private const int CELL_SIZE = 16;

        private readonly GameWorld _world;
        private readonly float _resolutionRatio;
        private readonly int _maxScaleLevel;

        public MapTextureGenerationgByScale(GameWorld world, float resolutionRatio, int maxScaleLevel)
        {
            _world = world;
            _resolutionRatio = resolutionRatio;
            _maxScaleLevel = maxScaleLevel;
        }

        public MapSpritesWrapper GenerateAndWrapMapTextures(Transform parent)
        {
            MapSpritesWrapper spritesWrapper = new(parent, _maxScaleLevel);

            for (int scaleLevel = 1; scaleLevel <= _maxScaleLevel; scaleLevel++)
            {
                spritesWrapper.AddSprite(GenerateTexture(scaleLevel), scaleLevel);
            }

            return spritesWrapper;
        }

        public Texture2D GenerateTexture(int scaleLevel)
        {
            if (scaleLevel <= 0)
                throw new ArgumentOutOfRangeException(nameof(scaleLevel), "Scale level must be greater than zero!");

            if (scaleLevel > _maxScaleLevel)
                throw new ArgumentOutOfRangeException(nameof(scaleLevel), "Scale level is too large!");

            float ratio = scaleLevel * _resolutionRatio;
            Vector2Int size = new()
            {
                x = Convert.ToInt32(_world.Width * ratio),
                y = Convert.ToInt32(_world.Height * ratio),
            };

            Texture2D texture = new(size.x * CELL_SIZE, size.y * CELL_SIZE);

            float horizontalRatio = _world.Width / (float)size.x;
            float verticalRatio = _world.Height / (float)size.y;

            for (int i = 0; i < size.x; i++)
            {
                for (int j = 0; j < size.y; j++)
                {
                    RectInt rect = new()
                    {
                        xMin = Mathf.RoundToInt(i * horizontalRatio),
                        yMin = Mathf.RoundToInt(j * verticalRatio),
                        xMax = Mathf.RoundToInt((i + 1) * horizontalRatio) - 1,
                        yMax = Mathf.RoundToInt((j + 1) * verticalRatio) - 1,
                    };

                    Color prevailingColor = FindPrevailingLocationColorInRect(rect);

                    //это что?
                    if (i == size.y - 1)
                    {
                        PaintLocationCell(texture, i, j, Color.black);
                    }

                    PaintLocationCell(texture, i, j, prevailingColor);
                }
            }

            texture.filterMode = FilterMode.Point;
            texture.Apply();

            return texture;
        }

        private Color FindPrevailingLocationColorInRect(RectInt rect)
        {
            Dictionary<Color, float> colorFrequency = new Dictionary<Color, float>();

            for (int i = rect.xMin; i <= rect.xMax; i++)
            {
                for (int j = rect.yMin; j <= rect.yMax; j++)
                {
                    Color locationColor = _world.World[i, j] == null ? Color.white : _world.World[i, j].Color;

                    float increment = 1f;

                    if (_world.World[i, j].Id == 3)
                        increment = 3.5f;

                    if (colorFrequency.ContainsKey(locationColor))
                    {
                        colorFrequency[locationColor] += increment;
                    }
                    else
                    {
                        colorFrequency[locationColor] = increment;
                    }
                }

            }

            Color mostFrequentColor = colorFrequency.OrderByDescending(keyValuePair => keyValuePair.Value).First().Key;

            return mostFrequentColor;
        }

        private void PaintLocationCell(Texture2D texture, int x, int y, Color color)
        {
            for (int i = x * CELL_SIZE; i < (x * CELL_SIZE) + CELL_SIZE; i++)
            {
                for (int j = y * CELL_SIZE; j < (y * CELL_SIZE) + CELL_SIZE; j++)
                {
                    texture.SetPixel(i, j, color);
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using Assets.Scripts.InGameScripts;
using UnityEngine;

namespace Assets.Scripts.Model
{
    public class MapScaling
    {
        public Action<int> ScaleLevelChanged;
        public Action<Vector2> GridCellSizeChanged;
        public Action<float> FovChanged;

        private readonly GameWorld _world;

        private const float RESOLUTION_RATIO = 0.25f;
        private const int CELL_SIZE = 16;
        private const float ZOOM_SPEED = 250f;

        private readonly int _maxScaleLevel;
        private readonly float _maxFov;
        private readonly float _minFov;

        private int _scaleLevel;
        private float _zoomPercentage;
        private Vector3 _gridCellSize;

        public MapScaling(GameWorld world)
        {
            _world = world;

            int largestEdge = _world.Width > _world.Height ? _world.Width : _world.Height;
            _minFov = 200f / largestEdge;
            _maxFov = _world.Width > _world.Height ? 34f : 61f;
            _zoomPercentage = 0f;

            _maxScaleLevel = Mathf.CeilToInt(1f / RESOLUTION_RATIO);
            _scaleLevel = 1;
            _gridCellSize = CalculateGridCellSizeByLevel(_scaleLevel);
        }

        public float Fov => (_maxFov - _minFov) * (1f - _zoomPercentage) + _minFov;
        public int MaxScaleLevel => _maxScaleLevel;
        public float ZoomPercentage => _zoomPercentage;

        public Texture2D CreateMapTexture(int scaleLevel)
        {
            if (scaleLevel <= 0 || scaleLevel > _maxScaleLevel)
                throw new ArgumentException("Scale level must be greater than zero and not greater than the maximum value");

            Vector2Int size = CalculateMapSizeByLevel(scaleLevel);

            Texture2D texture = new(size.x * CELL_SIZE, size.y * CELL_SIZE);

            float horizontalRatio = _world.Width / (float)size.x;
            float verticalRatio = _world.Height / (float)size.y;

            for (int i = 0; i < size.x; i++)
            {
                for (int j = 0; j < size.y; j++)
                {
                    int xStart = Mathf.RoundToInt(i * horizontalRatio);
                    int yStart = Mathf.RoundToInt(j * verticalRatio);

                    int xFinish = Mathf.RoundToInt((i + 1) * horizontalRatio) - 1;
                    int yFinish = Mathf.RoundToInt((j + 1) * verticalRatio) - 1;

                    Color color = GetColorInSquare(xStart, yStart, xFinish, yFinish);

                    if (i == size.y - 1)
                    {
                        SetCell(texture, i, j, Color.black);
                    }

                    SetCell(texture, i, j, color);
                }
            }

            texture.filterMode = FilterMode.Point;
            texture.Apply();

            return texture;

            Color GetColorInSquare(int xStart, int yStart, int xFinish, int yFinish)
            {
                Dictionary<Color, float> colorCounts = new Dictionary<Color, float>();

                for (int i = xStart; i <= xFinish; i++)
                {
                    for (int j = yStart; j <= yFinish; j++)
                    {
                        Color locationColor = _world.World[i, j] == null ? Color.white : _world.World[i, j].Color;

                        float increment = 1f;

                        if (_world.World[i, j].Id == 3)
                            increment = 3.5f;

                        if (colorCounts.ContainsKey(locationColor))
                        {
                            colorCounts[locationColor] += increment;
                        }
                        else
                        {
                            colorCounts[locationColor] = increment;
                        }
                    }

                }

                Color resultColor = Color.black;
                float maxCount = 0;

                foreach (var pair in colorCounts)
                {
                    if (pair.Value > maxCount)
                    {
                        resultColor = pair.Key;
                        maxCount = pair.Value;
                    }
                }

                return resultColor;

            }

            void SetCell(Texture2D texture, int x, int y, Color color)
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

        public void Zoom(float zoom)
        {
            _zoomPercentage += zoom * ZOOM_SPEED * Time.deltaTime;
            _zoomPercentage = Mathf.Clamp01(_zoomPercentage);
            FovChanged?.Invoke(Fov);
            CheckScale();
        }

        private void CheckScale()
        {
            float zoomStepPerLevel = 1f / _maxScaleLevel;

            float upperBound = (_scaleLevel) * zoomStepPerLevel;
            float lowerBound = (_scaleLevel - 1) * zoomStepPerLevel;

            if (_zoomPercentage > upperBound && _scaleLevel < _maxScaleLevel)
            {
                IncreaseScale();
                return;
            }

            if (_zoomPercentage <= lowerBound && _scaleLevel > 1)
            {
                DecreaseScale();
                return;
            }
        }

        private void IncreaseScale()
        {
            if (_scaleLevel >= _maxScaleLevel)
                return;

            _scaleLevel++;

            _gridCellSize = CalculateGridCellSizeByLevel(_scaleLevel);

            ScaleLevelChanged?.Invoke(_scaleLevel);
            GridCellSizeChanged?.Invoke(_gridCellSize);
        }

        private void DecreaseScale()
        {
            if (_scaleLevel <= 1)
                return;

            _scaleLevel--;

            _gridCellSize = CalculateGridCellSizeByLevel(_scaleLevel);

            ScaleLevelChanged?.Invoke(_scaleLevel);
            GridCellSizeChanged?.Invoke(_gridCellSize);
        }

        private Vector2Int CalculateMapSizeByLevel(int scaleLevel)
        {
            if (scaleLevel <= 0 || scaleLevel > _maxScaleLevel)
                throw new ArgumentException("Scale level must be greater than zero and not greater than the maximum value");

            float ratio = scaleLevel * RESOLUTION_RATIO;

            Vector2Int size = new()
            {
                x = Convert.ToInt32(_world.Width * ratio),
                y = Convert.ToInt32(_world.Height * ratio),
            };

            return size;
        }

        private Vector3 CalculateGridCellSizeByLevel(int scaleLevel)
        {
            Vector2Int size = CalculateMapSizeByLevel(scaleLevel);
            int largerEdge = size.x > size.y ? size.x : size.y;

            Vector3 cellSize = new()
            {
                x = 100f / largerEdge,
                y = 100f / largerEdge,
                z = 1,
            };

            return cellSize;
        }

        public void RefreshViev()
        {
            ScaleLevelChanged?.Invoke(_scaleLevel);
            GridCellSizeChanged?.Invoke(_gridCellSize);
            FovChanged?.Invoke(Fov);
        }
    }
}

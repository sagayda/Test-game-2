using System;
using System.Collections.Generic;
using Assets.Scripts.InGameScripts;
using UnityEngine;

namespace Assets.Scripts.Model
{
    public class MapScaling
    {
        public event Action<int> ScaleLevelChanged;
        public event Action<Vector2> GridCellSizeChanged;
        public event Action<float> FovChanged;

        private readonly GameWorld _world;

        private const float RESOLUTION_RATIO = 0.25f;
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
        public float ResolutionRatio => RESOLUTION_RATIO;

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

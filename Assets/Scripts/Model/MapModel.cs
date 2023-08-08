using System;
using Assets.Scripts.InGameScripts;
using UnityEngine;

namespace Assets.Scripts.Model
{
    public class MapModel
    {
        public Action<Vector2> PositionChanged;

        private const float MOVE_SPEED = 50f;

        private readonly GameWorld _world;
        private readonly MapScaling _scaling;

        private float _maxXDelta;
        private float _maxYDelta;

        private Vector2 _cameraPosition;

        public MapModel(GameWorld world)
        {
            _world = world ?? throw new ArgumentException("World can't be null!", nameof(world));

            _scaling = new MapScaling(world);
        }

        public MapScaling Scaling => _scaling;

        public void MoveInDirection(Vector2 direction)
        {
            _cameraPosition += MOVE_SPEED * Time.deltaTime * direction;

            PositionChanged.Invoke(_cameraPosition);
        }

        public void TranslateToPosition(Vector2 position)
        {
            _cameraPosition = position;

            PositionChanged.Invoke(_cameraPosition);
        }

        public void RefreshViev()
        {
            PositionChanged?.Invoke(_cameraPosition);
        }
    }
}

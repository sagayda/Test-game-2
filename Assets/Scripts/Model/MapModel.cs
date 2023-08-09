using System;
using Assets.Scripts.InGameScripts;
using UnityEngine;

namespace Assets.Scripts.Model
{
    public class MapModel
    {
        public event Action<Vector2> PositionChanged;

        private const float MOVE_SPEED = 50f;

        private readonly GameWorld _world;
        private readonly MapScaling _scaling;

        private IMapTextureGeneratingStrategy _textureGeneratingStrategy;

        private readonly float _deltaX = 50f;
        private readonly float _deltaY = 50f;

        private float _scaledDeltaX => _deltaX * _scaling.ZoomPercentage;
        private float _scaledDeltaY => _deltaY * _scaling.ZoomPercentage;

        private Vector2 _cameraPosition;

        public MapModel(GameWorld world)
        {
            _world = world ?? throw new ArgumentException("World can't be null!", nameof(world));

            _scaling = new MapScaling(_world);

            _textureGeneratingStrategy = new MapTextureGenerationgByScale(_world, _scaling.ResolutionRatio, _scaling.MaxScaleLevel);

            if(_world.Width == _world.Height)
            {
                return;
            }
            else if(_world.Width > _world.Height)
            {
                float ratio = _world.Height / (float)_world.Width;
                _deltaY *= ratio;
            }
            else
            {
                float ratio = _world.Width / (float)_world.Height;
                _deltaX *= ratio;
            }
        }

        public MapScaling Scaling => _scaling;
        public IMapTextureGeneratingStrategy MapPainting => _textureGeneratingStrategy;

        public void MoveInDirection(Vector2 direction)
        {
            Vector2 newPosition = _cameraPosition + MOVE_SPEED * Time.deltaTime * direction;

            newPosition.x = Mathf.Clamp(newPosition.x, -_scaledDeltaX, _scaledDeltaX);
            newPosition.y = Mathf.Clamp(newPosition.y, -_scaledDeltaY, _scaledDeltaY);

            _cameraPosition = newPosition;

            PositionChanged.Invoke(_cameraPosition);
        }

        public void TranslateToPosition(Vector2 position)
        {
            position.x = Mathf.Clamp(position.x, -_scaledDeltaX, _scaledDeltaX);
            position.y = Mathf.Clamp(position.y, -_scaledDeltaY, _scaledDeltaY);

            _cameraPosition = position;

            PositionChanged.Invoke(_cameraPosition);
        }

        public void RefreshViev()
        {
            PositionChanged?.Invoke(_cameraPosition);
        }
    }
}

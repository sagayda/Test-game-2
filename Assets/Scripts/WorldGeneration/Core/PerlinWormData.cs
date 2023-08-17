using System;
using System.Collections.Generic;
using Assets.Scripts.WorldGeneration.Core;
using UnityEngine;

namespace WorldGeneration.Core
{
    public class PerlinWormData
    {
        private readonly Vector2 _startPoint;
        private readonly float _minThickness;
        private readonly float _maxThickness;
        private readonly uint _length;
        protected readonly IThickeningStrategy _thickening;

        private readonly List<WormSegment> _worm = new();

        public PerlinWormData(Vector2 start)
        {
            _startPoint = start;
            _minThickness = 1f;
            _maxThickness = 2f;
            _length = 256;
            _thickening = new LinearThicken();

            Direction = Vector2.up;
            Position = start;
        }

        public PerlinWormData(Vector2 start, float minThickness, float maxThickness)
        {
            if (minThickness > maxThickness)
                throw new ArgumentException("Thickness is invalid");

            if (minThickness < 0f)
                minThickness = 0f;

            if (maxThickness < 0f)
                maxThickness = 0f;

            _startPoint = start;
            _minThickness = minThickness;
            _maxThickness = maxThickness;
            _length = 256;
            _thickening = new LinearThicken();

            Direction = Vector2.up;
            Position = start;
        }

        public PerlinWormData(Vector2 start, float minThickness, float maxThickness, uint length)
        {
            if (minThickness > maxThickness)
                throw new ArgumentException("Thickness is invalid");

            if (minThickness < 0f)
                minThickness = 0f;

            if (maxThickness < 0f)
                maxThickness = 0f;

            _startPoint = start;
            _minThickness = minThickness;
            _maxThickness = maxThickness;
            _length = length;
            _thickening = new LinearThicken();

            Direction = Vector2.up;
            Position = start;
        }

        public PerlinWormData(Vector2 start, float minThickness, float maxThickness, uint length, IThickeningStrategy thickeningStrategy)
        {
            if (minThickness > maxThickness)
                throw new ArgumentException("Thickness is invalid");

            if (minThickness < 0f)
                minThickness = 0f;

            if (maxThickness < 0f)
                maxThickness = 0f;

            _startPoint = start;
            _minThickness = minThickness;
            _maxThickness = maxThickness;
            _length = length;
            _thickening = thickeningStrategy;

            Position = start;
            Direction = Vector2.up;
        }

        public Vector2 Direction { get; private set; }
        public Vector2 Position { get; private set; }

        public Vector2 StartPoint => _startPoint;
        public float MinThickness => _minThickness;
        public float MaxThickness => _maxThickness;
        public float Length => _length;
        public List<WormSegment> Worm => _worm;

        protected virtual float CurrentThickness => _thickening.Thicken(_maxThickness, _minThickness, _worm.Count / (float)_length);

        public virtual bool Step()
        {
            if (_worm.Count >= _length)
            {
                return false;
            }

            _worm.Add(new(Position, Direction, CurrentThickness));

            Position += Direction;

            return true;
        }

        public void Direct(Vector2 direction)
        {
            if (direction.magnitude != 1)
                direction.Normalize();

            float angleBetween = Vector2.Angle(Direction, direction);

            float maxAngle = 45f;

            if (angleBetween > maxAngle)
            {
                // Найти направление поворота (по часовой или против часовой стрелки)
                float crossProduct = Vector3.Cross(Direction, direction).z;
                int rotationDirection = crossProduct > 0 ? 1 : -1;

                // Поворот нового направления на 45 градусов
                Quaternion rotation = Quaternion.Euler(0f, 0f, rotationDirection * maxAngle);
                direction = rotation * Direction;
            }

            Direction = direction;
        }

        public void SetDirection(Vector2 direction)
        {
            if (direction.magnitude != 1)
                direction.Normalize();

            Direction = direction;
        }
    }

    public readonly struct WormSegment
    {
        public Vector2 Position { get; }
        public Vector2 Direction { get; }
        public float Thickness { get; }

        public WormSegment(Vector2 position, Vector2 direction, float thickness)
        {
            Position = position;
            Direction = direction;
            Thickness = thickness;
        }
    }
}

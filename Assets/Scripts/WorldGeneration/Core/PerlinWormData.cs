using System.Collections.Generic;
using UnityEngine;
using UniversalTools;

namespace WorldGeneration.Core
{
    public class PerlinWormData
    {
        private readonly Vector2 _startPoint;
        private readonly uint _length;
        private readonly List<WormSegment> _worm = new();

        private BoundedValue<float> _thickness;

        public PerlinWormData(Vector2 start)
        {
            _startPoint = start;
            _thickness = new(1f, 2f);
            _length = 256;

            Direction = Vector2.up;
            Position = start;
        }

        public PerlinWormData(Vector2 start, BoundedValue<float> thickness)
        {
            _startPoint = start;
            _thickness = thickness;
            _length = 256;

            Direction = Vector2.up;
            Position = start;
        }

        public PerlinWormData(Vector2 start, BoundedValue<float> thickness, uint length)
        {
            _startPoint = start;
            _thickness = thickness;
            _length = length;

            Direction = Vector2.up;
            Position = start;
        }

        public Vector2 Direction { get; private set; }
        public Vector2 Position { get; private set; }

        public Vector2 StartPoint => _startPoint;
        public BoundedValue<float> Thickness => _thickness;
        public float Length => _length;
        public List<WormSegment> Worm => _worm;
        public virtual float Completeness => _worm.Count / _length;

        public virtual bool Step()
        {
            if (_worm.Count >= _length)
            {
                return false;
            }

            _worm.Add(new(Position, Direction, _thickness.Value));

            Debug.Log($"{Position}\t\t {_thickness.LowerBound} <= {_thickness} <= {_thickness.UpperBound} \t\t{Completeness}");

            //_thickness.Value = _thickening.Thicken(_thickness.LowerBound, _thickness.UpperBound, Completeness);
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

using System.Collections.Generic;
using UnityEngine;

namespace WorldGeneration.Core
{
    public class PerlinWormData
    {
        private readonly Vector2 _startPoint;
        private Vector2 _position;
        private Vector2 _direction;
        private readonly float _length;
        private readonly HashSet<WormSegment> _worm = new();

        public PerlinWormData(Vector2 start, float length = 256f)
        {
            _startPoint = start;
            _length = length;

            _position = _startPoint;
            _direction = Vector2.up;
        }

        public Vector2 Direction => _direction;
        public Vector2 Position => _position;
        public Vector2 StartPiont => _startPoint;
        public HashSet<WormSegment> Worm => _worm;

        public virtual bool Step()
        {
            if (_worm.Count >= _length)
            {
                return false;
            }

            _worm.Add(new(_position,_direction,1));

            _position += _direction;
            return true;
        }

        public void Direct(Vector2 direction)
        {
            if (direction.magnitude != 1)
                direction.Normalize();

            _direction = direction;
        }
    }

    public struct WormSegment
    {
        public Vector2 Position { get; set; }
        public Vector2 Direction { get; set; }
        public float Thickness { get; set; }

        public WormSegment( Vector2 position,Vector2 direction, float thickness)
        {
            Position = position;
            Direction = direction;
            Thickness = thickness;
        }
    }
}

using Assets.Scripts.WorldGeneration.Core;
using UnityEngine;

namespace WorldGeneration.Core
{
    public class DirectedPerlinWormData : PerlinWormData
    {
        private readonly Vector2 _endPoint;
        private readonly float _weight = 0.55f;

        public DirectedPerlinWormData(Vector2 start, Vector2 end) : base(start)
        {
            _endPoint = end;
        }

        public DirectedPerlinWormData(Vector2 start, Vector2 end, float minThickness, float maxThickness) : base(start, minThickness, maxThickness)
        {
            _endPoint = end;
        }

        public DirectedPerlinWormData(Vector2 start, Vector2 end, float minThickness, float maxThickness, float weight) : base(start, minThickness, maxThickness)
        {
            _endPoint = end;
            _weight = weight;
        }

        public DirectedPerlinWormData(Vector2 start, Vector2 end, float minThickness, float maxThickness, float weight, uint length) : base(start, minThickness, maxThickness, length)
        {
            _endPoint = end;
            _weight = weight;
        }

        public DirectedPerlinWormData(Vector2 start, Vector2 end, float minThickness, float maxThickness, float weight, uint length, IThickeningStrategy thickeningStrategy) : base(start, minThickness, maxThickness, length, thickeningStrategy)
        {
            _endPoint = end;
            _weight = weight;
        }

        protected override float CurrentThickness => _thickening.Thicken(MaxThickness, MinThickness, 1f - Vector2.Distance(Position, _endPoint) / Vector2.Distance(StartPoint, _endPoint));

        public Vector2 EndPoint => _endPoint;
        public float Weight => _weight;
         
        public override bool Step()
        {
            if (Vector2.Distance(Position, _endPoint) < 3)
                return false;

            return base.Step();
        }

    }
}

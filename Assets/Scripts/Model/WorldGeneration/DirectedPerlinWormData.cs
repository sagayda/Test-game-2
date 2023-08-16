using UnityEngine;

namespace Assets.Scripts.Model.WorldGeneration
{
    public class DirectedPerlinWormData : PerlinWormData
    {
        private Vector2 _endPoint;
        private float _weight = 0.55f;

        public DirectedPerlinWormData(Vector2 start, Vector2 end, float length = 256) : base(start, length)
        {
            _endPoint = end;
        }
        public DirectedPerlinWormData(Vector2 start, Vector2 end, float weight, float length = 256) : base(start, length)
        {
            _endPoint = end;
            _weight = weight;
        }

        public Vector2 EndPoint => _endPoint;
        public float Weight => _weight;

        public void ChangeEndPoint(Vector2 end)
        {
            _endPoint = end;
        }

        public void ChangeWeight(float weight)
        {
            _weight = weight;
        }

        public override bool Step()
        {
            if(Vector2.Distance(Position, _endPoint) <= 1)
                return false;

            return base.Step();
        }

    }
}

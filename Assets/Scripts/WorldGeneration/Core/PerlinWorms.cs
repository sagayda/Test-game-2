using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WorldGeneration.Core
{
    public class PerlinWorms
    {
        PerlinWormsParameters _parameters;

        public PerlinWorms(PerlinWormsParameters parameters)
        {
            _parameters = parameters;
        }

        public List<WormSegment> CreateWorm(PerlinWormData data)
        {
            while (data.Step())
            {
                DirectWorm(data);
            }

            return data.Worm.ToList();
        }

        public List<WormSegment> CreateWorm(DirectedPerlinWormData data)
        {
            while (data.Step())
            {
                DirectWorm(data);
            }

            return data.Worm.ToList();
        }

        private Vector2 DirectWorm(PerlinWormData data)
        {
            float noise = OctavePerlinNoise.Noise(data.Position, _parameters.Noise);
            float degrees = RangeMap(noise, -90, 90);
            data.Direct((Quaternion.AngleAxis(degrees, Vector3.forward) * data.Direction).normalized);

            return data.Direction;
        }

        private Vector2 DirectWorm(DirectedPerlinWormData data)
        {
            Vector2 direction = DirectWorm((PerlinWormData)data);

            Vector2 directionToEndPoint = (data.EndPoint - data.Position).normalized;
            Vector2 endDirection = (direction * (1 - data.Weight) + directionToEndPoint * data.Weight).normalized;
            data.Direct(endDirection);

            return data.Direction;
        }

        private float RangeMap(float noise, float min, float max)
        {
            float range = max - min;
            float rangedValue = noise * range;
            return rangedValue + min;
        }
    }
}

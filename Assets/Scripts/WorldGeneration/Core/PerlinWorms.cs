using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WorldGeneration.Core.Noise;

namespace WorldGeneration.Core
{
    public class PerlinWorms
    {
        private FractalNoise _noiseProvider;
        private PerlinWormsParameters _parameters;

        public PerlinWorms(PerlinWormsParameters parameters)
        {
            _parameters = parameters;
            _noiseProvider = new FractalNoise(_parameters.Noise);
        }

        public List<WormSegment> CreateWorm(PerlinWormData data)
        {
            DirectWorm(data);

            while (data.Step())
            {
                DirectWorm(data);
            }

            return data.Worm.ToList();
        }

        public List<WormSegment> CreateWorm(DirectedPerlinWormData data)
        {
            DirectWorm(data);

            while (data.Step())
            {
                DirectWorm(data);
            }

            return data.Worm.ToList();
        }

        private Vector2 DirectWorm(PerlinWormData data)
        {
            float noise = _noiseProvider.Generate(data.Position);
            float degrees = RangeMap((1f -noise), -90, 90);
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

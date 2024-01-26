using UnityEngine;

namespace WorldGeneration.Core.WaterBehavior
{
    public class WaterParticle
    {
        public WaterSourceOUTDATE Source { get; }
        public float Volume { get; }
        public Vector2Int Position { get; set; }

        public WaterParticle(WaterSourceOUTDATE source, Vector2Int position)
        {
            Source = source;
            Volume = Source.Volume;
            Position = position;
        }

        /// <summary>
        /// Returns the water cell from which this step was taken
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public WaterCell Step(Vector2 direction)
        {
            Vector2Int gridDirection = DirectionToGridDirection(direction);

            Position += gridDirection;

            return new WaterCell
            {
                //fix
                //Stream = direction.normalized * Volume,
                Volume = Volume,
            };

        }

        private Vector2Int DirectionToGridDirection(Vector2 direction)
        {
            var directionNormalized = direction.normalized;

            int x = Mathf.RoundToInt(directionNormalized.x);
            int y = Mathf.RoundToInt(directionNormalized.y);

            if (Mathf.Abs(x) == Mathf.Abs(y))
            {
                Debug.LogWarning($"Got vector {x}, {y}");
                return new Vector2Int(x, 0);
            }

            return new Vector2Int(x, y);
        }
    }
}

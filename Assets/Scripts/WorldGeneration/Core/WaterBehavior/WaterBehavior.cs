using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WorldGeneration.Core
{
    public class WaterBehavior
    {
        public WorldGenerator WorldGenerator { get; set; }
        public float[,] RiverMap { get; private set; }

        public float[,] LakesMap { get; private set; }

        public WaterBehavior(WorldGenerator worldGenerator)
        {
            WorldGenerator = worldGenerator;
            RiverMap = new float[WorldGenerator.WorldWidth, WorldGenerator.WorldHeight];
            LakesMap = new float[WorldGenerator.WorldWidth, WorldGenerator.WorldHeight];
        }

        public void StartSource(Vector2 position)
        {
            WaterBehaviorData data = new(position, new(0.1f, 0.1f, 2f));
            data.Direction = FindDirection(position, out float str);
            data.ImpulseModule = str;
            var river = DevelopSource(data);
            AddRiver(river);
        }

        private Vector2 FindDirection(Vector2 position, out float diffSum)
        {
            var directions = GetDirectionsInRadius(1f);

            Dictionary<float, Vector2> heightDirectionPairs = new Dictionary<float, Vector2>();
            foreach (var dir in directions)
            {
                Vector2Int pos = new(Mathf.RoundToInt(position.x + dir.x), Mathf.RoundToInt(position.y + dir.y));

                heightDirectionPairs.Add(WorldGenerator.GetHeightValue(pos.x, pos.y), dir);
            }

            float currentHeight = WorldGenerator.GetHeightValue(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));

            Vector2 resDirection = Vector2.zero;
            diffSum = 0;
            foreach (var pair in heightDirectionPairs)
            {
                resDirection += pair.Value * (currentHeight - pair.Key);
                diffSum += currentHeight - pair.Key;
            }


            return resDirection;
        }

        private Vector3 GetNormal(Vector2 point, float epsilon)
        {
            float doubleRadius = -(epsilon * 2);

            float x = point.x;
            float y = point.y;

            float left = WorldGenerator.GetHeightValue(x - epsilon, y);
            float top = WorldGenerator.GetHeightValue(x, y - epsilon);
            float right = WorldGenerator.GetHeightValue(x + epsilon, y);
            float bottom = WorldGenerator.GetHeightValue(x, y + epsilon);

            return new Vector3()
            {
                x = doubleRadius * (right - left),
                y = doubleRadius * (bottom - top),
                z = doubleRadius * doubleRadius,
            }.normalized;

            //float height = WorldGenerator.GetHeightValue(point);
            //float dx = (WorldGenerator.GetHeightValue(new Vector2(point.x + epsilon, point.y)) - height) / epsilon;
            //float dy = (WorldGenerator.GetHeightValue(new Vector2(point.x, point.y + epsilon)) - height) / epsilon;

            //Vector3 normal = new Vector3(-dx, -dy, 1).normalized;

            //return normal;
        }

        private List<WaterBehaviorSegment> DevelopSource(WaterBehaviorData data)
        {
            int counter = 0;

            while (true)
            {
                var direction = FindDirection(data.Position, out float diffSumm);

                var normal = GetNormal(data.Position, 1f);

                Debug.Log($"{normal.x} \t {normal.y} \t {normal.z}");
                Debug.Log($"{data.Position}");

                if (normal.z == 1)
                {
                    Debug.Log("Normal y is 1");
                    break;
                }

                data.Step(normal);

                if (data.ImpulseModule < 0)
                {
                    Debug.Log($"InertiaMagnitude {direction.magnitude} < 0");
                    break;
                }

                if (WorldGenerator.GetHeightValue(Mathf.RoundToInt(data.Position.x), Mathf.RoundToInt(data.Position.y)) < WorldGenerator.WaterLevel)
                {
                    Debug.Log("Water level");
                    break;
                }

                if (counter > 256)
                {
                    Debug.Log("256 iterations");
                    break;
                }

                counter++;
            }

            Debug.Log($"Source has {counter} iterations");

            return data.Segments;
        }

        private void AddRiver(List<WaterBehaviorSegment> water)
        {
            Debug.Log($"Adding river {water.Count()}");

            foreach (var segment in water)
            {
                Vector2Int pos = new(Mathf.RoundToInt(segment.Position.x), Mathf.RoundToInt(segment.Position.y));

                if (pos.x < 1
                    || pos.x > WorldGenerator.WorldWidth - 1
                    || pos.y < 1
                    || pos.y > WorldGenerator.WorldHeight - 1)
                {
                    continue;
                }

                RiverMap[pos.x, pos.y] = 1f;
            }
        }

        private static Vector2Int[] GetDirectionsInRadius(float radius)
        {
            List<Vector2Int> result = new List<Vector2Int>();

            int intRaduis = Mathf.RoundToInt(radius);

            for (int i = -intRaduis; i <= intRaduis; i++)
            {
                for (int j = -intRaduis; j <= intRaduis; j++)
                {
                    Vector2Int offset = new Vector2Int(i, j);

                    if (offset.magnitude <= radius)
                        result.Add(offset);
                }
            }

            return result.ToArray();
        }
    }
}

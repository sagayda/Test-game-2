using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WorldGeneration.Core.Outdate
{
    public class WaterBehavior
    {
        public WorldGenerator WorldGenerator { get; set; }

        public float[,] RiverMap { get; private set; }
        public float[,] SettledWaterMap { get; private set; }

        public List<WaterSource> Sources { get; private set; }

        public WaterBehavior(WorldGenerator worldGenerator)
        {
            WorldGenerator = worldGenerator;

            RiverMap = new float[WorldGenerator.Width, WorldGenerator.Height];
            SettledWaterMap = new float[WorldGenerator.Width, WorldGenerator.Height];

            Sources = new List<WaterSource>();
        }

        public void CreateSource(Vector2Int position)
        {
            Vector2 direction = FindDirection(position, out _);
            Vector2Int directionInt = ConvertToUnitVector2Int(direction);

            Sources.Add(new(position, directionInt));
        }

        public void IterateAllSources()
        {
            List<WaterSource> sourcesToRemove = new List<WaterSource>();

            foreach (var source in Sources)
            {
                if (IterateSource(source, out WaterSource returned))
                {
                    sourcesToRemove.Add(returned);
                }
            }

            foreach (var source in sourcesToRemove)
            {
                Sources.Remove(source);
            }
        }

        public bool IterateSource(WaterSource source, out WaterSource returned)
        {
            WaterBehaviorData data = new(source.Start, new UniversalTools.BoundedValue<float>(0.1f, 0.1f, 2f));
            data.Step(source.StartDirection);

            int counter = 0;

            bool needReturn = false;

            while (true)
            {
                Vector2Int intPosition = new(Convert.ToInt32(data.Position.x), Convert.ToInt32(data.Position.y));

                Vector3 normal = GetNormal(intPosition, 1);

                if (normal.z == 1)
                {
                    Debug.Log("Normal y is 1");

                    SettledWaterMap[intPosition.x, intPosition.y] += 0.05f;

                    break;
                }

                Vector2Int direction = ConvertToUnitVector2Int(normal);

                data.Step(direction);

                //if (data.ImpulseModule < 0)
                //{
                //    Debug.Log($"InertiaMagnitude {direction.magnitude} < 0");
                //    break;
                //}

                if (WorldGenerator.GetMapValue(data.Position, MapValueType.Height) < WorldGenerator.OceanLevel)
                {
                    Debug.Log("Water level");
                    needReturn = true;
                    break;
                }

                if (data.Position.x < 0 ||
                    data.Position.y < 0 ||
                    data.Position.x >= WorldGenerator.Width ||
                    data.Position.y >= WorldGenerator.Height)
                {
                    Debug.Log("Out of bounds");
                    needReturn = true;
                    break;
                }

                if (counter > 256)
                {
                    Debug.Log("256 iterations");
                    SettledWaterMap[intPosition.x, intPosition.y] += 0.05f;

                    break;
                }

                //if (counter > 250)
                //{
                //    Debug.Log($"{data.Position}");
                //}

                counter++;
            }

            Debug.Log($"Source had {counter} iterations");

            AddRiver(data.Segments);

            if (needReturn)
            {
                returned = source;
                return true;
            }

            returned = default;
            return false;
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

                heightDirectionPairs.Add(WorldGenerator.GetMapValue(pos, MapValueType.Height), dir);
            }

            float currentHeight = WorldGenerator.GetMapValue(position, MapValueType.Height);

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

            float left = WorldGenerator.GetMapValue(new(x - epsilon, y), MapValueType.Height);
            float top = WorldGenerator.GetMapValue(new(x, y - epsilon), MapValueType.Height);
            float right = WorldGenerator.GetMapValue(new(x + epsilon, y), MapValueType.Height);
            float bottom = WorldGenerator.GetMapValue(new(x, y + epsilon), MapValueType.Height);

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

        private Vector3 GetNormal(Vector2Int point, int epsilon)
        {
            int doubleRadius = -(epsilon * 2);

            int x = point.x;
            int y = point.y;

            float left = WorldGenerator.GetMapValue(new(x - epsilon, y), MapValueType.Height) + GetSettledWater(x - epsilon, y);
            float top = WorldGenerator.GetMapValue(new(x, y - epsilon), MapValueType.Height) + GetSettledWater(x, y - epsilon);
            float right = WorldGenerator.GetMapValue(new(x + epsilon, y), MapValueType.Height) + GetSettledWater(x + epsilon, y);
            float bottom = WorldGenerator.GetMapValue(new(x, y + epsilon), MapValueType.Height) + GetSettledWater(x, y + epsilon);

            return new Vector3()
            {
                x = doubleRadius * (right - left),
                y = doubleRadius * (bottom - top),
                z = doubleRadius * doubleRadius,
            }.normalized;
        }

        private float GetSettledWater(int x, int y)
        {
            if (x < 0 ||
                y < 0 ||
                x >= SettledWaterMap.GetLength(0) ||
                y >= SettledWaterMap.GetLength(1))
                return 0;

            return SettledWaterMap[x, y];
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

                if (WorldGenerator.GetMapValue(data.Position, MapValueType.Height) < WorldGenerator.OceanLevel)
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
                    || pos.x > WorldGenerator.Width - 1
                    || pos.y < 1
                    || pos.y > WorldGenerator.Height - 1)
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

        private Vector2Int ConvertToUnitVector2Int(Vector2 vector)
        {
            vector.Normalize(); // Normalize the vector to make it unit length

            int x = Mathf.RoundToInt(vector.x);
            int y = Mathf.RoundToInt(vector.y);

            return new Vector2Int(x, y);
        }
    }

    public readonly struct WaterSource
    {
        public readonly Vector2Int Start;
        public readonly Vector2Int StartDirection;

        public WaterSource(Vector2Int start, Vector2Int startDirection)
        {
            Start = start;
            StartDirection = startDirection;
        }
    }
}

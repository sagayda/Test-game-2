using System.Collections.Generic;
using Assets.Scripts.WorldGeneration.Core.Chunks;
using UnityEngine;
using WorldGeneration.Core;
using WorldGeneration.Core.WaterBehavior;

namespace Assets.Scripts.WorldGeneration.Core.WaterBehavior
{
    public class WaterBehaviour
    {
        private float _oceanLevel;
        private List<River> _rivers;

        public WaterBehaviour(float oceanLevel)
        {
            _rivers = new List<River>();
            _oceanLevel = oceanLevel;
        }

        public float OceanLevel => _oceanLevel;
        public IEnumerable<River> Rivers => _rivers;

        public void CreateSource(Chunk chunk, float strength)
        {
            if (chunk.IsWaterChunk)
            {
                if (chunk.Water.IsSource)
                    return;

                chunk.Water.Source = new(strength);
                return;
            }

            chunk.Water = new(0, new(0, 0))
            {
                Source = new(strength)
            };
        }

        public void CreateRiver(Chunk sourceChunk, World world)
        {
            River river = new(sourceChunk);

            _rivers.Add(river);

            int iterations = 0;

            while (IterateRiver(river, world))
            {
                iterations++;
                Debug.Log($"Iteration {iterations}");

                if( iterations > 256)
                {
                    break;
                }
            }
        }

        private bool IterateRiver(River river, World world)
        {
            if (river.HasLeakage)
            {
                Debug.Log("Try to iterate river with leakege");
                return false;
            }

            Vector3 normal = GetNormal(river.LastSegment.Position, 1, world);

            if(normal.z == 1)
            {
                Debug.Log($"Got z = 1 normal");
                return false;
            }

            normal *= Chunk.ChunkWidth;

            Vector2Int intNormal = new Vector2Int(Mathf.FloorToInt(normal.x), Mathf.FloorToInt(normal.y));

            Chunk nextSegment = world.GetChunkByGlobalCoordinates(river.LastSegment.Position + intNormal);

            if(world.Ocean.Contains(nextSegment))
            {
                Debug.Log($"Finished river at {nextSegment.Position}");

                river.TryAddSegment(nextSegment, true);

                return false;
            }

            nextSegment.Water = new(0.2f, normal);

            river.TryAddSegment(nextSegment);

            return true;
        }

        public void CreateOcean(World world)
        {
            List<IMapArea> oceanChunks = new();

            foreach (var chunk in world.Chunks)
                if (chunk.Value.Values[MapValueType.Height] <= world.OceanLevel)
                    oceanChunks.Add(chunk.Value);

            world.TrySetOcean(new(oceanChunks));
        }

        private Vector3 GetNormal(Vector2Int point, int epsilon, World world)
        {
            int doubleRadius = -(epsilon * 2);

            int x = point.x;
            int y = point.y;

            //float left = _worldGenerator.GetMapValue(x - epsilon, y, MapValueType.Height);
            //float top = _worldGenerator.GetMapValue(x, y - epsilon, MapValueType.Height);
            //float right = _worldGenerator.GetMapValue(x + epsilon, y, MapValueType.Height);
            //float bottom = _worldGenerator.GetMapValue(x, y + epsilon, MapValueType.Height);

            float left = world.GetChunkByGlobalCoordinates(x - epsilon, y).Values[MapValueType.Height];
            float top = world.GetChunkByGlobalCoordinates(x, y - epsilon).Values[MapValueType.Height];
            float right = world.GetChunkByGlobalCoordinates(x + epsilon, y).Values[MapValueType.Height];
            float bottom = world.GetChunkByGlobalCoordinates(x, y + epsilon).Values[MapValueType.Height];

            return new Vector3()
            {
                x = doubleRadius * (right - left),
                y = doubleRadius * (bottom - top),
                z = doubleRadius * doubleRadius,
            }.normalized;
        }

    }
}

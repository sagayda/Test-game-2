using System.Collections.Generic;
using Assets.Scripts.WorldGeneration.Core.Chunks;
using Codice.CM.Client.Differences;
using UnityEngine;
using WorldGeneration.Core;
using WorldGeneration.Core.Locations;
using WorldGeneration.Core.WaterBehavior;

namespace Assets.Scripts.WorldGeneration.Core.WaterBehavior
{
    public class WaterBehaviour
    {
        private float _oceanLevel;
        private List<River> _rivers;
        private List<Pool> _pools;

        public WaterBehaviour(float oceanLevel)
        {
            _rivers = new List<River>();
            _pools = new List<Pool>();
            _oceanLevel = oceanLevel;
        }

        public float OceanLevel => _oceanLevel;
        public IEnumerable<River> Rivers => _rivers;
        public IEnumerable<Pool> Pools => _pools;

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

            Vector2 normal = GetNormal(river.LastSegment.Position, world);

            Vector2Int intNormal = new(Mathf.RoundToInt(normal.x), Mathf.RoundToInt(normal.y));

            Vector2Int nextSegmentPosition = river.LastSegment.Position + intNormal;

            if(nextSegmentPosition.x > world.WidthByChunks || nextSegmentPosition.x < 0 || nextSegmentPosition.y > world.HeightByChunks || nextSegmentPosition.y < 0)
            {
                Debug.Log("World bounds");
                river.CreateLeakage();
                return false;
            }

            Chunk nextSegment = world.GetChunkByLocalCoordinates(river.LastSegment.Position + intNormal);

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

        public void Flood(Vector2Int position, World world, out List<Vector2Int> leakages)
        {
            float step = 0.001f;
            float volume = step;

            int count = 0;
            List<Vector2Int> retur;

            while (FloodStep(position, world, volume, out retur, out leakages) == false)
            {
                count++;
                volume += step;

                if(count > 512)
                {
                    Debug.Log("512");
                    return;
                }
            }

            List<IMapArea> flooded = new List<IMapArea>();

            foreach (var item in retur)
            {
                flooded.Add(world.GetChunkByLocalCoordinates(item));
            }

            Pool pool = new(flooded);
            _pools.Add(pool);
        }

        private bool FloodStep(Vector2Int position, World world, float volume, out List<Vector2Int> retFlooded, out List<Vector2Int> leakages)
        {
            retFlooded = null;
            leakages = null;

            Chunk chunk = world.GetChunkByLocalCoordinates(position);

            float height = chunk.Values[MapValueType.Height];
            float waterHeight = chunk.Values[MapValueType.Height] + volume;

            List<Vector2Int> lastVisited = new() { position };
            List<Vector2Int> flooded = new();
            List<Vector2Int> leakagesL = new();

            while (lastVisited.Count > 0)
            {
                if(leakagesL.Count > 0)
                {
                    retFlooded = flooded;
                    leakages = leakagesL;
                    return true;
                }    

                List<Vector2Int> visited = new();

                foreach (var lastVisit in lastVisited)
                {
                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            Vector2Int pos = new(lastVisit.x + i, lastVisit.y + j);

                            if (leakagesL.Contains(pos))
                                continue;

                            if (lastVisited.Contains(pos))
                                continue;

                            if (flooded.Contains(pos))
                                continue;

                            Chunk chunkCheck = world.GetChunkByLocalCoordinates(pos);

                            //border
                            if (chunkCheck.Values[MapValueType.Height] >= waterHeight)
                                continue;

                            //leakage
                            if (chunkCheck.Values[MapValueType.Height] < height)
                                leakagesL.Add(pos);

                            visited.Add(pos);
                        }
                    }
                }

                flooded.AddRange(lastVisited);

                lastVisited = visited;
            }

            return false;

            //for (int i = -1; i <= 1; i++)
            //{
            //    for (int j = -1; j <= 1; j++)
            //    {
            //        Vector2Int pos = new(position.x + i, position.y + j);

            //        if (lastVisited.Contains(pos))
            //            continue;

            //        Chunk chunkCheck = world.GetChunkByLocalCoordinates(pos);

            //        if (chunkCheck.Values[MapValueType.Height] >= waterHeight)
            //            //border
            //            continue;

            //        if (chunkCheck.Values[MapValueType.Height] < height)
            //            //leakage
            //            return;

            //        lastVisited.Add(pos);
            //    }
            //}

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
            Vector2 gradient = new(0, 0);

            int doubleRadius = -(epsilon * 2);

            int x = point.x;
            int y = point.y;

            if(x >= 31 || x <= 0 || y>= 31 || y <= 0)
            {
                return new Vector3(0, 0, 1);
            }

            //float left = world.GetChunkByGlobalCoordinates(x - epsilon, y).Values[MapValueType.Height];
            //float top = world.GetChunkByGlobalCoordinates(x, y - epsilon).Values[MapValueType.Height];
            //float right = world.GetChunkByGlobalCoordinates(x + epsilon, y).Values[MapValueType.Height];
            //float bottom = world.GetChunkByGlobalCoordinates(x, y + epsilon).Values[MapValueType.Height];

            float left = world.GetChunkByLocalCoordinates(new(x - epsilon, y)).Values[MapValueType.Height];
            float top = world.GetChunkByLocalCoordinates(new(x, y + epsilon)).Values[MapValueType.Height];
            float right = world.GetChunkByLocalCoordinates(new(x + epsilon, y)).Values[MapValueType.Height];
            float bottom = world.GetChunkByLocalCoordinates(new(x, y - epsilon)).Values[MapValueType.Height];

            float leftTop = world.GetChunkByLocalCoordinates(new(x - epsilon, y + epsilon)).Values[MapValueType.Height];
            float rightTop = world.GetChunkByLocalCoordinates(new(x + epsilon, y + epsilon)).Values[MapValueType.Height];
            float leftBottom = world.GetChunkByLocalCoordinates(new(x - epsilon, y - epsilon)).Values[MapValueType.Height];
            float rightBottom = world.GetChunkByLocalCoordinates(new(x + epsilon, y - epsilon)).Values[MapValueType.Height];

            //Vector3 normal = new Vector3()
            //{
            //    x = (left - right) + (leftTop - rightTop) + (leftBottom - rightBottom),
            //    y = (bottom - top) + (leftBottom - leftTop) + (rightBottom - rightTop),
            //    z = 0,
            //};

            //Vector3 normal = new Vector3()
            //{
            //    x = (left - right) + (leftTop - top) + (top - rightTop) + (leftBottom - bottom) + (bottom - rightBottom),
            //    y = (bottom - top) + (leftBottom - left) + (left - leftTop) + (rightBottom - right) + (right - rightTop),
            //    z = 0,
            //};

            Vector3 normal = new Vector3()
            {
                x = (left - right) + (leftBottom - rightTop) + (leftTop - rightBottom),
                y = (bottom - top) + (leftBottom - rightTop) + (rightBottom - leftTop),
                z = 0,
            };

            normal.Normalize();

            return normal;


            //return new Vector3()
            //{
            //    x = doubleRadius * (right - left),
            //    y = doubleRadius * (bottom - top),
            //    z = doubleRadius * doubleRadius,
            //}.normalized;
        }

        private Vector2 GetNormal(Vector2Int point, World world)
        {
            Vector2 direction = Vector2.zero;
            float height = world.GetChunkByLocalCoordinates(point).Values[MapValueType.Height];

            if(point.x == world.WidthByChunks)
                return new(1, 0);

            if(point.x == 0)
                return new(-1, 0);

            if (point.y == world.HeightByChunks)
                return new(0, 1);

            if(point.y == 0)
                return new(0, -1);

            Vector2Int[] directions =
            {
                new(0,1),
                new(0,-1),
                new(1,0),
                new(-1,0),
            };

            //foreach (var direction1 in directions)
            //{
            //    float currentHeight = world.GetChunkByLocalCoordinates(point + direction1).Values[MapValueType.Height];
            //    if (currentHeight < height)
            //    {
            //        direction = direction1;

            //        height = currentHeight;
            //    }
            //}

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    float currentHeight = world.GetChunkByLocalCoordinates(new(point.x + i, point.y + j)).Values[MapValueType.Height];

                    if (currentHeight < height)
                    {
                        direction = new(i, j);

                        height = currentHeight;
                    }
                }
            }

            return direction.normalized;
        }

    }
}

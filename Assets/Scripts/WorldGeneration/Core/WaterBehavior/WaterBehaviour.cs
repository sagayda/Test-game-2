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
                Debug.LogError($"Too many river iterations (256) at {sourceChunk.Position}. River generation stopped");

                if ( iterations > 256)
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

        public void Flood(Vector2Int startPosition, World world, out IEnumerable<Vector2Int> leakages)
        {
            float step = 0.001f;
            float waterVolume = step;
            int stepsCount = 0;

            IEnumerable<Vector2Int> flooded;

            while (FloodStep(startPosition, world, waterVolume, out flooded, out leakages) == false)
            {
                stepsCount++;
                waterVolume += step;

                if(stepsCount > 512)
                {
                    Debug.LogError($"Too many flood steps (512) at {startPosition}. Flooding stopped");
                    return;
                }
            }

            List<IMapArea> poolCells = new List<IMapArea>();

            foreach (var item in flooded)
            {
                poolCells.Add(world.GetChunkByLocalCoordinates(item));
            }

            Pool pool = new(poolCells);
            _pools.Add(pool);
        }

        private bool FloodStep(Vector2Int position, World world, float volume, out IEnumerable<Vector2Int> flooded, out IEnumerable<Vector2Int> leakages)
        {
            flooded = null;
            leakages = null;

            Chunk startChunk = world.GetChunkByLocalCoordinates(position);

            float mainHeight = startChunk.Values[MapValueType.Height];
            float mainWaterHeight = startChunk.Values[MapValueType.Height] + volume;

            List<Vector2Int> lastIterationVisited = new() { position };
            List<Vector2Int> currentFlooded = new();
            HashSet<Vector2Int> foundedLeakages = new();

            bool leakageFound = false;

            while (leakageFound == false && lastIterationVisited.Count > 0)
            {
                List<Vector2Int> visited = new();

                foreach (var cell in lastIterationVisited)
                {
                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            Vector2Int pos = new(cell.x + i, cell.y + j);

                            if (lastIterationVisited.Contains(pos))
                                continue;

                            if (currentFlooded.Contains(pos))
                                continue;

                            Chunk neighbourCell = world.GetChunkByLocalCoordinates(pos);

                            //border
                            if (neighbourCell.Values[MapValueType.Height] >= mainWaterHeight)
                                continue;

                            //leakage
                            if (neighbourCell.Values[MapValueType.Height] < mainHeight)
                            {
                                foundedLeakages.Add(pos);
                                leakageFound = true;
                            }

                            visited.Add(pos);
                        }
                    }
                }

                currentFlooded.AddRange(lastIterationVisited);

                lastIterationVisited = visited;
            }

            if (leakageFound)
            {
                flooded = currentFlooded;
                leakages = foundedLeakages;
                return true;
            }

            return false;
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

        //
        public void ClearRivers()
        {
            if(_rivers.Count == 0) 
                return;

            foreach (var river in _rivers)
            {
                foreach (var chunk in river.Chunks)
                {
                    chunk.Water = null;
                }
            }

            _rivers.Clear();
        }

        public void ClearPools()
        {
            _pools.Clear();
        }
    }
}

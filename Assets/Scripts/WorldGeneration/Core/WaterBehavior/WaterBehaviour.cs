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
            if (chunk.HasWater)
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
            //if (sourceChunk.Water.Source.Strength <= 0.001f)
            //    return;

            River river = new(sourceChunk);

            _rivers.Add(river);

            int iterations = 0;

            Pool flowsInto;

            while (IterateRiver(river, world, out flowsInto))
            {
                iterations++;

                if (iterations > 256)
                {
                    Debug.LogError($"Too many river iterations (256) at {sourceChunk.Position}. River generation stopped");
                    break;
                }
            }

            if(flowsInto.EvaporatingVolume < river.Strength)
            {
                foreach (var leakage in flowsInto.Leakages)
                {
                    CreateSource(leakage as Chunk, river.Strength - flowsInto.EvaporatingVolume);
                    CreateRiver(leakage as Chunk, world);
                }
            }
        }

        private bool IterateRiver(River river, World world, out Pool flowsInto)
        {
            flowsInto = null;
            
            if (river.HasLeakage)
            {
                Debug.Log("Tried to iterate river with leakege");
                return false;
            }

            //Vector2 normal = GetNormal(river.LastSegment.Position, world);

            Vector2 normal = GetNormalIncludingWater(river.LastSegment.Position, world);

            Vector2Int intNormal = new(Mathf.RoundToInt(normal.x), Mathf.RoundToInt(normal.y));

            //
            if (intNormal == Vector2Int.zero)
            {
                flowsInto = Flood(river.LastSegment.Position, world, river.Strength, out _);
                river.CreateLeakage();
                return false;
            }
            //

            Vector2Int nextSegmentPosition = river.LastSegment.Position + intNormal;

            if (nextSegmentPosition.x > world.WidthByChunks || nextSegmentPosition.x < 0 || nextSegmentPosition.y > world.HeightByChunks || nextSegmentPosition.y < 0)
            {
                Debug.Log("World bounds");
                river.CreateLeakage();
                return false;
            }

            Chunk nextSegment = world.GetChunkByLocalCoordinates(river.LastSegment.Position + intNormal);

            if (world.Ocean.Contains(nextSegment))
            {
                Debug.Log($"Finished river at {nextSegment.Position}");

                flowsInto = world.Ocean;

                river.TryAddSegment(nextSegment, true);

                return false;
            }

            nextSegment.Water = new(river.Strength, normal);

            river.TryAddSegment(nextSegment);

            return true;
        }

        public Pool Flood(Vector2Int startPosition, World world, float maxVolume, out IEnumerable<Vector2Int> leakages)
        {
            float step = 0.001f;
            float waterVolume = step;
            int stepsCount = 0;

            Dictionary<Vector2Int, float> flooded;

            while (FloodStep(startPosition, world, waterVolume, out flooded, out leakages) == false)
            {
                stepsCount++;
                waterVolume += step;

                if (waterVolume >= maxVolume)
                    break;

                if (stepsCount > 512)
                {
                    Debug.LogError($"Too many flood steps (512) at {startPosition}. Flooding stopped");
                    return null;
                }
            }

            List<IMapArea> poolCells = new();

            foreach (var item in flooded)
            {
                Chunk chunk = world.GetChunkByLocalCoordinates(item.Key);

                if (chunk.HasWater)
                    chunk.Water.Volume += item.Value;
                else
                    chunk.Water = new(item.Value, new());

                poolCells.Add(chunk);
            }

            List<IMapArea> poolLeakageCells = new();

            if (leakages != null)
            {
                foreach (var item in leakages)
                {
                    poolLeakageCells.Add(world.GetChunkByLocalCoordinates(item));
                }
            }



            Pool pool = new();

            pool.TryAddSegment(poolCells.ToArray());
            pool.TryAddLeakage(poolLeakageCells.ToArray());

            _pools.Add(pool);

            return pool;
        }

        private bool FloodStep(Vector2Int position, World world, float volume, out Dictionary<Vector2Int, float> flooded, out IEnumerable<Vector2Int> leakages)
        {
            flooded = null;
            leakages = null;

            Chunk startChunk = world.GetChunkByLocalCoordinates(position);

            float mainHeight = startChunk.Values[MapValueType.Height];
            float mainWaterHeight = startChunk.Values[MapValueType.Height] + volume;

            Dictionary<Vector2Int, float> lastIterationVisited = new()
            {
                { position, volume }
            };
            Dictionary<Vector2Int, float> currentFlooded = new();
            HashSet<Vector2Int> foundedLeakages = new();

            bool leakageFound = false;

            while (leakageFound == false && lastIterationVisited.Count > 0)
            {
                Dictionary<Vector2Int, float> visited = new();

                foreach (var cell in lastIterationVisited)
                {
                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            Vector2Int pos = new(cell.Key.x + i, cell.Key.y + j);

                            if (lastIterationVisited.ContainsKey(pos))
                                continue;

                            if (currentFlooded.ContainsKey(pos))
                                continue;

                            if (visited.ContainsKey(pos))
                                continue;

                            if (pos.x < 0 || pos.y < 0 || pos.x >= world.WidthByChunks || pos.y >= world.HeightByChunks)
                                continue;

                            Chunk neighbourCell = world.GetChunkByLocalCoordinates(pos);

                            //border
                            if (neighbourCell.Values[MapValueType.Height] >= mainWaterHeight)
                                continue;

                            if(neighbourCell.HasWater)
                                if (neighbourCell.Values[MapValueType.Height] + neighbourCell.Water.Volume >= mainWaterHeight)
                                    continue;

                            //leakage
                            if (neighbourCell.Values[MapValueType.Height] < mainHeight)
                            {
                                foundedLeakages.Add(pos);
                                leakageFound = true;
                            }

                            visited.Add(pos, mainWaterHeight - neighbourCell.Values[MapValueType.Height]);
                        }
                    }
                }

                foreach (var item in lastIterationVisited)
                {
                    currentFlooded.Add(item.Key, item.Value);
                }

                lastIterationVisited = visited;
            }

            if (leakageFound)
            {
                flooded = currentFlooded;
                leakages = foundedLeakages;
                return true;
            }

            flooded = currentFlooded;
            return false;
        }

        private bool FloodPool(Pool pool, World world, float volume)
        {
/*             List<IMapArea> current

            bool leakageFound = false;

            while (true)
            {
                List<Chunk> visited = new(); 
            } */
            return false;
        }

        public void CreateOcean(World world)
        {
            List<IMapArea> oceanChunks = new();

            foreach (var chunk in world.Chunks)
            {
                if (chunk.Value.Values[MapValueType.Height] <= world.OceanLevel)
                {
                    chunk.Value.Water = new(world.OceanLevel - chunk.Value.Values[MapValueType.Height], new());
                    oceanChunks.Add(chunk.Value);
                }
            }

            Pool ocean = new Pool();

            ocean.TryAddSegment(oceanChunks.ToArray());

            world.TrySetOcean(ocean);
        }

        private Vector3 GetNormal(Vector2Int point, int epsilon, World world)
        {
            Vector2 gradient = new(0, 0);

            int doubleRadius = -(epsilon * 2);

            int x = point.x;
            int y = point.y;

            if (x >= 31 || x <= 0 || y >= 31 || y <= 0)
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

            if (point.x == world.WidthByChunks)
                return new(1, 0);

            if (point.x == 0)
                return new(-1, 0);

            if (point.y == world.HeightByChunks)
                return new(0, 1);

            if (point.y == 0)
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

        private Vector2 GetNormalIncludingWater(Vector2Int point, World world)
        {

            Vector2 direction = Vector2.zero;

            float height = world.GetChunkByLocalCoordinates(point).Values[MapValueType.Height];

            if (point.x == world.WidthByChunks)
                return new(1, 0);

            if (point.x == 0)
                return new(-1, 0);

            if (point.y == world.HeightByChunks)
                return new(0, 1);

            if (point.y == 0)
                return new(0, -1);

            Vector2Int[] directions =
            {
                new(0,1),
                new(0,-1),
                new(1,0),
                new(-1,0),
            };

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    Chunk chunk = world.GetChunkByLocalCoordinates(new(point.x + i, point.y + j));

                    float currentHeight = chunk.Values[MapValueType.Height];

                    if (chunk.HasWater)
                        currentHeight += chunk.Water.Volume;

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
            if (_rivers.Count == 0)
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

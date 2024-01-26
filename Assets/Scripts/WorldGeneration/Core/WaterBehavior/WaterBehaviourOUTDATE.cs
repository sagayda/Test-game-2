using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WorldGeneration.Core.WaterBehavior;

namespace WorldGeneration.Core
{
    public class WaterBehaviourOUTDATE
    {
        private readonly WorldGenerator _worldGenerator;
        private readonly WaterCell[,] _waterMap;

        private readonly HashSet<WaterCell> _waterMapHash = new HashSet<WaterCell>();

        private readonly List<Vector2Int> _sources = new List<Vector2Int>();
        private readonly List<Vector2Int> _finishedSources = new List<Vector2Int>();

        private readonly float _allowableHeightDifference = 0.01f;

        public WaterBehaviourOUTDATE(WorldGenerator worldGenerator)
        {
            _worldGenerator = worldGenerator;

            _waterMap = new WaterCell[_worldGenerator.Width, _worldGenerator.Height];
        }

        public void AddSource(Vector2Int position, float strength)
        {
            Vector2 normal = GetNormal(position, 1);
            normal.Normalize();

            WaterSourceOUTDATE source = new(strength, normal);

            _waterMap[position.x, position.y] = source;

            _sources.Add(position);
        }

        private bool IterateSource(Vector2Int position)
        {
            if (_waterMap[position.x, position.y] is not WaterSourceOUTDATE source)
                return true;

            if (source.HasLeakage)
            {
                _sources.Remove(position);
                _finishedSources.Add(position);
                return true;
            }

            WaterParticle particle = new(source, position);

            while (true)
            {
                Vector3 normal = GetNormal(particle.Position, 1);

                if (normal.z == 1)
                {
                    Debug.Log("Vertical normal");

                }
            }

        }

        private List<Vector2Int> Flood(Vector2Int position, WaterSourceOUTDATE floodSource)
        {
            WaterCell cell = GetOrCreateWaterCell(position);

            //fix
            return null;
        }
        /// <summary>
        /// Cascading iteration in position. Returns whether recascading is needed
        /// </summary>
        /// <returns></returns>
        private bool Cascade(Vector2Int position)
        {
            HashSet<Vector2Int> visited = new HashSet<Vector2Int>();

            List<Vector2Int> toVisit = new List<Vector2Int>
            {
                position,
            };

            int counter = 0;

            while (toVisit.Count() > 0)
            {
                List<Vector2Int> affected = new List<Vector2Int>();

                foreach (var cellToVisit in toVisit)
                {
                    //fix
                    //affected.AddRange(CascadeStep(cellToVisit, visited));
                }

                toVisit = affected;

                if (counter > 256)
                {
                    Debug.Log("Infinitive cycle");
                    break;
                }

                counter++;
            }

            if (counter <= 1)
                return false;

            return true;
        }

        /// <summary>
        /// Water balancing at the position and at the neighbors
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        private List<Vector2Int> CascadeStep(Vector2Int position, HashSet<Vector2Int> visited, out List<Vector2Int> leakages)
        {
            List<WaterCell> cellsToBallance = new List<WaterCell>()
            {
                GetOrCreateWaterCell(position),
            };

            //add neighbours
            foreach (var direction in _directions)
            {
                if (PrepareCascade(position + direction, visited, out WaterCell cell) == false)
                {
                    continue;
                }

                cellsToBallance.Add(cell);
            }

            var orderedCells = cellsToBallance.OrderByDescending((cell) => cell.LandLevel).ToList();

            float waterToBallance = CalculateWaterForBallance(orderedCells);

            //if water is not enough
            while (waterToBallance <= 0 && orderedCells.Count > 1)
            {
                orderedCells.RemoveAt(0);
                waterToBallance = CalculateWaterForBallance(orderedCells);
            }

            if (orderedCells.Count <= 0)
            {
                //fix
                leakages = null;
                //fix
                return new List<Vector2Int>();
            }

            //cells are ballanced
            if (IsEnoughBalanced(orderedCells))
            {
                //fix
                leakages = null;
                //fix
                return new List<Vector2Int>();
            }

            float desiredLevel = orderedCells.Max((elem) => elem.LandLevel) + (waterToBallance / orderedCells.Count());

            //setting water cells
            foreach (var cell in orderedCells)
            {
                float waterToSet = desiredLevel - cell.LandLevel;

                cell.Volume = waterToSet;

                _waterMap[cell.Position.x, cell.Position.y] = cell;
            }

            visited.Add(position);

            List<Vector2Int> affectedCells = new List<Vector2Int>();

            foreach (var cell in orderedCells)
            {
                if (cell.Position == position)
                    continue;

                affectedCells.Add(cell.Position);
            }

            //fix
            leakages = null;
            //fix

            return affectedCells;
        }

        private bool PrepareCascade(Vector2Int position, HashSet<Vector2Int> visited, out WaterCell waterCell)
        {
            waterCell = null;

            if (CheckCoords(position) == false)
                return false;

            if (visited.Contains(position))
                return false;

            waterCell = GetOrCreateWaterCell(position);

            return true;
        }

        private float CalculateWaterForBallance(List<WaterCell> cells)
        {
            //var topCell = cells.First();
            var topCellHeight = cells.Max((cell) => cell.LandLevel);

            float waterToBallance = 0;

            foreach (WaterCell cell in cells)
            {
                waterToBallance += cell.Volume + cell.LandLevel - topCellHeight;
            }

            return waterToBallance;
        }

        private bool IsEnoughBalanced(List<WaterCell> cells)
        {
            if (cells.Count <= 1)
                return true;

            float maxDifference = cells.Max((cell) => cell.LandLevel + cell.Volume) - cells.Min((cell) => cell.LandLevel + cell.Volume);

            return maxDifference <= _allowableHeightDifference;
        }

        private bool CheckCoords(Vector2Int position)
        {
            if (position.x < 0 || position.y < 0 || position.x >= _worldGenerator.Width || position.y >= _worldGenerator.Height)
                return false;

            return true;
        }

        private WaterCell GetOrCreateWaterCell(Vector2Int position)
        {
            WaterCell cell = _waterMap[position.x, position.y];

            if (cell != null)
                return cell;

            return new(position, _worldGenerator.GetMapValue(position, MapValueType.Height));
        }

        private Vector3 GetNormal(Vector2Int point, int epsilon)
        {
            int doubleRadius = -(epsilon * 2);

            int x = point.x;
            int y = point.y;

            //float left = _worldGenerator.GetMapValue(x - epsilon, y, MapValueType.Height);
            //float top = _worldGenerator.GetMapValue(x, y - epsilon, MapValueType.Height);
            //float right = _worldGenerator.GetMapValue(x + epsilon, y, MapValueType.Height);
            //float bottom = _worldGenerator.GetMapValue(x, y + epsilon, MapValueType.Height);

            float left = GetHeight(x - epsilon, y);
            float top = GetHeight(x, y - epsilon);
            float right = GetHeight(x + epsilon, y);
            float bottom = GetHeight(x, y + epsilon);

            return new Vector3()
            {
                x = doubleRadius * (right - left),
                y = doubleRadius * (bottom - top),
                z = doubleRadius * doubleRadius,
            }.normalized;
        }

        private float GetHeight(Vector2Int position)
        {
            if (IsPointInMapBounds(position) == false)
                return 0f;

            float height = _worldGenerator.GetMapValue(position, MapValueType.Height);
            float water = _waterMap[position.x, position.y] == null ? 0 : _waterMap[position.x, position.y].Volume;

            return height + water;
        }

        private float GetHeight(int x, int y)
        {
            return GetHeight(new Vector2Int(x, y));
        }

        private bool IsPointInMapBounds(Vector2Int position)
        {
            if (position.x < 0 ||
                position.y < 0 ||
                position.x >= _worldGenerator.Width ||
                position.y >= _worldGenerator.Height)
                return false;

            return true;
        }

        private static Vector2Int[] _directions =
        {
            new Vector2Int(1,0),
            new Vector2Int(-1,0),
            new Vector2Int(0,1),
            new Vector2Int(0,-1),
        };
    }
}

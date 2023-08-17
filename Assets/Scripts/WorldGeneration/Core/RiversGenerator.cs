using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.WorldGeneration.Core;
using log4net.Util;
using UnityEngine;

namespace WorldGeneration.Core
{
    public class RiversGenerator
    {
        public PerlinWorms PerlinWorms;

        private readonly RiversGeneratorParameters _parameters;

        public float[,] RiverMap { get; private set; }

        public RiversGenerator(RiversGeneratorParameters parameters)
        {
            _parameters = parameters;

            RiverMap = new float[_parameters.WorldWidth, _parameters.WorldHeight];
        }

        //public void GenerateRivers()
        //{
        //    var maximas = FindLocalMaximas();
        //    var minimas = FindLocalMinimas();
        //    float currentBottomLevel = 0.8f;

        //    List<DirectedPerlinWormData> unfinishedRiversData = new();

        //    foreach (var maxima in maximas)
        //    {
        //        unfinishedRiversData.Add(BuildRiverData(maxima, minimas, 0.1f));
        //    }

        //    while (unfinishedRiversData.Count > 0)
        //    {
        //        List<DirectedPerlinWormData> returnedRivers = new();

        //        foreach(var riverData in unfinishedRiversData)
        //        {
        //            if(!CreateRiver(riverData,minimas, out DirectedPerlinWormData returnedRiver))
        //            {
        //                returnedRivers.Add(returnedRiver);
        //            }
        //        }

        //        unfinishedRiversData.Clear();
        //        unfinishedRiversData.AddRange(returnedRivers);

        //        currentBottomLevel -= 0.05f;
        //        minimas = minimas.FindAll((minima) => _worldGenerator.GetHeightValue(minima.x, minima.y) <= currentBottomLevel);
        //    }
        //}

        //private DirectedPerlinWormData BuildRiverData(Vector2Int startPoint, List<Vector2Int> minimas, float thickness)
        //{
        //    Vector2Int endPoint = minimas.OrderBy(pos => Vector2.Distance(pos, startPoint)).First();

        //    DirectedPerlinWormData wormData = new(startPoint, endPoint, thickness, new LinearThicken(), _riversLength);

        //    return wormData;
        //}

        //private bool CreateRiver(DirectedPerlinWormData riverData, List<Vector2Int> minimas, out DirectedPerlinWormData unfinishedRiverData)
        //{
        //    Vector2Int endPoint = minimas.OrderBy(pos => Vector2.Distance(pos, riverData.Position)).First();

        //    PerlinWorms perlinWorms = new(new(_noise));

        //    riverData.ChangeEndPoint(endPoint);
        //    riverData.SetThickness(riverData.Thickness + _thicknessStep);

        //    var river = perlinWorms.CreateWorm(riverData);
        //    AddRiver(river);

        //    if (_worldGenerator.GetHeightValue(endPoint.x, endPoint.y) < _worldGenerator.WaterLevel)
        //    {
        //        unfinishedRiverData = null;
        //        return true;
        //    }

        //    unfinishedRiverData = riverData;
        //    return false;
        //}


        public void GenerateRivers()
        {
            var startMaximas = FindLocalMaximas(_parameters.MaximasButtom, 1f);
            var startMinimas = FindLocalMinimas(0f, _parameters.MinimasTop);

            UnityEngine.Random.InitState(_parameters.Seed);

            List<Vector2Int> selectedMaximas = new();
            int maximasToSelect = startMaximas.Count / 2;
            while (selectedMaximas.Count < maximasToSelect)
            {
                int randomIndex = UnityEngine.Random.Range(0, startMaximas.Count);
                if (!selectedMaximas.Contains(startMaximas[randomIndex]))
                {
                    selectedMaximas.Add(startMaximas[randomIndex]);
                }
            }

            List<Vector2Int> selectedMinimas = new();
            int minimasToSelect = startMinimas.Count / 2;
            while (selectedMinimas.Count < minimasToSelect)
            {
                int randomIndex = UnityEngine.Random.Range(0, startMinimas.Count);
                if (!selectedMinimas.Contains(startMinimas[randomIndex]))
                {
                    selectedMinimas.Add(startMinimas[randomIndex]);
                }
            }

            List<WormSegment> unfinishedRivers = new();
            foreach (var maxima in selectedMaximas)
            {
                Vector2Int endPoint = selectedMinimas.OrderBy(pos => Vector2.Distance(pos, maxima)).First();

                //thikness step = 0.4f
                if (CreateRiver(maxima, endPoint, Vector2.up, 0.1f, 0.5f, out WormSegment? lastSegment) == false)
                {
                    unfinishedRivers.Add(lastSegment.Value);
                }
            }

            float minimaStep = 0.1f;
            float thicknessStep = 0.4f;
            float currentMinimaTop = _parameters.MinimasTop - minimaStep;

            List<Vector2Int> minimas = FindLocalMinimas(0, currentMinimaTop);

            while (unfinishedRivers.Count > 0 && minimas.Count > 0)
            {
                List<WormSegment> returnedRivers = new List<WormSegment>();

                unfinishedRivers = MergeSegments(unfinishedRivers);

                foreach (var item in unfinishedRivers)
                {
                    Vector2Int startPoint = new(Mathf.RoundToInt(item.Position.x), Mathf.RoundToInt(item.Position.y));

                    var admissibleMinimas = FilterPoints(minimas, startPoint, item.Direction);

                    Vector2Int endPoint;
                    if (admissibleMinimas.Count > 0)
                    {
                        Debug.Log("Selected admissible minima");
                        endPoint = admissibleMinimas.First();
                    }
                    else 
                    {
                        Debug.Log("No admissible minimas");
                        endPoint = minimas.OrderBy(pos => Vector2.Distance(pos, item.Position)).First();
                    }

                    if (CreateRiver(startPoint, endPoint, item.Direction, item.Thickness, item.Thickness + thicknessStep, out WormSegment? lastSegment) == false)
                    {
                        returnedRivers.Add(lastSegment.Value);
                    }
                }

                currentMinimaTop -= minimaStep;
                unfinishedRivers.Clear();
                unfinishedRivers.AddRange(returnedRivers);
                minimas = FindLocalMinimas(0, currentMinimaTop);
            }
        }

        private List<WormSegment> MergeSegments(List<WormSegment> segments)
        {
            var mergedObjects = segments
                .GroupBy(obj => new Vector2(Mathf.RoundToInt(obj.Position.x), Mathf.RoundToInt(obj.Position.y)))
                .Select(group =>
                {
                    var mergedPosition = group.Key;
                    var mergedDirection = Vector2.zero;
                    var mergedThickness = 0f;

                    foreach (var obj in group)
                    {
                        mergedDirection += obj.Direction;
                        mergedThickness += obj.Thickness;
                    }

                    return new WormSegment(mergedPosition, mergedDirection.normalized, mergedThickness);
                })
                .ToList();

            return mergedObjects;
        }

        private List<Vector2Int> FilterPoints(List<Vector2Int> points, Vector2Int mainPoint, Vector2 direction)
        {
            float maxAngleDeviation = 60f; // Максимальное отклонение угла (в градусах)
            float maxDistance = 64f; // Максимальное расстояние

            List<Vector2Int> filteredPoints = new List<Vector2Int>();

            direction.Normalize();

            foreach (Vector2Int point in points)
            {
                Vector2 toPoint = point - mainPoint;
                float angle = Vector2.Angle(direction, toPoint);

                if (angle <= maxAngleDeviation && toPoint.magnitude <= maxDistance)
                {
                    filteredPoints.Add(point);
                }
            }

            return filteredPoints;

        }

        private bool CreateRiver(Vector2Int start, Vector2Int end, Vector2 direction, float minThickness, float maxThickness, out WormSegment? lastSegment)
        {
            DirectedPerlinWormData wormData = new(start, end, minThickness, maxThickness);
            wormData.SetDirection(direction);
            var river = PerlinWorms.CreateWorm(wormData);
            AddRiver(river);

            float endHeight = _parameters.WorldGenerator.GetHeightValue(Mathf.RoundToInt(end.x), Mathf.RoundToInt(end.y));

            if (endHeight < _parameters.WorldGenerator.WaterLevel)
            {
                lastSegment = null;
                return true;
            }

            lastSegment = river.Last();

            return false;
        }

        private void AddRiver(List<WormSegment> river)
        {
            foreach (var riverSegment in river)
            {
                int x = Mathf.RoundToInt(riverSegment.Position.x);
                int y = Mathf.RoundToInt(riverSegment.Position.y);

                if (CheckEntryIntoMap(x, y) == false)
                    continue;

                RiverMap[x, y] = 1f;
                ThickenRiverPosition(x, y, riverSegment.Thickness);
            }
        }

        private void ThickenRiverPosition(int x, int y, float thickness)
        {
            Vector2Int center = new Vector2Int(x, y);

            Vector2Int[] directions = GetDirectionsInRadius(thickness);

            foreach (var direction in directions)
            {
                Vector2Int position = center + direction;

                if (CheckEntryIntoMap(position) == false)
                    continue;

                RiverMap[position.x, position.y] = 1f;
            }
        }

        public List<Vector2Int> FindLocalMaximas(float buttom, float top)
        {
            List<Vector2Int> maximas = new List<Vector2Int>();

            for (int i = 0; i < _parameters.WorldWidth; i++)
            {
                for (int j = 0; j < _parameters.WorldHeight; j++)
                {
                    float height = _parameters.WorldGenerator.GetHeightValue(i, j);

                    if (height < buttom || height > top)
                        continue;

                    if (CheckNeighbours(i, j, (neighbourHeight) => neighbourHeight > height))
                        maximas.Add(new(i, j));
                }
            }

            return maximas;
        }

        public List<Vector2Int> FindLocalMinimas(float buttom, float top)
        {
            List<Vector2Int> minimas = new List<Vector2Int>();

            for (int i = 0; i < _parameters.WorldWidth; i++)
            {
                for (int j = 0; j < _parameters.WorldHeight; j++)
                {
                    float height = _parameters.WorldGenerator.GetHeightValue(i, j);

                    if (height < buttom || height > top)
                        continue;

                    if (CheckNeighbours(i, j, (neighbourHeight) => neighbourHeight < height))
                        minimas.Add(new(i, j));
                }
            }

            return minimas;
        }

        private bool CheckNeighbours(int x, int y, Func<float, bool> failCondition)
        {
            Vector2Int[] directions = GetDirectionsInRadius(_parameters.Radius);

            foreach (var direction in directions)
            {
                Vector2Int neighbourPosition = new Vector2Int(x, y) + direction;

                if (CheckEntryIntoMap(neighbourPosition) == false)
                    continue;

                float neighbourHeight = _parameters.WorldGenerator.GetHeightValue(neighbourPosition.x, neighbourPosition.y);

                if (failCondition(neighbourHeight))
                    return false;
            }

            return true;
        }

        private bool CheckEntryIntoMap(Vector2Int position)
        {
            if (position.x < 0 ||
                position.x >= _parameters.WorldWidth ||
                position.y < 0 ||
                position.y >= _parameters.WorldHeight)
                return false;

            return true;
        }

        private bool CheckEntryIntoMap(float x, float y)
        {
            if (x < 0 ||
                x >= _parameters.WorldWidth ||
                y < 0 ||
                y >= _parameters.WorldHeight)
                return false;

            return true;
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

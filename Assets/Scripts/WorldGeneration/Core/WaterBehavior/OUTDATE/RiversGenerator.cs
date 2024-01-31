using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WorldGeneration.Core.Maps;

namespace WorldGeneration.Core.Outdate
{
	public class RiversGenerator
	{
		public PerlinWorms PerlinWorms;

		private readonly RiversGeneratorParameters _parameters;
		private WaterBehavior _waterBehavior;

		public float[,] RiverMap { get; private set; }
		public float[,] RiverMap2 { get; private set; }

		public List<Vector2Int> riversPoints = new List<Vector2Int>();

		public RiversGenerator(RiversGeneratorParameters parameters)
		{
			_parameters = parameters;

			RiverMap2 = new float[_parameters.WorldWidth, _parameters.WorldHeight];

			_waterBehavior = new(_parameters.WorldGenerator);
		}

		public void FindMaximasAndCreateSources()
		{
			var maximas = FindLocalMaximas(0.8f, 1f);

			foreach (var maxima in maximas)
			{
				_waterBehavior.CreateSource(maxima);
			}
		}

		public void IterateAllSources()
		{
			_waterBehavior.IterateAllSources();

			for (int x = 0; x < RiverMap2.GetLength(0); x++)
			{
				for (int y = 0; y < RiverMap2.GetLength(1); y++)
				{
					//RiverMap2[x,y] = _waterBehavior.RiverMap[x,y] + _waterBehavior.SettledWaterMap[x,y];
					RiverMap2[x, y] = _waterBehavior.RiverMap[x, y];
				}
			}
		}

		public void GenerateRivers2()
		{
			var maximas = FindLocalMaximas(0.8f, 1f);

			WaterBehavior waterBehavior = new(_parameters.WorldGenerator);
			waterBehavior.StartSource(maximas.First());
			RiverMap2 = waterBehavior.RiverMap;
		}

		//public void GenerateRivers()
		//{
		//    var startMaximas = FindLocalMaximas(_parameters.MaximasButtom, 1f);
		//    var startMinimas = FindLocalMinimas(0f, _parameters.MinimasTop);

		//    UnityEngine.Random.InitState(_parameters.Seed);

		//    List<Vector2Int> selectedMaximas = new();
		//    int maximasToSelect = startMaximas.Count / 2;
		//    while (selectedMaximas.Count < maximasToSelect)
		//    {
		//        int randomIndex = UnityEngine.Random.Range(0, startMaximas.Count);
		//        if (!selectedMaximas.Contains(startMaximas[randomIndex]))
		//        {
		//            selectedMaximas.Add(startMaximas[randomIndex]);
		//        }
		//    }

		//    List<Vector2Int> selectedMinimas = new();
		//    int minimasToSelect = startMinimas.Count / 2;
		//    while (selectedMinimas.Count < minimasToSelect)
		//    {
		//        int randomIndex = UnityEngine.Random.Range(0, startMinimas.Count);
		//        if (!selectedMinimas.Contains(startMinimas[randomIndex]))
		//        {
		//            selectedMinimas.Add(startMinimas[randomIndex]);
		//        }
		//    }



		//    List<WormSegment> unfinishedRivers = new();
		//    foreach (var maxima in selectedMaximas)
		//    {
		//        Vector2Int endPoint = selectedMinimas.OrderBy(pos => Vector2.Distance(pos, maxima)).First();

		//        //thikness step = 0.4f
		//        if (CreateRiver(maxima, endPoint, Vector2.up, 0.1f, 0.5f, out WormSegment? lastSegment) == false)
		//        {
		//            unfinishedRivers.Add(lastSegment.Value);
		//        }
		//    }

		//    float minimaStep = 0.1f;
		//    float thicknessStep = 0.8f;
		//    BoundedValue<float> currentMinimaTop = new(_parameters.MinimasTop - minimaStep, _parameters.WorldGenerator.WaterLevel, _parameters.MinimasTop);

		//    List<Vector2Int> minimas = FindLocalMinimas(0, currentMinimaTop.Value);

		//    int counter = 0;

		//    while (unfinishedRivers.Count > 0 && minimas.Count > 0)
		//    {
		//        if(counter > 64)
		//        {
		//            Debug.Log("!!!!!");
		//            break;
		//        }


		//        List<WormSegment> returnedRivers = new List<WormSegment>();

		//        unfinishedRivers = MergeSegments(unfinishedRivers);

		//        foreach (var item in unfinishedRivers)
		//        {
		//            Vector2Int startPoint = new(Mathf.RoundToInt(item.Position.x), Mathf.RoundToInt(item.Position.y));

		//            var admissibleMinimas = FilterPoints(minimas, startPoint, item.Direction);

		//            Vector2Int endPoint;
		//            if (admissibleMinimas.Count > 0)
		//            {
		//                Debug.Log("Selected admissible minima");
		//                endPoint = admissibleMinimas.First();
		//            }
		//            else
		//            {
		//                Debug.Log("No admissible minimas");
		//                endPoint = minimas.OrderBy(pos => Vector2.Distance(pos, item.Position)).First();
		//            }

		//            if (CreateRiver(startPoint, endPoint, item.Direction, item.Thickness, item.Thickness + thicknessStep, out WormSegment? lastSegment) == false)
		//            {
		//                returnedRivers.Add(lastSegment.Value);
		//            }
		//        }

		//        currentMinimaTop.Value -= minimaStep;
		//        unfinishedRivers.Clear();
		//        unfinishedRivers.AddRange(returnedRivers);
		//        minimas = FindLocalMinimas(0, currentMinimaTop.Value);

		//        counter++;
		//    }
		//}

		//public void GenerateRivers1()
		//{
		//    var sources = CreateSources(1, _parameters.WorldGenerator.WaterLevel + 0.2f);

		//    for (int i = 0; i < 64; i++)
		//    {
		//        for (int j = 0; j < sources.Count; j++)
		//        {
		//            if (i == sources.Count /2)
		//                IterateSorce(sources[j], true);
		//            else
		//                IterateSorce(sources[j], false);
		//        }
		//    }

		//}

		//private void IterateSorce(RiverSource source, bool flag)
		//{
		//    RiverMap[source.Position.x, source.Position.y] += source.Strength;

		//    int x = source.Position.x;
		//    int y = source.Position.y;

		//    int i = 0;

		//    if(flag)
		//        Debug.Log("===============================");

		//    while (Split(x, y, flag, out int nextX, out int nextY))
		//    {
		//        if (i > 256)
		//        {
		//            Debug.LogWarning("256 splittings");
		//            break;
		//        }

		//        x = nextX;
		//        y = nextY;
		//        i++;
		//    }

		//    //for (int x = 0; x < _parameters.WorldWidth; x++)
		//    //{
		//    //    for (int y = 0; y < _parameters.WorldHeight; y++)
		//    //    {

		//    //    }
		//    //}
		//}

		//private bool Split(int x, int y, bool flag, out int xSplited, out int ySplited)
		//{
		//    if (x < 1
		//        || x > _parameters.WorldWidth - 1
		//        || y < 1
		//        || y > _parameters.WorldHeight - 1)
		//    {
		//        RiverMap[x, y] = 0f;
		//        xSplited = -1;
		//        ySplited = -1;

		//        if (flag)
		//            Debug.Log("1");

		//        return false;
		//    }


		//    Vector2Int lowestNeighbour = FindLowestNeighbour(x, y);

		//    float currentHeightRaw = _parameters.WorldGenerator.GetHeightValue(x, y);
		//    float neighbourHeightRaw = _parameters.WorldGenerator.GetHeightValue(lowestNeighbour.x, lowestNeighbour.y);

		//    if (neighbourHeightRaw <= _parameters.WorldGenerator.WaterLevel)
		//    {
		//        RiverMap[x, y] = 0f;
		//        xSplited = -1;
		//        ySplited = -1;

		//        if (flag)
		//            Debug.Log("2");

		//        return false;
		//    }

		//    if (lowestNeighbour.x < 1
		//        || lowestNeighbour.x > _parameters.WorldWidth - 1
		//        || lowestNeighbour.y < 1
		//        || lowestNeighbour.y > _parameters.WorldHeight - 1)
		//    {
		//        RiverMap[x, y] = 0f;
		//        xSplited = -1;
		//        ySplited = -1;

		//        if (flag)
		//            Debug.Log("3");

		//        return false;
		//    }

		//    float neighbourHeight = neighbourHeightRaw + RiverMap[lowestNeighbour.x, lowestNeighbour.y];
		//    float currentHeight = currentHeightRaw + RiverMap[x, y];

		//    if (currentHeight < neighbourHeight)
		//    {
		//        if(RiverMap[lowestNeighbour.x, lowestNeighbour.y] > 0.1f)
		//        {
		//            xSplited = lowestNeighbour.x;
		//            ySplited = lowestNeighbour.y;

		//            return true;
		//        }
		//        else
		//        {
		//            xSplited = -1;
		//            ySplited = -1;

		//            return false;
		//        }

		//    }


		//    //float waterToSplit = RiverMap[x, y] - RiverMap[lowestNeighbour.x, lowestNeighbour.y];
		//    float waterToSplit;

		//    if (currentHeightRaw <= neighbourHeight)
		//        waterToSplit = currentHeight - neighbourHeight / 2f;
		//    else
		//        waterToSplit = RiverMap[x, y] - 0.1f;

		//    waterToSplit = Mathf.Clamp(waterToSplit, 0f, RiverMap[x, y] - 0.1f);

		//    if(waterToSplit < 0.01f)
		//    {
		//        xSplited = -1;
		//        ySplited = -1;

		//        return false;
		//    }


		//    if (flag)
		//        Debug.Log($"{RiverMap[x, y]} - {waterToSplit}\t {RiverMap[lowestNeighbour.x, lowestNeighbour.y]} + {waterToSplit}");


		//    RiverMap[x, y] -= waterToSplit;
		//    RiverMap[lowestNeighbour.x, lowestNeighbour.y] += waterToSplit;
		//    xSplited = lowestNeighbour.x;
		//    ySplited = lowestNeighbour.y;


		//    return true;
		//}

		//private Vector2Int FindLowestNeighbour(int x, int y)
		//{
		//    var directions = GetDirectionsInRadius(1);

		//    List<Vector2Int> neighbours = new List<Vector2Int>();
		//    foreach (var direction in directions)
		//    {
		//        neighbours.Add(new Vector2Int(x, y) + direction);
		//    }

		//    return neighbours.OrderBy((pos) => _parameters.WorldGenerator.GetHeightValue(pos.x, pos.y)).First();
		//}

		//private List<RiverSource> CreateSources(float top, float bottom)
		//{
		//    float sourseChanse = 40f;
		//    float sourceStrength = 0.5f;

		//    var maximas = FindLocalMaximas(bottom, top);
		//    List<Vector2Int> selectedMaximas = new List<Vector2Int>();

		//    UnityEngine.Random.InitState(_parameters.Seed);
		//    foreach (var maxima in maximas)
		//    {
		//        var chanse = UnityEngine.Random.Range(0, 100);

		//        if (chanse <= sourseChanse)
		//        {
		//            selectedMaximas.Add(maxima);
		//        }
		//    }

		//    List<RiverSource> sources = new List<RiverSource>();
		//    foreach (var maxima in selectedMaximas)
		//    {
		//        sources.Add(new RiverSource(maxima, Vector2.zero, _parameters.WorldGenerator.GetHeightValue(maxima.x, maxima.y), sourceStrength));
		//    }

		//    return sources;
		//}

		//private List<WormSegment> MergeSegments(List<WormSegment> segments)
		//{
		//    var mergedObjects = segments
		//        .GroupBy(obj => new Vector2(Mathf.RoundToInt(obj.Position.x), Mathf.RoundToInt(obj.Position.y)))
		//        .Select(group =>
		//        {
		//            var mergedPosition = group.Key;
		//            var mergedDirection = Vector2.zero;
		//            var mergedThickness = 0f;

		//            foreach (var obj in group)
		//            {
		//                mergedDirection += obj.Direction;
		//                mergedThickness += obj.Thickness;
		//            }

		//            return new WormSegment(mergedPosition, mergedDirection.normalized, mergedThickness);
		//        })
		//        .ToList();

		//    return mergedObjects;
		//}

		//private List<Vector2Int> FilterPoints(List<Vector2Int> points, Vector2Int mainPoint, Vector2 direction)
		//{
		//    float maxAngleDeviation = 60f; // Максимальное отклонение угла (в градусах)
		//    float maxDistance = 64f; // Максимальное расстояние

		//    List<Vector2Int> filteredPoints = new List<Vector2Int>();

		//    direction.Normalize();

		//    foreach (Vector2Int point in points)
		//    {
		//        Vector2 toPoint = point - mainPoint;
		//        float angle = Vector2.Angle(direction, toPoint);

		//        if (angle <= maxAngleDeviation && toPoint.magnitude <= maxDistance)
		//        {
		//            filteredPoints.Add(point);
		//        }
		//    }

		//    return filteredPoints.FindAll((point) => Vector2.Distance(point, mainPoint) > 4);

		//}

		//private bool CreateRiver(Vector2Int start, Vector2Int end, Vector2 direction, float minThickness, float maxThickness, out WormSegment? lastSegment)
		//{
		//    riversPoints.Add(start);
		//    riversPoints.Add(end);

		//    DirectedPerlinWormData wormData = new(start, end, new BoundedValue<float>(minThickness, minThickness, maxThickness));
		//    wormData.SetDirection(direction);
		//    var river = PerlinWorms.CreateWorm(wormData);

		//    if (river.Count <= 0)
		//    {
		//        Debug.LogWarning($"Returned empty worm: {start} - {end}");
		//        lastSegment = null;
		//        return true;
		//    }

		//    if (AddRiver(river) == false)
		//    {
		//        lastSegment = null;
		//        return true;
		//    }

		//    lastSegment = river.Last();

		//    if (lastSegment.Value.Position.x < 1
		//        || lastSegment.Value.Position.x > _parameters.WorldWidth - 1
		//        || lastSegment.Value.Position.y < 1
		//        || lastSegment.Value.Position.y > _parameters.WorldHeight - 1)
		//    {
		//        lastSegment = null;
		//        return true;
		//    }

		//    float endHeight = _parameters.WorldGenerator.GetHeightValue(Mathf.RoundToInt(lastSegment.Value.Position.x), Mathf.RoundToInt(lastSegment.Value.Position.y));

		//    if (endHeight <= _parameters.WorldGenerator.WaterLevel)
		//    {
		//        lastSegment = null;
		//        return true;
		//    }


		//    return false;
		//}

		//private bool AddRiver(List<WormSegment> river)
		//{
		//    foreach (var riverSegment in river)
		//    {
		//        int x = Mathf.RoundToInt(riverSegment.Position.x);
		//        int y = Mathf.RoundToInt(riverSegment.Position.y);

		//        if (CheckEntryIntoMap(x, y) == false)
		//            continue;

		//        if (_parameters.WorldGenerator.GetHeightValue(x, y) <= _parameters.WorldGenerator.WaterLevel)
		//        {
		//            return false;
		//        }

		//        RiverMap[x, y] = 1f;
		//        ThickenRiverPosition(x, y, riverSegment.Thickness);
		//    }

		//    return true;
		//}

		//private void ThickenRiverPosition(int x, int y, float thickness)
		//{
		//    Vector2Int center = new Vector2Int(x, y);

		//    Vector2Int[] directions = GetDirectionsInRadius(thickness);

		//    foreach (var direction in directions)
		//    {
		//        Vector2Int position = center + direction;

		//        if (CheckEntryIntoMap(position) == false)
		//            continue;

		//        RiverMap[position.x, position.y] = 1f;
		//    }
		//}

		public List<Vector2Int> FindLocalMaximas(float buttom, float top)
		{
			List<Vector2Int> maximas = new List<Vector2Int>();

			for (int i = 0; i < _parameters.WorldWidth; i++)
			{
				for (int j = 0; j < _parameters.WorldHeight; j++)
				{
					float height = _parameters.WorldGenerator.GetMapValue(new(i, j), MapValueType.Height);

					if (height < buttom || height > top)
						continue;

					if (CheckNeighbours(i, j, (neighbourHeight) => neighbourHeight > height))
						maximas.Add(new Vector2Int(i, j));
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
					float height = _parameters.WorldGenerator.GetMapValue(new(i, j), MapValueType.Height);

					if (height < buttom || height > top)
						continue;

					if (CheckNeighbours(i, j, (neighbourHeight) => neighbourHeight < height))
						minimas.Add(new Vector2Int(i, j));
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

				float neighbourHeight = _parameters.WorldGenerator.GetMapValue(new(neighbourPosition.x, neighbourPosition.y), MapValueType.Height);

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

	public struct RiverSource
	{
		public Vector2Int Position { get; set; }
		public Vector2 Direction { get; set; }
		public float Height { get; set; }
		public float Strength { get; set; }

		public RiverSource(Vector2Int position, Vector2 direction, float height, float strength)
		{
			Position = position;
			Direction = direction;
			Height = height;
			Strength = strength;
		}
	}
}

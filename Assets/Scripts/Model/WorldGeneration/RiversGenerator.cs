using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Model.WorldGeneration
{
    public class RiversGenerator
    {
        private OctaveNoiseParameters _noise;
        private WorldGenerator _worldGenerator;

        private int _riversLength = 512;


        public float[,] RiverMap { get; private set; }

        public RiversGenerator(WorldGenerator worldGenerator, OctaveNoiseParameters noise)
        {
            _noise = noise;
            _worldGenerator = worldGenerator;

            RiverMap = new float[_worldGenerator.WorldWidth, _worldGenerator.WorldHeight];
        }

        public void GenerateRivers()
        {
            var maximas = FindLocalMaximas();

            var minimas = FindLocalMinimas();

            HashSet<Vector2Int> riversStart = maximas.ToHashSet();

            //List<Vector2Int> riversStart = maximas;
            float currentBottomLevel = 0.8f;

            while (riversStart.Count > 0 && minimas.Count > 0)
            {
                HashSet<Vector2Int> unfinishedRivers = new();

                foreach (var riverStart in riversStart)
                {
                    var finish = CreateRiver(riverStart, minimas);

                    if (finish != null)
                        unfinishedRivers.Add(finish.Value);
                }

                riversStart.Clear();

                foreach (var river in unfinishedRivers)
                {
                    riversStart.Add(river);
                }

                currentBottomLevel -= 0.1f;

                minimas = minimas.FindAll((minima) => _worldGenerator.GetHeightValue(minima.x, minima.y) <= currentBottomLevel);
            }
        }

        private Vector2Int? CreateRiver(Vector2Int startPoint, List<Vector2Int> minimas)
        {
            PerlinWorms perlinWorms;

            Vector2Int endPoint;

            endPoint = minimas.OrderBy(pos => Vector2.Distance(pos, startPoint)).First();
            perlinWorms = new(new(_noise));

            DirectedPerlinWormData wormData = new(startPoint, endPoint, _riversLength);
            var river = perlinWorms.CreateWorm(wormData);

            AddRiver(river);

            if (_worldGenerator.GetHeightValue(endPoint.x, endPoint.y) < _worldGenerator.WaterLevel)
                return null;

            return endPoint;
        }

        private void AddRiver(List<WormSegment> river)
        {
            foreach (var riverTile in river)
            {
                if (riverTile.Position.x < 0 || riverTile.Position.x >= _worldGenerator.WorldWidth || riverTile.Position.y < 0 || riverTile.Position.y >= _worldGenerator.WorldHeight)
                    break;

                RiverMap[Mathf.FloorToInt(riverTile.Position.x), Mathf.FloorToInt(riverTile.Position.y)] = 1f;
            }
        }

        private void AddRiver(List<Vector2Int> river)
        {
            foreach (var riverTile in river)
            {
                if (riverTile.x < 0 || riverTile.x >= _worldGenerator.WorldWidth || riverTile.y < 0 || riverTile.y >= _worldGenerator.WorldHeight)
                    break;

                RiverMap[riverTile.x, riverTile.y] = 1f;
            }
        }

        public List<Vector2Int> FindLocalMaximas()
        {
            List<Vector2Int> maximas = new List<Vector2Int>();

            for (int i = 0; i < _worldGenerator.WorldWidth; i++)
            {
                for (int j = 0; j < _worldGenerator.WorldHeight; j++)
                {
                    float height = _worldGenerator.GetHeightValue(i, j);

                    if (height < 0.8f)
                        continue;

                    if (CheckNeighbours(i, j, (neighbourHeight) => neighbourHeight > height))
                        maximas.Add(new(i, j));
                }
            }

            return maximas;
        }

        public List<Vector2Int> FindLocalMinimas()
        {
            List<Vector2Int> minimas = new List<Vector2Int>();

            for (int i = 0; i < _worldGenerator.WorldWidth; i++)
            {
                for (int j = 0; j < _worldGenerator.WorldHeight; j++)
                {
                    float height = _worldGenerator.GetHeightValue(i, j);

                    if (height > 0.8f)
                        continue;

                    if (CheckNeighbours(i, j, (neighbourHeight) => neighbourHeight < height))
                        minimas.Add(new(i, j));
                }
            }

            return minimas;
        }

        private bool CheckNeighbours(int x, int y, Func<float, bool> failCondition)
        {
            foreach (var direction in _directionsBig)
            {
                Vector2Int neighbourPos = new Vector2Int(x, y) + direction;

                if (neighbourPos.x > _worldGenerator.WorldWidth || neighbourPos.x < 0 || neighbourPos.y > _worldGenerator.WorldHeight || neighbourPos.y < 0)
                    continue;

                float neighbourHeight = _worldGenerator.GetHeightValue(neighbourPos.x, neighbourPos.y);

                if (failCondition(neighbourHeight))
                    return false;
            }

            return true;
        }

        private static Vector2Int[] _directions = new Vector2Int[]
        {
        new Vector2Int( 0, 1), //N
        new Vector2Int( 1, 1), //NE
        new Vector2Int( 1, 0), //E
        new Vector2Int(-1, 1), //SE
        new Vector2Int(-1, 0), //S
        new Vector2Int(-1,-1), //SW
        new Vector2Int( 0,-1), //W
        new Vector2Int( 1,-1),  //NW
        ////
        //new Vector2Int( 0, 2), //N
        //new Vector2Int( 2, 2), //NE
        //new Vector2Int( 2, 0), //E
        //new Vector2Int(-2, 2), //SE
        //new Vector2Int(-2, 0), //S
        //new Vector2Int(-2,-2), //SW
        //new Vector2Int( 0,-2), //W
        //new Vector2Int( 2,-2),  //NW
        ////
        //new Vector2Int(-1,-2),
        //new Vector2Int(1,-2),
        //new Vector2Int(2,-1),
        //new Vector2Int(2,1),
        //new Vector2Int(1,2),
        //new Vector2Int(-1,2),
        //new Vector2Int(-2,1),
        //new Vector2Int(-2,-1),
        };

        private static Vector2Int[] _directionsBig = new Vector2Int[]
        {
        new Vector2Int( 0, 1), //N
        new Vector2Int( 1, 1), //NE
        new Vector2Int( 1, 0), //E
        new Vector2Int(-1, 1), //SE
        new Vector2Int(-1, 0), //S
        new Vector2Int(-1,-1), //SW
        new Vector2Int( 0,-1), //W
        new Vector2Int( 1,-1),  //NW
        //
        new Vector2Int( 0, 2), //N
        new Vector2Int( 2, 2), //NE
        new Vector2Int( 2, 0), //E
        new Vector2Int(-2, 2), //SE
        new Vector2Int(-2, 0), //S
        new Vector2Int(-2,-2), //SW
        new Vector2Int( 0,-2), //W
        new Vector2Int( 2,-2),  //NW
        //
        new Vector2Int(-1,-2),
        new Vector2Int(1,-2),
        new Vector2Int(2,-1),
        new Vector2Int(2,1),
        new Vector2Int(1,2),
        new Vector2Int(-1,2),
        new Vector2Int(-2,1),
        new Vector2Int(-2,-1),
        };

    }
}

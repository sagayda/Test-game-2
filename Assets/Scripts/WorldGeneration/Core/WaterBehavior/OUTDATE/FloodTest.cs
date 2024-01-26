using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

namespace WorldGeneration.Core.Outdate
{
    public class FloodTest : MonoBehaviour
    {
        [HideInInspector] public float[,] heightsMap;
        [HideInInspector] public float[,] waterMap;
        [HideInInspector] public bool[,] lastVisited;

        private float minWater = 1f;
        private float ballanceLevel = 0.1f;

        public int DropX;
        public int DropY;
        public int DropAmount;

        private void Awake()
        {
            heightsMap = CreateHeightsArray();
            waterMap = new float[heightsMap.GetLength(0), heightsMap.GetLength(1)];
        }

        [Button("Create water drop")]
        private void OnDropButtonClick()
        {
            Drop(DropX, DropY, DropAmount);
        }

        [Button("Cascade iteration")]
        private void OnCascadeButtonClick()
        {
            Cascade(DropX, DropY);
        }

        [Button("Check water")]
        private void OnWaterAmountCheckButtonClick()
        {
            float res = 0;

            foreach (var item in waterMap)
            {
                res += item;
            }

            Debug.Log(res);
        }

        [Button("Full cascade")]
        private void OnCascadeUntilBalancedButtonClick()
        {
            CascadeUntilBalanced(DropX, DropY);
        }

        public void Drop(int x, int y, float amount)
        {
            waterMap[x, y] += amount;
        }

        public void CascadeUntilBalanced(int xStart, int yStart)
        {
            while (Cascade(xStart, yStart) == false) ;

            Debug.Log("Water ballanced");
        }

        public bool Cascade(int xStart, int yStart)
        {
            bool[,] visited = new bool[waterMap.GetLength(0), waterMap.GetLength(1)];

            List<Vector2Int> toVisit = new List<Vector2Int>
            {
                new(xStart, yStart)
            };

            int counter = 0;

            while (toVisit.Count() > 0)
            {
                List<Vector2Int> affected = new List<Vector2Int>();

                foreach (var cellToVisit in toVisit)
                {
                    affected.AddRange(CascadeStep(cellToVisit.x, cellToVisit.y, visited));
                }

                toVisit = affected;

                if(counter > 256)
                {
                    Debug.Log("Infinitive cycle");
                    break;
                }

                counter++;
            }

            lastVisited = visited;

            if (counter <= 1)
                return true;

            return false;
        }

        public List<Vector2Int> CascadeStep(int x, int y, bool[,] visited)
        {
            List<Cell> cellsToBallance = new List<Cell>()
            {
                new(x,y, heightsMap[x,y], waterMap[x,y]),
            };

            //add neighbours
            foreach (var direction in _directions)
            {
                if (PrepareCascade(x + direction.x, y + direction.y, visited, out Cell cell) == false)
                {
                    continue;
                }

                cellsToBallance.Add(cell);
            }

            var orderedCells = cellsToBallance.OrderByDescending((cell) => cell.Height).ToList();
            
            float waterToBallance = CalculateWaterForBallance(orderedCells);

            //if water is not enough
            while (waterToBallance <= 0 && orderedCells.Count > 1)
            {
                orderedCells.RemoveAt(0);
                waterToBallance = CalculateWaterForBallance(orderedCells);
            }

            if (orderedCells.Count <= 0)
            {
                return new List<Vector2Int>();
            }

            //cells are ballanced
            if(IsEnoughBalanced(orderedCells))
            {
                return new List<Vector2Int>();
            }

            float desiredLevel = orderedCells.Max((elem) => elem.Height) + (waterToBallance / orderedCells.Count());

            //setting water
            foreach (var cell in orderedCells)
            {
                float waterToSet = desiredLevel - heightsMap[cell.X, cell.Y];

                waterMap[cell.X, cell.Y] = waterToSet;
            }

            visited[x, y] = true;

            List<Vector2Int> affectedCells = new List<Vector2Int>();

            foreach (var cell in orderedCells)
            {
                if (cell.X == x && cell.Y == y)
                    continue;

                affectedCells.Add(new(cell.X, cell.Y));
            }


            return affectedCells;

            ////top
            //int xTop = x;
            //int yTop = y + 1;
            //CascadeFromTo(x, y, xTop, yTop, visited);

            ////left
            //int xLeft = x - 1;
            //int yLeft = y;
            //CascadeFromTo(x, y, xLeft, yLeft, visited);

            ////right
            //int xRight = x + 1;
            //int yRight = y;
            //CascadeFromTo(x, y, xRight, yRight, visited);

            ////buttom
            //int xButtom = x;
            //int yButtom = y - 1;
            //CascadeFromTo(x, y, xButtom, yButtom, visited);

            //visited[x, y] = true;
        }

        private bool PrepareCascade(int x, int y, bool[,] visited, out Cell cell)
        {
            cell = null;

            if (CheckCoords(x, y) == false)
            {
                return false;
            }

            if (visited[x, y])
            {
                return false;
            }

            float water = waterMap[x, y];
            float height = heightsMap[x, y];

            cell = new(x, y, height, water);

            return true;
        }

        private bool IsEnoughBalanced(List<Cell> cells)
        {
            if (cells.Count <= 1)
                return true;

            float maxDifference = cells.Max((cell) => cell.Height + cell.Water) - cells.Min((cell) => cell.Height + cell.Water);

            return maxDifference <= ballanceLevel;
        }

        private float CalculateWaterForBallance(List<Cell> cells)
        {
            //var topCell = cells.First();
            var topCellHeight = cells.Max((cell) => cell.Height);

            float waterToBallance = 0;

            foreach (Cell cell in cells)
            {
                waterToBallance += cell.Water + cell.Height - topCellHeight;
            }

            return waterToBallance;
        }

        private bool CheckCoords(int x, int y)
        {
            if (x < 0 || y < 0 || x >= waterMap.GetLength(0) || y >= waterMap.GetLength(1))
                return false;

            return true;
        }

        private static float[,] CreateHeightsArray()
        {
            int size = 64;

            float[,] array = new float[size, size];
            int center = size / 2;
            int maxDistance = center;

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    int distanceToCenter = Mathf.Abs(i - center) + Mathf.Abs(j - center);
                    array[i, j] = Mathf.Lerp(0f, 100f, Mathf.InverseLerp(0, maxDistance, distanceToCenter));
                }
            }

            return array;


            //int rows = 16;
            //int cols = 16;

            //float[,] array = new float[rows, cols];

            //// Initialize border elements to 1
            //for (int i = 0; i < rows; i++)
            //{
            //    array[i, 0] = 100;
            //    array[i, cols - 1] = 100;
            //}

            //for (int j = 1; j < cols - 1; j++)
            //{
            //    array[0, j] = 100;
            //    array[rows - 1, j] = 100;
            //}

            ////// Initialize interior elements randomly as 0 or 1
            ////System.Random random = new System.Random();

            ////for (int i = 1; i < rows - 1; i++)
            ////{
            ////    for (int j = 1; j < cols - 1; j++)
            ////    {
            ////        array[i, j] = random.Next(20); // Generates either 0 or 1
            ////    }
            ////}

            //return array;
        }

        private static Vector2Int[] _directions =
{
            new Vector2Int(1,0),
            new Vector2Int(-1,0),
            new Vector2Int(0,1),
            new Vector2Int(0,-1),
        };
    }

    public class Cell
    {
        public int X;
        public int Y;

        public float Height;
        public float Water;

        public Cell(int x, int y, float height, float water)
        {
            X = x;
            Y = y;
            Height = height;
            Water = water;
        }
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

namespace WorldGeneration.Core.Locations
{
    [Serializable]
    public abstract class Location
    {
        public abstract int Id { get; }
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract Color Color { get; }

        public int X { get; }
        public int Y { get; }

        #region TESTING
        public float Noise { get; set; }
        #endregion

        public Sublocation[,] Sublocations { get; protected set; }

        public List<LocationConnector> Connectors { get; protected set; } = new List<LocationConnector>();

        public Location(int x, int y)
        {
            Sublocations = new Sublocation[10,10];

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Sublocations[i,j] = new Sublocation_Default();
                }
            } 

            X = x;
            Y = y;
        }

        public Location(int x, int y, Sublocation[,] sublocations)
        {
            if(sublocations == null)
                throw new ArgumentNullException(nameof(sublocations));

            if (sublocations.GetLength(0) != 10 || sublocations.GetLength(1) != 10)
                throw new ArgumentException("Invalid sublocation array size!");

            Sublocations = sublocations;

            X = x;
            Y = y;
        }
    }
}

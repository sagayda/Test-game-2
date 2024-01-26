using System;
using UnityEngine;

namespace WorldGeneration.Core.Locations
{
    [Serializable]
    public class Location_Plain : Location
    {
        public override int Id => 1;
        public override string Name => "Plain";
        public override string Description => "Green plain";
        public override Color Color => Color.green;

        public Location_Plain(int x, int y) : base(x, y)
        {
        }

        public Location_Plain(int x, int y, Sublocation[,] sublocations) : base(x, y, sublocations)
        {
        }

    }
}

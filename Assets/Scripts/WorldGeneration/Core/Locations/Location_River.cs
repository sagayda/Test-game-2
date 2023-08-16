﻿using UnityEngine;

namespace WorldGeneration.Core.Locations
{
    public class Location_River : Location
    {
        public override int Id => 3;

        public override string Name => "River";

        public override string Description => "Normal river";

        public override Color Color => new(0.15f, 0.15f, 1f);

        public Location_River(int x, int y) : base(x, y)
        {
        }

        public Location_River(int x, int y, Sublocation[,] sublocations) : base(x, y, sublocations)
        {
        }

    }
}
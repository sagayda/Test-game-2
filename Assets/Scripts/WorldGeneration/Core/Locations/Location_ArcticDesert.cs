using UnityEngine;

namespace WorldGeneration.Core.Locations
{
    public class Location_ArcticDesert : Location
    {
        public override int Id => 8;

        public override string Name => "Arctic desert";

        public override string Description => "Wear a hat!";

        public override Color Color => new(0.9f, 0.9f, 1);

        public Location_ArcticDesert(int x, int y) : base(x, y)
        {
        }

        public Location_ArcticDesert(int x, int y, Sublocation[,] sublocations) : base(x, y, sublocations)
        {
        }

    }
}

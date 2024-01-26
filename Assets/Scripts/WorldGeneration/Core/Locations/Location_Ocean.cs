using UnityEngine;

namespace WorldGeneration.Core.Locations
{
    public class Location_Ocean : Location
    {
        public override int Id => 2;

        public override string Name => "Ocean";

        public override string Description => "Just a ocean";

        public override Color Color => new(0, 0, 0.9f);

        public Location_Ocean(int x, int y) : base(x, y)
        {
        }

        public Location_Ocean(int x, int y, Sublocation[,] sublocations) : base(x, y, sublocations)
        {
        }
    }
}

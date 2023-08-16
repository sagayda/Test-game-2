using UnityEngine;

namespace WorldGeneration.Core.Locations
{ 
    public class Location_Mountains : Location
    {
        public override int Id => 7;

        public override string Name => "Mountains";

        public override string Description => "Its very high!";

        public override Color Color => new(0.45f, 0.15f, 0);

        public Location_Mountains(int x, int y) : base(x, y)
        {
        }

        public Location_Mountains(int x, int y, Sublocation[,] sublocations) : base(x, y, sublocations)
        {
        }

    }
}

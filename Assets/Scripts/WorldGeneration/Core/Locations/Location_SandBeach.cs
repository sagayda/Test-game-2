using UnityEngine;

namespace WorldGeneration.Core.Locations
{
    public class Location_SandBeach : Location
    {
        public override int Id => 5;

        public override string Name => "Sand beach";

        public override string Description => "Peaceful sandy beach";

        public override Color Color => new(1, 1, 0);

        public Location_SandBeach(int x, int y) : base(x, y)
        {
        }

        public Location_SandBeach(int x, int y, Sublocation[,] sublocations) : base(x, y, sublocations)
        {
        }

    }
}

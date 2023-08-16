using UnityEngine;

namespace WorldGeneration.Core.Locations
{
    public class Location_DeepOcean : Location
    {
        public override int Id => 4;

        public override string Name => "Deep ocean";

        public override string Description => "Very deep ocean";

        public override Color Color => new(0, 0, 0.75f);

        public Location_DeepOcean(int x, int y) : base(x, y)
        {
        }

        public Location_DeepOcean(int x, int y, Sublocation[,] sublocations) : base(x, y, sublocations)
        {
        }

    }
}

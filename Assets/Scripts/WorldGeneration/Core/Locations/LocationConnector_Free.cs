using System;

namespace WorldGeneration.Core.Locations
{
    [Serializable]
    public class LocationConnector_Free : LocationConnector
    {
        public override int Id => 0;

        public override string Name => "FreeConnection";

        public override string Description => "Player can move freely";

        public LocationConnector_Free()
        {

        }

        public LocationConnector_Free(Location fromLocation, Location toLocation) : base(fromLocation, toLocation)
        {

        }
    }
}

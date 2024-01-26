using System;

namespace WorldGeneration.Core.Locations
{
    [Serializable]
    public class Sublocation_Default : Sublocation
    {
        public override int Id => 0;
        public override string Name => "Default sublocation";

        public Sublocation_Default()
        {

        }

    }
}

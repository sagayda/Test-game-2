using System;

namespace WorldGeneration.Core.Locations
{
    [Serializable]
    public abstract class Sublocation
    {
        public abstract int Id { get; }

        public abstract string Name { get; }

        public Location ParentLocation { get; }

        public Sublocation()
        {

        }
    }
}

using System;

namespace Assets.Scripts.Model.InGameScripts.World
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

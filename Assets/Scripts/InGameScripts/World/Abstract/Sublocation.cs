using System;

namespace Assets.Scripts.InGameScripts.World.Absctract
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

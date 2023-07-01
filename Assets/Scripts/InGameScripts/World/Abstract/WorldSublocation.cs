using System;

namespace Assets.Scripts.InGameScripts.World.Absctract
{
    [Serializable]
    public abstract class WorldSublocation
    {
        public abstract int Id { get; }

        public abstract string Name { get; }

        public WorldLocation ParentLocation { get; }

        public WorldSublocation()
        {

        }
    }
}

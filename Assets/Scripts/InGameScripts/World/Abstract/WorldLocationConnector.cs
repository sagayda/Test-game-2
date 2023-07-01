using System;

namespace Assets.Scripts.InGameScripts.World.Absctract
{
    [Serializable]
    public abstract class WorldLocationConnector
    {
        public abstract int Id { get; }

        public abstract string Name { get; }

        public abstract string Description { get; }

        public WorldLocation FromLocation { get; protected set; }

        public WorldLocation ToLocation { get; protected set; }

        public WorldLocationConnector()
        {

        }

        public WorldLocationConnector(WorldLocation fromLocation, WorldLocation toLocation)
        {
            FromLocation = fromLocation;
            ToLocation = toLocation;
        }

        public virtual void Connect(WorldLocation fromLocation, WorldLocation toLocation)
        {
            FromLocation = fromLocation;
            ToLocation = toLocation;
        }
    }
}

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

        public Direction Direction => GetDirection();

        public bool IsBidirectional 
        { get
            {
                if(FromLocation == null || ToLocation == null)
                    return false;

                foreach (var toLocationConnector in ToLocation.Connectors)
                    if(toLocationConnector.ToLocation == FromLocation)
                        return true;

                return false;
            } 
        }

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

        public Direction GetDirection()
        {
            if (FromLocation == null || ToLocation == null)
                throw new ArgumentNullException("Locations are empty!");

            if (FromLocation.Y - ToLocation.Y == 1)
                return Direction.North;
            else if (FromLocation.Y - ToLocation.Y == -1)
                return Direction.South;
            else if (FromLocation.X - ToLocation.X == 1)
                return Direction.West;
            else if (FromLocation.X - ToLocation.X == -1)
                return Direction.East;
            else
                throw new ArgumentException("Can't get direction!");

        }
    }

    public enum Direction 
    {
        North,
        East,
        South,
        West,
    }
}

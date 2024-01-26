using System;

namespace WorldGeneration.Core.Locations
{
    [Serializable]
    public abstract class LocationConnector
    {
        public abstract int Id { get; }

        public abstract string Name { get; }

        public abstract string Description { get; }

        public Location FromLocation { get; protected set; }

        public Location ToLocation { get; protected set; }

        public Direction Direction => GetDirection();

        //public bool IsBidirectional 
        //{ get
        //    {
        //        if(FromLocation == null || ToLocation == null)
        //            return false;

        //        foreach (var toLocationConnector in ToLocation.Connectors)
        //            if(toLocationConnector.ToLocation == FromLocation)
        //                return true;

        //        return false;
        //    } 
        //}

        public LocationConnector()
        {

        }

        public LocationConnector(Location fromLocation, Location toLocation)
        {
            FromLocation = fromLocation;
            ToLocation = toLocation;
        }

        public virtual void Connect(Location fromLocation, Location toLocation)
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

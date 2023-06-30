using Assets.Scripts.InGameScripts.Interfaces;
using Assets.Scripts.InGameScripts.World.Interfaces;
using System;

namespace Assets.Scripts.InGameScripts
{
    [Serializable]
    public class Player
    {
        public IPlayerInfo Info { get; }

        public IWorldLocation Location { get; set; }

        public Player(IPlayerInfo playerInfo, IWorldLocation playerLocation)
        {
            Info = playerInfo;
            Location = playerLocation;
        }

        public void TimeStep()
        {
            Info.Health -= 1;
        }

        public void GoToLocation(IWorldLocation location)
        {
            foreach (var neighbourLocation in Location.NeighbourLocations)
            {
                if (neighbourLocation == location)
                {
                    Location = location;
                    break;
                }
            }
        }
    }
}

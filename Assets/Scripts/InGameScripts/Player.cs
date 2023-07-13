using Assets.Scripts.InGameScripts.Interfaces;
using Assets.Scripts.InGameScripts.World.Absctract;
using System;

namespace Assets.Scripts.InGameScripts
{
    [Serializable]
    public class Player
    {
        public IPlayerInfo Info { get; }

        public Location Location { get; set; }

        public Player(IPlayerInfo playerInfo, Location playerLocation)
        {
            Info = playerInfo;
            Location = playerLocation;
        }

        public void TimeStep()
        {
            Info.Hunger -= 1;
            Info.Thirst -= 2;
        }

        //not working
        public void GoToLocation(Location location)
        {
            //foreach (var connector in Location.Connectors)
            //{
            //    if ((connector.ToLocation) == location)
            //    {
            //        Location = location;
            //        break;
            //    }
            //}
        }
    }
}

using Assets.Scripts.InGameScripts.World.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.InGameScripts.World
{
    public class WorldLocationConnector_Free : IWorldLocationConnector
    {
        public int Id => 0;

        public string Name => "FreeConnection";

        public string Description => "Player can move freely";

        public bool IsBidirectional {get; set;} = true;

        public IWorldLocation FromLocation { get; set; }
        public IWorldLocation ToLocation { get; set; }

        public WorldLocationConnector_Free(IWorldLocation fromLocation, IWorldLocation toLocation, bool isBidirectional = true)
        {
            FromLocation = fromLocation;
            ToLocation = toLocation;
            IsBidirectional = isBidirectional;
        }
    }
}

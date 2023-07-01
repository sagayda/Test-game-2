using Assets.Scripts.InGameScripts.World.Absctract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.InGameScripts.World
{
    [Serializable]
    public class WorldLocationConnector_Free : WorldLocationConnector
    {
        public override int Id => 0;

        public override string Name => "FreeConnection";

        public override string Description => "Player can move freely";

        public WorldLocationConnector_Free() 
        { 
        
        }

        public WorldLocationConnector_Free(WorldLocation fromLocation, WorldLocation toLocation) : base(fromLocation, toLocation)
        {

        }
    }
}

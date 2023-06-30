using Assets.Scripts.InGameScripts.World.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.InGameScripts.World
{
    [Serializable]
    public class WorldSublocation_Wasteland : IWorldSublocation
    {
        public int Id => 0;

        public string Name => "Wasteland sublocation";

        public IWorldLocation ParentLocation { get; }

        public IWorldSublocation[] NeighbourSublocations { get; set; } = new IWorldSublocation[4];

        public WorldSublocation_Wasteland()
        {
            
        }

        public WorldSublocation_Wasteland(IWorldLocation parentLocation)
        {
            if(parentLocation == null) 
                throw new ArgumentNullException(nameof(parentLocation));
            
            ParentLocation = parentLocation;
        }
    }
}

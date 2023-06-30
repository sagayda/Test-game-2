using Assets.Scripts.InGameScripts.World.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.InGameScripts.World
{
    [Serializable]
    public class WorldLocation_Wasteland : IWorldLocation
    {
        public int Id => 0;

        public string Name { get => "Wasteland"; }
        public string Description { get => "This is wasteland"; }
        public IWorldSublocation Sublocation { get; set; }
        public IWorldLocation[] NeighbourLocations { get; set; } = new IWorldLocation[4];

        public bool IsHasPlayer => PlayerId != -1;

        public int PlayerId { get; set; } = -1;

        public WorldLocation_Wasteland()
        {
            
        }

        public WorldLocation_Wasteland(IWorldSublocation sublocation)
        {
            if (sublocation == null)
                throw new ArgumentNullException(nameof(sublocation));

            Sublocation = sublocation;
        }

        public WorldLocation_Wasteland(IWorldSublocation sublocation, IWorldLocation[] neighbourLocations)
        {
            if (sublocation == null)
                throw new ArgumentNullException(nameof(sublocation));

            if (neighbourLocations == null)
                throw new ArgumentNullException(nameof(neighbourLocations));

            if (neighbourLocations.Length == 0)
                throw new ArgumentException("Min neighbour locations is 1. To clear neighbour locations use method");

            if (neighbourLocations.Length > 4)
                throw new ArgumentException("Max neighbour locations is 4");

            Sublocation = sublocation;

            for (int i = 0; i < neighbourLocations.Length; i++)
            {
                NeighbourLocations[i] = neighbourLocations[i];
            }
        }

        public void SetNeighbours(IWorldLocation[] neighbourLocations)
        {
            if (neighbourLocations == null)
                throw new ArgumentNullException(nameof(neighbourLocations));

            if (neighbourLocations.Length == 0)
                throw new ArgumentException("Min neighbour locations is 1. To clear neighbour locations use method");

            if (neighbourLocations.Length > 4)
                throw new ArgumentException("Max neighbour locations is 4");

            for (int i = 0; i < neighbourLocations.Length; i++)
            {
                NeighbourLocations[i] = neighbourLocations[i];
            }
        }

        public void SetNeighbours(IWorldLocation neighbourLocation)
        {
            NeighbourLocations[0] = neighbourLocation;
        }
    }
}

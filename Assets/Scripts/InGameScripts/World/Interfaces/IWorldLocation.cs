using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.InGameScripts.World.Interfaces
{
    public interface IWorldLocation
    {
        public int Id { get; }

        public int X { get; set; }
        public int Y { get; set; }

        public string Name { get; }

        public string Description { get; }

        public IWorldSublocation Sublocation { get; set; }

        public IWorldLocation[] NeighbourLocations { get; set; }

        public IWorldLocationConnector[] Connectors { get; set; }

    }
}

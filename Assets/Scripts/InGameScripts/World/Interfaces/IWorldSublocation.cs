using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.InGameScripts.World.Interfaces
{
    public interface IWorldSublocation
    {
        public int Id { get; }

        public string Name { get; }

        public IWorldLocation ParentLocation { get; }

        public IWorldSublocation[] NeighbourSublocations { get; set; }
    }
}

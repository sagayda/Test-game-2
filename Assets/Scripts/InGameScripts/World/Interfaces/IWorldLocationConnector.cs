using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.InGameScripts.World.Interfaces
{
    public interface IWorldLocationConnector
    {
        public int Id { get; }

        public string Name { get;  }

        public string Description { get; }

        public bool IsBidirectional { get; set; }

        public IWorldLocation FromLocation { get; set; }

        public IWorldLocation ToLocation { get; set; }

    }
}

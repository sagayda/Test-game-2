using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.InGameScripts.Interfaces
{
    public interface IEntityInfo
    {
        public int Id { get; }

        public bool IsLoaded { get; }

        public string Name { get; }

        public string Description { get; }

        public uint Health { get; }

        public uint Experience { get; }
    }
}

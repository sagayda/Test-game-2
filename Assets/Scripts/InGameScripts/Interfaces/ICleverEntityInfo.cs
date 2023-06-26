using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.InGameScripts.Interfaces
{
    public interface ICleverEntityInfo : IEntityInfo
    {
        public uint Damage { get; }

        public uint Armor { get; }

        public uint Stamina { get; }

        public uint Hunger { get; }

        public uint Thirst { get; }
    }
}

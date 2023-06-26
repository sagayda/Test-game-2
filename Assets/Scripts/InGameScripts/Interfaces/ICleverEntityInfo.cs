using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.InGameScripts.Interfaces
{
    public interface ICleverEntityInfo : IEntityInfo
    {
        public int Damage { get; set; }

        public int Armor { get; set; }

        public int Stamina { get; set; }

        public int Hunger { get; set; }

        public int Thirst { get; set; }
    }
}

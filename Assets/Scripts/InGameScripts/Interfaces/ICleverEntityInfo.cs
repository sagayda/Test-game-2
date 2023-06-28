using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.InGameScripts.Interfaces
{
    public interface ICleverEntityInfo : IEntityInfo
    {
        public float Damage { get; set; }

        public float Armor { get; set; }

        public float Stamina { get; set; }

        public float Hunger { get; set; }

        public float Thirst { get; set; }
    }
}

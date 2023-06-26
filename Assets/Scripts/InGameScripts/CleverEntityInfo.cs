using Assets.Scripts.InGameScripts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.InGameScripts
{
    public class CleverEntityInfo : ICleverEntityInfo
    {
        public int Damage { 
            get;
            set;
        }

        public int Armor
        {
            get;
            set;
        }

        public int Stamina {
            get;
            set;
        }

        public int Hunger {
            get; 
            set;
        }

        public int Thirst {
            get;
            set;
        }

        public int Id
        {
            get;
            set;
        }

        public bool IsLoaded {
            get;
            set;
        }

        public string Name {
            get;
            set;
        }

        public string Description {
            get;
            set;
        }

        public int Health {
            get;
            set;
        }

        public int Experience {
            get;
            set;
        }

        public CleverEntityInfo(int id, string name, string description, int health, int experience, bool isLoaded = false)
        {
            Id = id;
            IsLoaded = isLoaded;
            Name = name;
            Description = description;
            Health = health;
            Experience = experience;
        }
    }
}

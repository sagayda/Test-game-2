using Assets.Scripts.InGameScripts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.InGameScripts
{
    public class EntityInfo : IEntityInfo
    {
        public int Id { get; set; }

        public bool IsLoaded { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int MaxHealth { get; set; }

        public int Health { get; set; }

        public int Experience { get; set; }

        public EntityInfo(int id, string name, string description, int maxHealth, int health, int experience, bool isLoaded = false)
        {
            Id = id;
            IsLoaded = isLoaded;
            Name = name;
            Description = description;
            MaxHealth = maxHealth;
            Health = health;
            Experience = experience;
        }
    }
}

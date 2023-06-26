using Assets.Scripts.InGameScripts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.InGameScripts
{
    internal class PlayerInfo : IPlayerInfo
    {
        public int Id { get; }
        public string Name {get; set;}
        public string Description { get; set; }
        public int Health { get; set; }
        public int Experience { get; set; }
        public int Damage { get; set; }
        public int Armor { get; set; }
        public int Stamina { get; set; }
        public int Hunger { get; set; }
        public int Thirst { get; set; }

        public PlayerInfo(int id, string name, string description, int health, int experience, int damage, int armour, int stamina, int hunger, int thirst)
        {
            Id = id;
            Name = name;
            Description = description;
            Health = health;
            Experience = experience;
            Damage = damage;
            Armor = armour;
            Stamina = stamina;
            Hunger = hunger;
            Thirst = thirst;
        }
    }
}

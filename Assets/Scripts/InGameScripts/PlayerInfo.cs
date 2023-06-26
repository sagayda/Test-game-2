using Assets.Scripts.InGameScripts.Interfaces;

namespace Assets.Scripts.InGameScripts
{
    internal class PlayerInfo : IPlayerInfo
    {
        public int Id { get; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MaxHealth { get; set; }
        public int Health { get; set; }
        public int Experience { get; set; }
        public int Damage { get; set; }
        public int Armor { get; set; }
        public int MaxStamina { get; set; }
        public int Stamina { get; set; }
        public int MaxHunger { get; set; }
        public int Hunger { get; set; }
        public int MaxThirst { get; set; }
        public int Thirst { get; set; }

        public PlayerInfo(int id, string name, string description, int maxHealth, int health, int experience, int damage, int armour, int maxStamina, int stamina, int maxHunger, int hunger, int maxThirst, int thirst)
        {
            Id = id;
            Name = name;
            Description = description;
            MaxHealth = maxHealth;
            Health = health;
            Experience = experience;
            Damage = damage;
            Armor = armour;
            MaxStamina = stamina;
            Stamina = stamina;
            MaxHunger = maxHunger;
            Hunger = hunger;
            MaxThirst = thirst;
            Thirst = thirst;
        }
    }
}

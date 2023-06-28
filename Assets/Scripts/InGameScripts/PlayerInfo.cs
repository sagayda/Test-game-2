using Assets.Scripts.InGameScripts.Interfaces;

namespace Assets.Scripts.InGameScripts
{
    internal class PlayerInfo : IPlayerInfo
    {
        public int Id { get; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float MaxHealth { get; set; }
        public float Health { get; set; }
        public float Experience { get; set; }
        public float Damage { get; set; }
        public float Armor { get; set; }
        public float MaxStamina { get; set; }
        public float Stamina { get; set; }
        public float MaxHunger { get; set; }
        public float Hunger { get; set; }
        public float MaxThirst { get; set; }
        public float Thirst { get; set; }

        public PlayerInfo(int id,
                          string name,
                          string description,
                          float maxHealth,
                          float health,
                          float experience,
                          float damage,
                          float armour,
                          float maxStamina,
                          float stamina,
                          float maxHunger,
                          float hunger,
                          float maxThirst,
                          float thirst)
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

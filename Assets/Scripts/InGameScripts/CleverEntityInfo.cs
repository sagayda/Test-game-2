using Assets.Scripts.InGameScripts.Interfaces;

namespace Assets.Scripts.InGameScripts
{
    public class CleverEntityInfo : ICleverEntityInfo
    {
        public int Id
        {
            get;
            set;
        }

        public int MaxHealth
        {
            get;
            set;
        }

        public int Health
        {
            get;
            set;
        }

        public int Damage
        {
            get;
            set;
        }

        public int Armor
        {
            get;
            set;
        }
        public int MaxStamina
        {
            get;
            set;
        }

        public int Stamina
        {
            get;
            set;
        }

        public int MaxHunger
        {
            get;
            set;
        }

        public int Hunger
        {
            get;
            set;
        }

        public int MaxThirst
        {
            get;
            set;
        }

        public int Thirst
        {
            get;
            set;
        }

        public bool IsLoaded
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public int Experience
        {
            get;
            set;
        }

        //not all properties added
        public CleverEntityInfo(int id, string name, string description, int maxHealth, int health, int experience, bool isLoaded = false)
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

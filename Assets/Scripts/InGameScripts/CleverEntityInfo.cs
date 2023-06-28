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

        public float MaxHealth
        {
            get;
            set;
        }

        public float Health
        {
            get;
            set;
        }

        public float Damage
        {
            get;
            set;
        }

        public float Armor
        {
            get;
            set;
        }
        public float MaxStamina
        {
            get;
            set;
        }

        public float Stamina
        {
            get;
            set;
        }

        public float MaxHunger
        {
            get;
            set;
        }

        public float Hunger
        {
            get;
            set;
        }

        public float MaxThirst
        {
            get;
            set;
        }

        public float Thirst
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

        public float Experience
        {
            get;
            set;
        }

        //not all properties added
        public CleverEntityInfo(int id, string name, string description, float maxHealth, float health, float experience, bool isLoaded = false)
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

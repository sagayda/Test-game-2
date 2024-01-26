using Assets.Scripts.Model.InGameScripts.Interfaces;

namespace Assets.Scripts.Model.InGameScripts
{
    public class EntityInfo : IEntityInfo
    {
        public int Id { get; set; }

        public bool IsLoaded { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public float MaxHealth { get; set; }

        public float Health { get; set; }

        public float Experience { get; set; }

        public EntityInfo(int id, string name, string description, float maxHealth, float health, float experience, bool isLoaded = false)
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

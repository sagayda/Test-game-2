namespace Assets.Scripts.InGameScripts.Interfaces
{
    public interface IPlayerInfo
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
    }
}

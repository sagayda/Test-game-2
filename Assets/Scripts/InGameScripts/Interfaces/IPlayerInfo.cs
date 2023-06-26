namespace Assets.Scripts.InGameScripts.Interfaces
{
    public interface IPlayerInfo
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
    }
}

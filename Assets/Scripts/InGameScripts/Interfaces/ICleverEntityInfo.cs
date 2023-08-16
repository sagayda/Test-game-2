namespace Assets.Scripts.Model.InGameScripts.Interfaces
{
    public interface ICleverEntityInfo : IEntityInfo
    {
        public float Damage { get; set; }

        public float Armor { get; set; }

        public float Stamina { get; set; }

        public float Hunger { get; set; }

        public float Thirst { get; set; }
    }
}

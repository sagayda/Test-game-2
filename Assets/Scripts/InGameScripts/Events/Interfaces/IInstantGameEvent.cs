namespace Assets.Scripts.InGameScripts.Events.Interfaces
{
    public interface IInstantGameEvent
    {
        public int Id { get; }

        public World World { get; }

        public string Name { get; }

        public string Info { get; }

        public bool IsDead { get; }

        public bool Start();

        public void Break();
    }
}

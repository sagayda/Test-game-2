using WorldGeneration.Core.Locations;

namespace Assets.Scripts.Model.InGameScripts.Events.Interfaces
{
    public interface IInstantGameEvent
    {
        public int Id { get; }

        public GameWorld World { get; }

        public string Name { get; }

        public string Info { get; }

        public bool IsDead { get; }

        public bool Start();

        public void Break();
    }
}

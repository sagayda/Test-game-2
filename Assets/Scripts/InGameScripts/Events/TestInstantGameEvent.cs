using Assets.Scripts.Model.InGameScripts.Events.Interfaces;
using System;
using WorldGeneration.Core.Locations;

namespace Assets.Scripts.Model.InGameScripts.Events
{
    [Serializable]
    public class TestInstantGameEvent : IInstantGameEvent
    {
        public int Id { get; }
        public GameWorld World { get; }
        public string Name => "TEST EVENT";
        public string Info => "THIS IS TEST EVENT FOR -10hp FOR ALL PLAYERS";
        public bool IsDead { get; private set; } = false;

        public TestInstantGameEvent(GameWorld world)
        {
            World = world;
        }

        public void Break()
        {
            IsDead = true;
        }

        public bool Start()
        {
            if(IsDead) 
                return false;

            IsDead = true;
            return true;
        }
    }
}

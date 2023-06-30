using Assets.Scripts.InGameScripts.Events.Interfaces;
using System;

namespace Assets.Scripts.InGameScripts.Events
{
    public class TestInstantGameEvent : IInstantGameEvent
    {
        public int Id { get; }
        public World World { get; }
        public string Name => "TEST EVENT";
        public string Info => "THIS IS TEST EVENT FOR -10hp FOR ALL PLAYERS";
        public bool IsDead { get; private set; } = false;

        public TestInstantGameEvent(World world)
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

            foreach (var player in World.Players)
            {
                player.PlayerInfo.Health -= 10;
            }

            IsDead = true;
            return true;
        }
    }
}

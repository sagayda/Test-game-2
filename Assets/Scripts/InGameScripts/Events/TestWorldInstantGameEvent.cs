using Assets.Scripts.InGameScripts.Events.Interfaces;
using System;

namespace Assets.Scripts.InGameScripts.Events
{
    public class TestWorldInstantGameEvent : IWorldInstantGameEvent
    {
        public int Id { get; }

        public World World { get; }

        public string Name { get; private set; }
        public string Info { get; private set; }
        public bool IsDead { get; private set; } = false;

        public TestWorldInstantGameEvent(World world)
        {
            World = world;
        }

        public void Break()
        {
            throw new NotImplementedException();
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

using Assets.Scripts.InGameScripts.Events.Interfaces;
using Assets.Scripts.InGameScripts.World.Absctract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.InGameScripts
{
    [Serializable]
    public class GameWorld
    {
        int Id { get; }
        int CurrentTimeTick { get; set; } = 0;
        public string Name { get; }

        public List<Player> Players { get; private set; } = new List<Player>();

        public List<IInstantGameEvent> instantGameEvents { get; set; } = new();

        public int WorldSize => World.Length;
        public WorldLocation[,] World { get; }

        public GameWorld(int id, string name, WorldLocation[,] world)
        {
            if(world == null)
                throw new ArgumentNullException(nameof(world));

            if(world.Length == 0)
                throw new ArgumentException("World map cant be empty");

            Id = id;
            Name = name;
            World = world;
        }

        public GameWorld(int id, string name, WorldLocation[,] world, Player player)
        {
            if (world == null)
                throw new ArgumentNullException(nameof(world));

            if (world.Length == 0)
                throw new ArgumentException("World map cant be empty");

            Id = id;
            Name = name;
            World = world;

            AddPlayer(player);
        }

        public GameWorld(int id, string name, WorldLocation[,] world, List<Player> players)
        {
            if (world == null)
                throw new ArgumentNullException(nameof(world));

            if (world.Length == 0)
                throw new ArgumentException("World map cant be empty");

            Id = id;
            Name = name;
            World = world;

            AddPlayer(players);
        }

        public void TimeTickStep()
        {
            foreach (var gameEvent in instantGameEvents) 
            {
                CurrentTimeTick++;
                gameEvent.Start();

                foreach (var player in Players)
                {
                    player.TimeStep();
                }
            }
        }

        public void AddPlayer(Player player)
        {
            Players.Add(player);
        }

        public void AddPlayer(List<Player> players)
        {
            Players.AddRange(players);
        }
    }
}

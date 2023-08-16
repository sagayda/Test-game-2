using System;
using System.Collections.Generic;

namespace Assets.Scripts.Model.InGameScripts
{
    [Serializable]
    public class GameWorld
    {
        int Id { get; }
        int CurrentTimeTick { get; set; } = 0;
        public string Name { get; }

        public List<Player> Players { get; private set; } = new List<Player>();

        public List<IInstantGameEvent> InstantGameEvents { get; set; } = new();

        public int Width => World.GetLength(0);
        public int Height => World.GetLength(1);

        public Location[,] World { get; }

        public GameWorld(int id, string name, Location[,] world)
        {
            if (world == null)
                throw new ArgumentNullException(nameof(world));

            if (world.Length == 0)
                throw new ArgumentException("World map cant be empty");

            Id = id;
            Name = name;
            World = world;
        }

        public GameWorld(int id, string name, Location[,] world, Player player)
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

        public GameWorld(int id, string name, Location[,] world, List<Player> players)
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
            foreach (var gameEvent in InstantGameEvents)
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

        public void ClearPlayers()
        {
            Players.Clear();
        }

    }
}

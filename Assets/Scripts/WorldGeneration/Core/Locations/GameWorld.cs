using System;

namespace WorldGeneration.Core.Locations
{
    [Serializable]
    public class GameWorld
    {
        int Id { get; }
        int CurrentTimeTick { get; set; } = 0;
        public string Name { get; }

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

    }
}

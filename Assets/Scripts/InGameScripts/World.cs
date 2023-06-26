﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.InGameScripts
{
    public class World
    {
        int Id { get; }

        int CurrentTimeTick { get; set; } = 0;

        public string Name { get; }

        public List<Player> Players { get; set; } = new List<Player>();

        public World(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public World(int id, string name, Player player)
        {
            Id = id;
            Name = name;
            Players.Add(player);
        }

        public World(int id, string name, List<Player> players)
        {
            Id = id;
            Name = name;
            Players.AddRange(players);
        }

    }
}

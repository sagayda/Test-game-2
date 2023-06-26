using System;
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

        public World(int id, string name)
        {
            Id = id;
            Name = name;
        }


    }
}

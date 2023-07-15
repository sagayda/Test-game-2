using Assets.Scripts.InGameScripts.World.Absctract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.InGameScripts.World
{
    public class Location_Mountains : Location
    {
        public override int Id => 7;

        public override string Name => "Mountains";

        public override string Description => "Its very high!";

        public override Color Color => new(0.45f, 0.15f, 0);

        public Location_Mountains(int x, int y) : base(x, y)
        {
        }

        public Location_Mountains(int x, int y, Sublocation sublocation) : base(x, y, sublocation)
        {
        }

        public Location_Mountains(int x, int y, LocationBarriers barriers) : base(x, y, barriers)
        {
        }
    }
}

using Assets.Scripts.InGameScripts.World.Absctract;
using System;
using UnityEngine;

namespace Assets.Scripts.InGameScripts.World
{
    public class Location_Foothills : Location
    {
        public override int Id => 6;

        public override string Name => "Foothills";

        public override string Description => "There are mountains nearby!";

        public override Color Color => new(0, 0.75f,0);

        public Location_Foothills(int x, int y) : base(x, y)
        {
        }

        public Location_Foothills(int x, int y, Sublocation sublocation) : base(x, y, sublocation)
        {
        }

        public Location_Foothills(int x, int y, LocationBarriers barriers) : base(x, y, barriers)
        {
        }
    }
}

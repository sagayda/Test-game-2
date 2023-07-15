using Assets.Scripts.InGameScripts.World.Absctract;
using UnityEngine;

namespace Assets.Scripts.InGameScripts.World
{
    public class Location_DeepOcean : Location
    {
        public override int Id => 4;

        public override string Name => "Deep ocean";

        public override string Description => "Very deep ocean";

        public override Color Color => new(0,0,0.75f);

        public Location_DeepOcean(int x, int y) : base(x, y)
        {
        }

        public Location_DeepOcean(int x, int y, Sublocation sublocation) : base(x, y, sublocation)
        {
        }

        public Location_DeepOcean(int x, int y, LocationBarriers barriers) : base(x, y, barriers)
        {
        }
    }
}

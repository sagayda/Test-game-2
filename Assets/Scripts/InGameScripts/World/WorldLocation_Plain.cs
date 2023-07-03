using Assets.Scripts.InGameScripts.World.Absctract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.InGameScripts.World
{
    [Serializable]
    public class WorldLocation_Plain : WorldLocation
    {
        public override int Id => 1;
        public override string Name => "Plain";
        public override string Description => "Green plain";
        public override Color Color => Color.green;

        public WorldLocation_Plain(int x, int y) : base(x, y)
        {
        }

        public WorldLocation_Plain(int x, int y, WorldSublocation sublocation) : base(x, y, sublocation)
        {
        }

        public WorldLocation_Plain(int x, int y, WorldSublocation sublocation, List<WorldLocationConnector> connectors) : base(x, y, sublocation, connectors)
        {
        }
    }
}

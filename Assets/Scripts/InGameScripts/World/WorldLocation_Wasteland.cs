using Assets.Scripts.InGameScripts.World.Absctract;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.InGameScripts.World
{
    [Serializable]
    public class WorldLocation_Wasteland : WorldLocation
    {
        public override string Name => "Wasteland";
        public override string Description => "This is wasteland";
        public override int Id => 0;

        public WorldLocation_Wasteland(int x, int y, WorldSublocation sublocation) : base(x, y, sublocation)
        {

        }

        public WorldLocation_Wasteland(int x, int y) : base(x, y)
        {

        }

        public WorldLocation_Wasteland(int x, int y, WorldSublocation sublocation, List<WorldLocationConnector> connectors) : base(x, y, sublocation, connectors)
        {

        }
    }
}

using Assets.Scripts.InGameScripts.World.Absctract;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.InGameScripts.World
{
    [Serializable]
    public class Location_Wasteland : Location
    {
        public override string Name => "Wasteland";
        public override string Description => "This is wasteland";
        public override int Id => 0;
        public override Color Color => Color.yellow;

        public Location_Wasteland(int x, int y, Sublocation sublocation) : base(x, y, sublocation)
        {

        }

        public Location_Wasteland(int x, int y) : base(x, y)
        {

        }

    }
}

using System;
using UnityEngine;

namespace Assets.Scripts.Model.InGameScripts.World
{
    [Serializable]
    public class Location_Wasteland : Location
    {
        public override string Name => "Wasteland";
        public override string Description => "This is wasteland";
        public override int Id => 0;
        public override Color Color => Color.yellow;

        public Location_Wasteland(int x, int y, Sublocation[,] sublocations) : base(x, y, sublocations)
        {

        }

        public Location_Wasteland(int x, int y) : base(x, y)
        {

        }

    }
}

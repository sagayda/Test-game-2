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
    public class Location_Plain : Location
    {
        public override int Id => 1;
        public override string Name => "Plain";
        public override string Description => "Green plain";
        public override Color Color => Color.green;

        public Location_Plain(int x, int y) : base(x, y)
        {
        }

        public Location_Plain(int x, int y, Sublocation sublocation) : base(x, y, sublocation)
        {
        }

    }
}

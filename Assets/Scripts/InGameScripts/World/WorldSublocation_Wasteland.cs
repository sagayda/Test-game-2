﻿using Assets.Scripts.InGameScripts.World.Absctract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.InGameScripts.World
{
    [Serializable]
    public class WorldSublocation_Wasteland : WorldSublocation
    {
        public override int Id => 0;
        public override string Name => "Wasteland sublocation";

        public WorldSublocation_Wasteland()
        {
            
        }

    }
}

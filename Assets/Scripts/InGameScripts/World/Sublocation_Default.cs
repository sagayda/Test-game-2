using Assets.Scripts.InGameScripts.World.Absctract;
using System;

namespace Assets.Scripts.InGameScripts.World
{
    [Serializable]
    public class Sublocation_Default : Sublocation
    {
        public override int Id => 0;
        public override string Name => "Default sublocation";

        public Sublocation_Default()
        {

        }

    }
}

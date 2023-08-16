using System;

namespace Assets.Scripts.Model.InGameScripts.World
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

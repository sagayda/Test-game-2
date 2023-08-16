using Assets.Scripts.Model.InGameScripts.Interfaces;

namespace Assets.Scripts.Model.InGameScripts
{
    public class PlayerPosition : IEntityPosition
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public PlayerPosition(float x, float y)
        {
            X = x;
            Y = y;
        }
    }
}

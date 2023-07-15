using Assets.Scripts.InGameScripts.Interfaces;

namespace Assets.Scripts.InGameScripts
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

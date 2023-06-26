using Assets.Scripts.InGameScripts.Interfaces;

namespace Assets.Scripts.InGameScripts
{
    public class Player
    {
        public IPlayerInfo PlayerInfo { get; }

        public Player(IPlayerInfo playerInfo)
        {
            PlayerInfo = playerInfo;
        }

        public void TimeStep()
        {
            PlayerInfo.Health -= 1;
        }
    }
}

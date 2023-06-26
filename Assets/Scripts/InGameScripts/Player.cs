using Assets.Scripts.InGameScripts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.InGameScripts
{
    public class Player
    {
        public IPlayerInfo PlayerInfo { get; }

        public Player(IPlayerInfo playerInfo)
        {
            PlayerInfo = playerInfo;
        }

    }
}

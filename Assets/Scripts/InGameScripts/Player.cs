using System;
using Assets.Scripts.Model.InGameScripts.Interfaces;
using Core;
using UnityEngine;
using WorldGeneration.Core.Locations;

namespace Assets.Scripts.Model.InGameScripts
{
    [Serializable]
    public class Player
    {
        public IPlayerInfo Info { get; }

        public PlayerPosition Position { get; }

        public Vector2 VectorPosition { get; private set; }

        public GameWorld World { get; }

        public Player(IPlayerInfo playerInfo, GameWorld world)
        {
            Info = playerInfo;
            World = world;
        }

        public void TimeStep()
        {
            Info.Hunger -= 1;
            Info.Thirst -= 2;
        }

        public void GoToCoordinates(Vector2 coordinates)
        {
            //VectorPosition = Vector2.MoveTowards(VectorPosition, coordinates, 1 * Time.deltaTime);
            VectorPosition = coordinates;

            EventBus.WorldEvents.onPlayerPositionChanged?.Invoke();
        }

    }
}

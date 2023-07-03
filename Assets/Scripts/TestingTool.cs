using Assets.Scripts.InGameScripts.Interfaces;
using Assets.Scripts.InGameScripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.InGameScripts.World.Absctract;
using Assets.Scripts.InGameScripts.Events;
using Assets.Scripts.InGameScripts.World;

namespace Assets.Scripts
{
    public static class TestingTool
    {
        public static IPlayerInfo CreatePlayerInfo()
        {
            return new PlayerInfo(0, "TestPlayer", "TEST", 100, 85, 50, 1, 0, 100, 100, 100, 10, 200, 100);
        }

        public static Player CreatePlayer(WorldLocation playerLocation)
        {
            return new Player(CreatePlayerInfo(), playerLocation);
        }

        public static GameWorld CreateWorld(byte size)
        {
            var world = new GameWorld(0, "FirstWorld", CreateWorldMap(size));
            world.instantGameEvents.Add(new TestInstantGameEvent(world));
            world.instantGameEvents.Add(new TestInstantGameEvent(world));
            world.instantGameEvents.Add(new TestInstantGameEvent(world));

            return world;
        }

        public static WorldLocation[,] CreateWorldMap(int size)
        {
            WorldLocation[,] map = new WorldLocation[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (i == 0 && j == 0)
                    {
                        map[i, j] = new WorldLocation_Wasteland(i, j);
                    }

                    if (UnityEngine.Random.Range(0, 100) >= 80)
                        continue;

                    map[i, j] = CreateRandomLocation(i,j);
                }
            }

            for (int i = 0; i < size - 1; i++)
            {
                for (int j = 0; j < size - 1; j++)
                {
                    if (map[i, j] == null)
                        continue;

                    WorldLocationConnector[] connectors;

                    if (i == 0 && j == 0)
                    {
                        map[0, 0].TryConnect(map[0, 1], new WorldLocationConnector_Free());
                        map[0, 0].TryConnect(map[1, 0], new WorldLocationConnector_Free());

                        continue;
                    }

                    if (i == 0)
                    {
                        map[i, j].TryConnect(map[i, j - 1], new WorldLocationConnector_Free());
                        map[i, j].TryConnect(map[i, j + 1], new WorldLocationConnector_Free());

                        if (UnityEngine.Random.Range(0, 100) >= 90)
                        {
                            map[i, j].TryConnect(map[i+1, j], new WorldLocationConnector_Free());
                        }

                        continue;
                    }

                    if (j == 0)
                    {
                        map[i, j].TryConnect(map[i - 1, j], new WorldLocationConnector_Free());
                        map[i, j].TryConnect(map[i + 1, j], new WorldLocationConnector_Free());

                        if (UnityEngine.Random.Range(0, 100) >= 90)
                        {
                            map[i, j].TryConnect(map[i, j + 1], new WorldLocationConnector_Free());
                        }

                        continue;
                    }

                    if (UnityEngine.Random.Range(0, 100) >= 95)
                    {
                        connectors = new WorldLocationConnector[3];
                    }
                    else
                    {
                        connectors = new WorldLocationConnector[4];
                    }

                    map[i, j].TryConnect(map[i - 1, j], new WorldLocationConnector_Free());
                    map[i, j].TryConnect(map[i + 1, j], new WorldLocationConnector_Free());
                    map[i, j].TryConnect(map[i, j + 1], new WorldLocationConnector_Free());

                    if (UnityEngine.Random.Range(0, 100) >= 95)
                    {
                        map[i, j].TryConnect(map[i, j - 1], new WorldLocationConnector_Free());
                    }
                }
            }

            return map;
        }

        public static WorldLocation CreateRandomLocation(int x, int y)
        {
            int rnd = UnityEngine.Random.Range (0, 2);

            switch (rnd)
            {
                case 0:
                    return new WorldLocation_Wasteland(x, y);
                case 1:
                    return new WorldLocation_Plain(x, y);
                default:
                    return new WorldLocation_Wasteland(x, y);
            }
        }

    }
}

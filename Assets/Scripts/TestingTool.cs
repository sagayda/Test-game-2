using Assets.Scripts.InGameScripts;
using Assets.Scripts.InGameScripts.Events;
using Assets.Scripts.InGameScripts.Interfaces;
using Assets.Scripts.InGameScripts.World;
using Assets.Scripts.InGameScripts.World.Absctract;
using UnityEngine;

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

        public static GameWorld CreateWorld(int seed, float zoom, int size)
        {
            var world = new GameWorld(0, "FirstWorld", CreateWorldMap(seed, zoom, size));
            world.instantGameEvents.Add(new TestInstantGameEvent(world));
            world.instantGameEvents.Add(new TestInstantGameEvent(world));
            world.instantGameEvents.Add(new TestInstantGameEvent(world));

            return world;
        }

        public static WorldLocation[,] CreateWorldMap(int seed, float zoom, int size)
        {
            WorldLocation[,] map = new WorldLocation[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (i == 0 && j == 0)
                    {
                        map[i, j] = new WorldLocation_Wasteland(i, j);
                        continue;
                    }

                    map[i, j] = CreateRandomLocation(seed, zoom, i, j);
                }
            }

            for (int i = 0; i < size - 1; i++)
            {
                for (int j = 0; j < size - 1; j++)
                {
                    if (map[i, j] == null)
                        continue;

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

                        if (UnityEngine.Random.Range(0, 100) <= 95)
                        {
                            map[i, j].TryConnect(map[i + 1, j], new WorldLocationConnector_Free());
                        }

                        continue;
                    }

                    if (j == 0)
                    {
                        map[i, j].TryConnect(map[i - 1, j], new WorldLocationConnector_Free());
                        map[i, j].TryConnect(map[i + 1, j], new WorldLocationConnector_Free());

                        if (UnityEngine.Random.Range(0, 100) <= 95)
                        {
                            map[i, j].TryConnect(map[i, j + 1], new WorldLocationConnector_Free());
                        }

                        continue;
                    }

                    map[i, j].TryConnect(map[i - 1, j], new WorldLocationConnector_Free());
                    map[i, j].TryConnect(map[i + 1, j], new WorldLocationConnector_Free());
                    map[i, j].TryConnect(map[i, j + 1], new WorldLocationConnector_Free());

                    if (UnityEngine.Random.Range(0, 100) <= 95)
                    {
                        map[i, j].TryConnect(map[i, j - 1], new WorldLocationConnector_Free());
                    }
                }
            }

            return map;
        }

        public static WorldLocation CreateRandomLocation(int seed, float zoom, int x, int y)
        {

            float noise = Mathf.PerlinNoise(((x) + seed) / zoom, ((y) + seed) / zoom);

            WorldLocation loc;

            if (noise <= 0.5f)
            {
                loc = new WorldLocation_Wasteland(x, y);
            }
            else if (noise > 0.5f && noise <= 0.85f)
            {
                loc =  new WorldLocation_Plain(x, y);
            }
            else
            {
                loc = null;
            }

            if(loc != null)
                loc.Noise = noise;

            return loc;
        }

    }
}

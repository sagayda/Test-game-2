using Assets.Scripts.InGameScripts;
using Assets.Scripts.InGameScripts.Events;
using Assets.Scripts.InGameScripts.Interfaces;
using Assets.Scripts.InGameScripts.World;
using Assets.Scripts.InGameScripts.World.Absctract;
using Assets.Scripts.Model.WorldGeneration;
using UnityEngine;

namespace Assets.Scripts.Model.Tester
{
    public static class TestingTool
    {
        public static IPlayerInfo CreatePlayerInfo()
        {
            return new PlayerInfo(0, "TestPlayer", "TEST", 100, 85, 50, 1, 0, 100, 100, 100, 10, 200, 100);
        }

        public static Player CreatePlayer(GameWorld world)
        {
            return new Player(CreatePlayerInfo(), world);
        }

        public static GameWorld CreateWorld(WorldGeneratorOld worldGenerator)
        {

            WorldGenerator worldGenerator1 = new(new NoiseParametersSave().Default);

            var world = new GameWorld(0, "FirstWorld", worldGenerator1.CreateWorld());
            world.InstantGameEvents.Add(new TestInstantGameEvent(world));
            world.InstantGameEvents.Add(new TestInstantGameEvent(world));
            world.InstantGameEvents.Add(new TestInstantGameEvent(world));

            return world;
        }

        public static Location[,] CreateWorldMap(int seed, float zoom, int size)
        {
            Location[,] map = new Location[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (i == 0 && j == 0)
                    {
                        map[i, j] = new Location_Wasteland(i, j);
                        continue;
                    }

                    map[i, j] = CreateRandomLocation(seed, zoom, i, j);
                }
            }

            //for (int i = 0; i < size - 1; i++)
            //{
            //    for (int j = 0; j < size - 1; j++)
            //    {
            //        if (map[i, j] == null)
            //            continue;

            //        if (i == 0 && j == 0)
            //        {
            //            map[0, 0].TryConnect(map[0, 1], new WorldLocationConnector_Free());
            //            map[0, 0].TryConnect(map[1, 0], new WorldLocationConnector_Free());

            //            continue;
            //        }

            //        if (i == 0)
            //        {
            //            map[i, j].TryConnect(map[i, j - 1], new WorldLocationConnector_Free());
            //            map[i, j].TryConnect(map[i, j + 1], new WorldLocationConnector_Free());

            //            if (UnityEngine.Random.Range(0, 100) <= 95)
            //            {
            //                map[i, j].TryConnect(map[i + 1, j], new WorldLocationConnector_Free());
            //            }

            //            continue;
            //        }

            //        if (j == 0)
            //        {
            //            map[i, j].TryConnect(map[i - 1, j], new WorldLocationConnector_Free());
            //            map[i, j].TryConnect(map[i + 1, j], new WorldLocationConnector_Free());

            //            if (UnityEngine.Random.Range(0, 100) <= 95)
            //            {
            //                map[i, j].TryConnect(map[i, j + 1], new WorldLocationConnector_Free());
            //            }

            //            continue;
            //        }

            //        map[i, j].TryConnect(map[i - 1, j], new WorldLocationConnector_Free());
            //        map[i, j].TryConnect(map[i + 1, j], new WorldLocationConnector_Free());
            //        map[i, j].TryConnect(map[i, j + 1], new WorldLocationConnector_Free());

            //        if (UnityEngine.Random.Range(0, 100) <= 95)
            //        {
            //            map[i, j].TryConnect(map[i, j - 1], new WorldLocationConnector_Free());
            //        }
            //    }
            //}

            return map;
        }

        public static Location CreateRandomLocation(int seed, float zoom, int x, int y)
        {

            float noise = GetNoise(seed, zoom, x, y);

            Location loc;

            if (noise <= 0.5f)
            {
                loc = new Location_Wasteland(x, y);
            }
            else if (noise > 0.5f && noise <= 0.85f)
            {
                loc = new Location_Plain(x, y);
            }
            else
            {
                loc = null;
            }

            if (loc != null)
                loc.Noise = noise;

            return loc;
        }

        private static float GetNoise(int seed, float zoom, int x, int y)
        {
            float zoomStep = 0.4f;
            float amplitude = 5;


            float noise = Mathf.PerlinNoise(((x) + seed) / zoom, ((y) + seed) / zoom);

            noise += Mathf.PerlinNoise(((x) + seed) / (zoom * zoomStep), ((y) + seed) / (zoom * zoomStep)) / amplitude;

            noise /= 1f + (1f / amplitude);

            noise += Mathf.PerlinNoise(((x) + seed) / (zoom * zoomStep * zoomStep), ((y) + seed) / (zoom * zoomStep * zoomStep)) / (amplitude * amplitude);

            noise /= 1f + (1f / (amplitude * amplitude));

            return noise;
        }
    }

}

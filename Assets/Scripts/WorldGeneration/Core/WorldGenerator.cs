using System;
using System.Security.Cryptography;
using System.Text;
using Assets.Scripts.WorldGeneration.Core;
using Assets.Scripts.WorldGeneration.Core.Chunks;
using Assets.Scripts.WorldGeneration.Core.WaterBehavior;
using UnityEngine;
using WorldGeneration.Core.Locations;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace WorldGeneration.Core
{
    public class WorldGenerator
    {
        private World _world;

        private CompositeValueMap _compositeMap;
        private WaterBehaviour _waterBehavior;

        public WorldGenerator(string seed, int width, int height, float waterLevel, CompositeValueMap compositeMap)
        {
            Seed = seed;
            Width = width;
            Height = height;

            _compositeMap = compositeMap;
            _waterBehavior = new WaterBehaviour(waterLevel);

            SetSeed(seed);
        }

        public string Seed { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public float OceanLevel => _waterBehavior.OceanLevel;
        public CompositeValueMap CompositeValueMap => _compositeMap;
        public WaterBehaviour WaterBehavior => _waterBehavior;

        private void SetSeed(string seed)
        {
            int intSeed = ComputeInt32Seed(seed);

            _compositeMap.SetSeed(intSeed);
        }

        public float GetMapValue(Vector2 position, MapValueType valueType)
        {
            return _compositeMap.ComputeValueUpTo(position, valueType)[valueType];
        }

        public void InitWorld(string worldName, string worldDesc)
        {
            _world = new(this, worldName, worldDesc);

            _world.InitChunks();
        }

        public void InitWorldMapValues()
        {
            //foreach (var chunk in _world.Chunks)
            //{
            //    chunk.
            //}

            //Values = worldGenerator.CompositeValueMap.ComputeValue(Rect.center);
            ////no water now
            //Water = null;
            //GenerationStage = GenerationStage.Pre;
            //break;
        }

        public static int ComputeInt32Seed(string seed)
        {
            using SHA256 sha256 = SHA256.Create();

            byte[] hashX = sha256.ComputeHash(Encoding.UTF8.GetBytes("x" + seed));

            return BitConverter.ToInt32(hashX, 0);
        }

        public static GameWorld GetGameWorld()
        {
            return null;
        }
    }
}

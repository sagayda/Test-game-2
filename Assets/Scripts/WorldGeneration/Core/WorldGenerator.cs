using System;
using System.Security.Cryptography;
using System.Text;
using Assets.Scripts.WorldGeneration.Core;
using Assets.Scripts.WorldGeneration.Core.Chunks;
using Assets.Scripts.WorldGeneration.Core.WaterBehavior;
using UnityEngine;
using WorldGeneration.Core.Locations;

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
        public World World => _world;

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
            foreach (var chunk in _world.Chunks)
            {
                //ValueMapPoint[] chunkPoints =
                //{ 
                //    //center
                //    _compositeMap.ComputeValues(new(chunk.Key.x + Chunk.ChunkWidth / 2, chunk.Key.y + Chunk.ChunkHeight / 2)),
                //    //top left
                //    _compositeMap.ComputeValues(chunk.Key),
                //    //top right
                //    _compositeMap.ComputeValues(new(chunk.Key.x + Chunk.ChunkWidth, chunk.Key.y)),
                //    //bottom left
                //    _compositeMap.ComputeValues(new(chunk.Key.x, chunk.Key.y + Chunk.ChunkHeight)),
                //    //bottom right
                //    _compositeMap.ComputeValues(new(chunk.Key.x + Chunk.ChunkWidth, chunk.Key.y + Chunk.ChunkHeight)),
                //};

                Vector2 center = new(chunk.Key.x + Chunk.ChunkWidth / 2, chunk.Key.y + Chunk.ChunkHeight / 2);

                ValueMapPoint[] chunkPoints =
                { 
                    //center
                    _compositeMap.ComputeValues(center),
                    //top left
                    _compositeMap.ComputeValues(new(center.x - Chunk.ChunkWidth / 4f, center.y - Chunk.ChunkHeight / 4f)),
                    //top right
                    _compositeMap.ComputeValues(new(center.x + Chunk.ChunkWidth / 4f, center.y - Chunk.ChunkHeight / 4f)),
                    //bottom left
                    _compositeMap.ComputeValues(new(center.x - Chunk.ChunkWidth / 4f, center.y + Chunk.ChunkHeight / 4f)),
                    //bottom right
                    _compositeMap.ComputeValues(new(center.x + Chunk.ChunkWidth / 4f, center.y + Chunk.ChunkHeight / 4f)),
                };


                ValueMapPoint chunkAvarageValues = CompositeValueMap.GetAvarageValue(chunkPoints);

                chunk.Value.Values = chunkAvarageValues;

                //chunk.Value.Values = _compositeMap.ComputeValues(chunk.Key);

                if (chunk.Value.TrySetGenerationStage(GenerationStage.Pre) == false)
                    Debug.LogWarning($"Failed to initialize chunk at {chunk.Key}");
            }
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

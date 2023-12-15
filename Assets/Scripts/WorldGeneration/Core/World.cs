using System;
using System.Collections.Generic;
using Assets.Scripts.WorldGeneration.Core.Chunks;
using UnityEngine;
using WorldGeneration.Core;
using WorldGeneration.Core.WaterBehavior;

namespace Assets.Scripts.WorldGeneration.Core
{
    public class World
    {
        private readonly string _seed;
        private readonly int _width;
        private readonly int _height;
        private readonly float _oceanLevel;

        private Dictionary<Vector2Int, Chunk> _chunks;
        private WorldGenerator _generator;
#nullable enable
        private Pool? _ocean;
#nullable restore
        public World(WorldGenerator generator, string name, string desc)
        {
            _generator = generator ?? throw new ArgumentNullException(nameof(generator), "World generator can not be null!");

            Name = name;
            Description = desc;

            _seed = generator.Seed;
            _width = generator.Width;
            _height = generator.Height;
            _oceanLevel = generator.OceanLevel;

        }

        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Seed => _seed;
        public int Width => _width;
        public int Height => _height;
        public float OceanLevel => _oceanLevel;

        public IReadOnlyDictionary<Vector2Int, Chunk> Chunks => _chunks;
        public Pool Ocean => _ocean;

        public void InitChunks()
        {
            int widthByChunks = _width / Chunk.ChunkWidth;
            int heightByChunks = _height / Chunk.ChunkHeight;

            _chunks = new Dictionary<Vector2Int, Chunk>();

            for (int i = 0; i < widthByChunks; i++)
            {
                for (int j = 0; j < heightByChunks; j++)
                {
                    Chunk chunk = new(i * Chunk.ChunkWidth, j * Chunk.ChunkHeight);
                    _chunks.Add(chunk.Rect.position, chunk);
                }
            }
        }

        public Chunk GetChunkByGlobalCoordinates(Vector2 coordinates)
        {
            return _chunks[Chunk.WorldToLocalCoordinates(coordinates)];

            //Vector2Int chunkCoords = Chunk.GetChunkCoorinates(coordinates);

            //if (_chunks[chunkCoords].Rect.Contains(coordinates))
            //    return _chunks[chunkCoords];

            //Debug.LogWarning("Failed to found chunk via formula. Used full traversal");


            //foreach (var chunk in _chunks)
            //{
            //    if (chunk.Value.Rect.Contains(coordinates))
            //        return chunk.Value;
            //}

            //return null;
        }

        public Chunk GetChunkByGlobalCoordinates(float x, float y)
        {
            return _chunks[Chunk.WorldToLocalCoordinates(x,y)];

            //Vector2Int chunkCoords = Chunk.GetChunkCoorinates(x, y);

            //if (_chunks[chunkCoords].Rect.Contains(new(x,y)))
            //    return _chunks[chunkCoords];

            //Debug.LogWarning("Failed to found chunk via formula. Used full traversal");

            //Vector2Int coordinates = new(x, y);

            //foreach (var chunk in _chunks)
            //    if (chunk.Value.Rect.Contains(coordinates))
            //        return chunk.Value;

            //return null;
        }

        public Chunk GetChunkByLocalCoordinates(Vector2Int coordinates)
        {
            return _chunks.ContainsKey(coordinates) ? _chunks[coordinates] : null;
        }

        public bool TrySetOcean(Pool ocean)
        {
            if (_ocean != null)
                return false;

            _ocean = ocean;
            return true;
        }
    }
}

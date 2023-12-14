using Assets.Scripts.WorldGeneration.Core.Maps;
using UnityEngine;
using WorldGeneration.Core;

namespace Assets.Scripts.WorldGeneration.Core.Chunks
{
    public class Chunk : IMapArea
    {
        public static int ChunkWidth = 32;
        public static int ChunkHeight = 32;

#nullable enable

        private ValueMapPoint? _values;
        private WaterMapPoint? _water;

        public readonly RectInt Rect;
        public GenerationStage GenerationStage { get; private set; }
        public ValueMapPoint? Values { get; private set; }
        public WaterMapPoint? Water { get; set; }
        public Vector2Int Position => Rect.position;
        public bool IsWaterChunk => Water != null;

        public Chunk(int x, int y)
        {
            Rect = new(x, y, ChunkWidth, ChunkHeight);

            GenerationStage = GenerationStage.Empty;
            Values = null;
            Water = null;
        }

        //--
        public void GenerateNextStage(WorldGenerator worldGenerator)
        {
            switch (GenerationStage)
            {
                case GenerationStage.Empty:
                    Values = worldGenerator.CompositeValueMap.ComputeValues(Rect.center);
                    //no water now
                    Water = null;
                    GenerationStage = GenerationStage.Pre;
                    break;
                case GenerationStage.Pre:
                    //not implemented
                    GenerationStage = GenerationStage.Full;
                    break;
                case GenerationStage.Full:
                    break;
                default:
                    break;
            }
        }

        //--
        public void GenerateToStage(WorldGenerator worldGenerator, GenerationStage stage)
        {
            if (GenerationStage >= stage)
                return;

            while (GenerationStage < stage)
            {
                GenerateNextStage(worldGenerator);
            }
        }

        public static Vector2Int GetChunkCoorinates(float x, float y)
        {
            int chunkX = Mathf.FloorToInt(x / ChunkWidth) * ChunkWidth;
            int chunkY = Mathf.FloorToInt(y / ChunkHeight) * ChunkHeight;

            return new Vector2Int(chunkX, chunkY);
        }

        public static Vector2Int GetChunkCoorinates(int x, int y)
        {
            int chunkX = Mathf.FloorToInt((float)x / ChunkWidth) * ChunkWidth;
            int chunkY = Mathf.FloorToInt((float)y / ChunkHeight) * ChunkHeight;


            return new Vector2Int(chunkX, chunkY);
        }

        public static Vector2Int GetChunkCoorinates(Vector2 coordinates)
        {
            int chunkX = Mathf.FloorToInt(coordinates.x / ChunkWidth) * ChunkWidth;
            int chunkY = Mathf.FloorToInt(coordinates.y / ChunkHeight) * ChunkHeight;

            return new Vector2Int(chunkX, chunkY);
        }

        public static Vector2Int GetChunkCoorinates(Vector2Int coordinates)
        {
            int chunkX = Mathf.FloorToInt((float)coordinates.x / ChunkWidth) * ChunkWidth;
            int chunkY = Mathf.FloorToInt((float)coordinates.y / ChunkHeight) * ChunkHeight;

            return new Vector2Int(chunkX, chunkY);
        }
    }
}

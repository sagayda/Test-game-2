using Assets.Scripts.WorldGeneration.Core.Maps;
using UnityEngine;
using WorldGeneration.Core;

namespace Assets.Scripts.WorldGeneration.Core.Chunks
{
    public class Chunk : IMapArea
    {
        public static int Size = 32;
        public static Vector2 Pivot = new(0.5f, 0.5f);

#nullable enable


        public readonly RectInt Rect;
        public GenerationStage GenerationStage { get; private set; }
        public ValueMapPoint? Values { get; internal set; }
        public WaterMapPoint? Water { get; internal set; }
        public Vector2Int Position => Rect.position;
        public bool IsWaterChunk => Water != null;

        public Chunk(int x, int y)
        {
            Rect = new(x, y, Size, Size);

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

        internal bool TrySetGenerationStage(GenerationStage stage)
        {
            switch(stage)
            {
                case GenerationStage.Pre:
                    if (Values == null)
                        return false;
                    GenerationStage = stage;
                    return true;
                default:
                    return false;
            }
        }

        public static Vector2Int GlobalToLocalCoordinates(float x, float y)
        {
            int chunkX = Mathf.FloorToInt(x / Size);
            int chunkY = Mathf.FloorToInt(y / Size);

            return new Vector2Int(chunkX, chunkY);
        }

        public static Vector2Int GlobalToLocalCoordinates(int x, int y)
        {
            int chunkX = Mathf.FloorToInt((float)x / Size);
            int chunkY = Mathf.FloorToInt((float)y / Size);


            return new Vector2Int(chunkX, chunkY);
        }

        public static Vector2Int GlobalToLocalCoordinates(Vector2 coordinates)
        {
            int chunkX = Mathf.FloorToInt(coordinates.x / Size);
            int chunkY = Mathf.FloorToInt(coordinates.y / Size);

            return new Vector2Int(chunkX, chunkY);
        }

        public static Vector2Int GlobalToLocalCoordinates(Vector2Int coordinates)
        {
            int chunkX = Mathf.FloorToInt((float)coordinates.x / Size);
            int chunkY = Mathf.FloorToInt((float)coordinates.y / Size);

            return new Vector2Int(chunkX, chunkY);
        }

        public static Vector2 LocalToGlobalCoordinates(Vector2Int coordinates)
        {
            return new(coordinates.x * Size + (Size * Pivot.x), coordinates.y * Size + (Size * Pivot.y));
        }

        /// <summary>
        /// Returns global coordinates of realtive point in chunk
        /// </summary>
        /// <param name="coordinates">Local chunk coordinates</param>
        /// <param name="realtivePosition">Position of point in chunk.
        /// For example, (0,1) will return the coordinates of the bottom left corner of the chunk.</param>
        /// <returns></returns>
        public static Vector2 LocalToGlobalCoordinates(Vector2Int coordinates, Vector2 realtivePosition)
        {
            return new(coordinates.x * Size + (Size * realtivePosition.x), coordinates.y * Size + (Size * realtivePosition.y));
        }
    }
}

using System;
using System.Collections.Generic;
using Assets.Scripts.WorldGeneration.Core;
using Assets.Scripts.WorldGeneration.Core.Chunks;
using NaughtyAttributes;
using UnityEngine;
using WorldGeneration.Core;
using WorldGeneration.Core.Maps;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts.WorldGeneration.Editor
{
    public class WorldGeneratorTest : MonoBehaviour
    {
        private WorldGenerator _worldGenerator;
        private World _world;
        private MapVisualiser _mapVisualiser;
        private MapVisualiser _chunkVisualiser;

        #region Editor fields
        public string Seed = string.Empty;
        public int Width = 256;
        [Range(4, 256)]
        public int ChunkWidth = 32;
        public int Height = 256;
        [Range(4, 256)]
        public int ChunkHeight = 32;
        [Range(0, 1)]
        public float WaterLevel = 0.4f;

        [BoxGroup("Parameters save slots")]
        public ParametersSave.SaveSlot HeightSaveSlot;
        [BoxGroup("Parameters save slots")]
        public ParametersSave.SaveSlot TemperatureSaveSlot;
        [BoxGroup("Parameters save slots")]
        public ParametersSave.SaveSlot PolutionSaveSlot;
        [BoxGroup("Parameters save slots")]
        public ParametersSave.SaveSlot ProgressSaveSlot;

        [BoxGroup("Renderers")]
        [ShowAssetPreview(2048, 2048)]
        public Sprite HeightRenderer;
        [BoxGroup("Renderers")]
        [ShowAssetPreview(2048, 2048)]
        public Sprite HeightChunkRenderer;
        #endregion

        [Button("Create generator")]
        public void CreateGenerator()
        {
            HeightsMapParameters? heightsMapParameters = ParametersSave.LoadParameters<HeightsMapParameters>(HeightSaveSlot);
            if (heightsMapParameters.HasValue == false)
            {
                Debug.Log("HeParams loading failed");
                return;
            }

            HeightsValueMap heights = new(heightsMapParameters.Value);

            TemperatureMapParameters? temperatureMapParameters = ParametersSave.LoadParameters<TemperatureMapParameters>(TemperatureSaveSlot);
            if (temperatureMapParameters.HasValue == false)
            {
                Debug.Log("TeParams loading failed");
                return;
            }

            TemperatureValueMap temperature = new(temperatureMapParameters.Value);

            PolutionMapParameters? polutionMapParameters = ParametersSave.LoadParameters<PolutionMapParameters>(PolutionSaveSlot);
            if (polutionMapParameters.HasValue == false)
            {
                Debug.Log("PoParams loading failed");
                return;
            }

            PolutionValueMap polution = new(polutionMapParameters.Value);

            ProgressMapParameters? progressMapParameters = ParametersSave.LoadParameters<ProgressMapParameters>(ProgressSaveSlot);
            if (progressMapParameters.HasValue == false)
            {
                Debug.Log("PrParams loading failed");
                return;
            }

            Chunk.ChunkWidth = ChunkWidth;
            Chunk.ChunkHeight = ChunkHeight;

            ProgressValueMap progress = new(progressMapParameters.Value);

            CompositeValueMap compositeMap = new(heights, temperature, polution, progress);

            _worldGenerator = new(Seed, Width, Height, WaterLevel, compositeMap);

            _mapVisualiser = new(Width, Height);
        }

        [Button("Create world")]
        public void CreateEmptyWorld()
        {
            _world = new(_worldGenerator, "_", "_");
        }

        [Button("Init world chunks")]
        public void InitWorldChunks()
        {
            _world.InitChunks();

            _chunkVisualiser = new(_worldGenerator.Width / Chunk.ChunkWidth, _worldGenerator.Height / Chunk.ChunkHeight, ChunkWidth);
        }

        [Button("Pre-generate all world")]
        public void PreGenerateAllWorldChunks()
        {
            _world.GenerateAllChunksToStage(Core.Chunks.GenerationStage.Pre);
        }

        [Button("Create ocean")]
        public void CreateOcean()
        {
            _worldGenerator.WaterBehavior.CreateOcean(_world);
        }

        [Button("Paint heights")]
        public void PaintHeights()
        {
            float GetHeight(int x, int y) => _worldGenerator.GetMapValue(new(x, y), MapValueType.Height);

            float landSize = 1 - WaterLevel;
            float landStep = landSize / 3;

            List<Color> colors = new()
            {
                Color.black, //
                new Color(0f, 0.4f, 1f),
                new Color(0.9f, 0.9f, 0.1f),
                new Color(0.9f, 0.9f, 0.1f),
                new Color(0.5f, 0.95f, 0f),
                new Color(0.55f, 0.55f, 0),
                new Color(0.45f, 0f, 0f) //
            };

            List<float> maximas = new()
            {
                0,
                WaterLevel,
                WaterLevel,
                WaterLevel + landStep / 6,
                WaterLevel + landStep,
                WaterLevel + (landStep * 2),
                1
            };

            ColorMap colorMap = new(maximas, colors);

            HeightRenderer = _mapVisualiser.Paint(GetHeight, colorMap);
        }

        [Button("Paint chunk heights")]
        public void PaintChunkHeights()
        {
            float GetHeight(int x, int y) => _world.GetChunk(x * ChunkWidth, y * ChunkHeight).Values[MapValueType.Height];

            float landSize = 1 - WaterLevel;
            float landStep = landSize / 3;

            List<Color> colors = new()
            {
                Color.black,
                new Color(0f, 0.4f, 1f),
                new Color(0.5f, 0.95f, 0f),
                new Color(0.5f, 0.95f, 0f),
                new Color(0.55f, 0.55f, 0),
                new Color(0.45f, 0f, 0f)
            };

            List<float> maximas = new()
            {
                0,
                WaterLevel,
                WaterLevel + landStep / 4,
                WaterLevel + landStep,
                WaterLevel + (landStep * 2),
                1
            };

            ColorMap colorMap = new(maximas, colors);

            HeightChunkRenderer = _chunkVisualiser.Paint(GetHeight, colorMap);
        }

        [Button("Paint chunk ocean")]
        public void AddOceanToPaintedMap()
        {
            bool IsOcean(int x, int y) => _world.Ocean.IncludedArea.Exists((segment) => segment.Position.x == x * ChunkWidth && segment.Position.y == y * ChunkHeight);

            HeightChunkRenderer = _chunkVisualiser.Overpaint(IsOcean, Color.magenta);
        }

        [Button("Test")]
        public void Test()
        {
            int x = 1;
            int y = -23;

            float xf = 1.8f;
            float yf = -23.1f;

            Debug.Log(Chunk.GetChunkCoorinates(xf, yf));
            Debug.Log(Chunk.GetChunkCoorinates(new Vector2(xf, yf)));
            Debug.Log(Chunk.GetChunkCoorinates(x, y));
            Debug.Log(Chunk.GetChunkCoorinates(new Vector2Int(x, y)));
        }
    }
}

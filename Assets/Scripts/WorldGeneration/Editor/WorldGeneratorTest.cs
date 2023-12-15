using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.WorldGeneration.Core;
using Assets.Scripts.WorldGeneration.Core.Chunks;
using NaughtyAttributes;
using Newtonsoft.Json.Bson;
using Unity.Burst;
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

        [Button("Init world")]
        public void CreateEmptyWorld()
        {
            _worldGenerator.InitWorld("_", "_");

            _world = _worldGenerator.World;

            _chunkVisualiser = new(_worldGenerator.Width / Chunk.ChunkWidth, _worldGenerator.Height / Chunk.ChunkHeight, ChunkWidth);
        }

        [Button("Pre-generate all world")]
        public void PreGenerateAllWorldChunks()
        {
            _worldGenerator.InitWorldMapValues();
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
            float GetHeight(int x, int y) => _world.GetChunkByGlobalCoordinates(x * ChunkWidth, y * ChunkHeight).Values[MapValueType.Height];

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

        [Button("Paint river chunks")]
        public void AddRiversToPaintedMap()
        {
            bool IsRiver(int x, int y)
            {
                Vector2 chunkCoords = Chunk.WorldToLocalCoordinates(x * ChunkWidth, y * ChunkHeight);

                foreach (var item in _worldGenerator.WaterBehavior.Rivers.First().Chunks)
                    if(item.Position.x == chunkCoords.x && item.Position.y == chunkCoords.y)
                        return true;

                return false;
            }

            HeightChunkRenderer = _chunkVisualiser.Overpaint(IsRiver, Color.cyan);
        }

        [Button("Test")]
        public void Test()
        {
            Chunk source = _world.GetChunkByGlobalCoordinates(56, 40);

            _worldGenerator.WaterBehavior.CreateSource(source, 1);

            _worldGenerator.WaterBehavior.CreateRiver(source, _world);
        }
    }
}

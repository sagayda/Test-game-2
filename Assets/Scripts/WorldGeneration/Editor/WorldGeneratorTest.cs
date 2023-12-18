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
using WorldGeneration.Core.WaterBehavior;
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
        public int Height = 256;
        [Range(4, 256)]
        public int ChunkSize = 32;
        public Vector2 ChunkPivot = new(0.5f, 0.5f);
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
        public bool ShowGenerationSteps = false;
        #endregion

        private void Awake()
        {
            HeightChunkRenderer = null;
            HeightRenderer = null;
        }

        private void OnValidate()
        {
            if(HeightChunkRenderer != null && _chunkVisualiser != null)
            {
                _chunkVisualiser.ClearOverpaint();
                HeightChunkRenderer = _chunkVisualiser.Overpaint(new Vector2Int[] { SourceCoords }, Color.red);
            }
        }

        [ShowIf("ShowGenerationSteps")]
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

            Chunk.Size = ChunkSize;

            ProgressValueMap progress = new(progressMapParameters.Value);

            CompositeValueMap compositeMap = new(heights, temperature, polution, progress);

            _worldGenerator = new(Seed, Width, Height, WaterLevel, compositeMap);

            _mapVisualiser = new(Width, Height);
        }

        [ShowIf("ShowGenerationSteps")]
        [Button("Init world")]
        public void CreateEmptyWorld()
        {
            _worldGenerator.InitWorld("_", "_");

            _world = _worldGenerator.World;

            _chunkVisualiser = new(_worldGenerator.Width / Chunk.Size, _worldGenerator.Height / Chunk.Size, ChunkSize);
        }

        [ShowIf("ShowGenerationSteps")]
        [Button("Pre-generate all world")]
        public void PreGenerateAllWorldChunks()
        {
            _worldGenerator.InitWorldMapValues();
        }

        [ShowIf("ShowGenerationSteps")]
        [Button("Create ocean")]
        public void CreateOcean()
        {
            _worldGenerator.WaterBehavior.CreateOcean(_world);
        }

        [HideIf("ShowGenerationSteps")]
        [Button("Generate")]
        public void Generate()
        {
            CreateGenerator();
            CreateEmptyWorld();
            PreGenerateAllWorldChunks();
            CreateOcean();
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
            float GetHeight(int x, int y) => _world.GetChunkByLocalCoordinates(new(x, y)).Values[MapValueType.Height];

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
            bool IsOcean(int x, int y) => _world.Ocean.IncludedArea.Exists((segment) => segment.Position.x == x && segment.Position.y == y);

            HeightChunkRenderer = _chunkVisualiser.Overpaint(IsOcean, Color.magenta);
        }

        [Button("Paint river chunks")]
        public void AddRiversToPaintedMap()
        {
            foreach (var river in _worldGenerator.WaterBehavior.Rivers)
            {
                bool IsRiver(int x, int y)
                {
                    Vector2 chunkCoords = new(x, y);

                    foreach (var item in river.Chunks)
                        if (item.Position.x == chunkCoords.x && item.Position.y == chunkCoords.y)
                            return true;

                    return false;
                }

                _chunkVisualiser.Overpaint(IsRiver, Color.cyan);
            }

            HeightChunkRenderer = _chunkVisualiser.OverpaintedSprite;
        }

        [Button("Paint pools chunks")]
        public void AddPoolsToPaintedMap()
        {
            foreach (var pool in _worldGenerator.WaterBehavior.Pools)
            {
                bool isPool(int x, int y)
                {
                    Vector2 chunkCoords = new(x, y);

                    foreach (var item in pool.IncludedArea)
                        if (item.Position.x == chunkCoords.x && item.Position.y == chunkCoords.y)
                            return true;

                    return false;
                }

                _chunkVisualiser.Overpaint(isPool, Color.magenta);
            }

            HeightChunkRenderer = _chunkVisualiser.OverpaintedSprite;
        }

        [BoxGroup("Rivers")]
        public Vector2Int SourceCoords;

        [Button("Test")]
        public void Test()
        {
            Chunk source = _world.GetChunkByLocalCoordinates(SourceCoords);

            _worldGenerator.WaterBehavior.CreateSource(source, 1);

            _worldGenerator.WaterBehavior.CreateRiver(source, _world);
        }

        [Button("Test2")]
        public void Test2()
        {
            IEnumerable<Vector2Int> leakages;

            _worldGenerator.WaterBehavior.Flood(SourceCoords, _world, out leakages);

            _chunkVisualiser.Overpaint(leakages.ToArray(), Color.black);
        }

        [Button("Clear pool and rivers")]
        public void ClearPoolsAndRivers()
        {
            _worldGenerator.WaterBehavior.ClearPools();
            _worldGenerator.WaterBehavior.ClearRivers();
        }

    }
}

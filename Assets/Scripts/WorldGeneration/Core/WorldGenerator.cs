using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using WorldGeneration.Core;
using WorldGeneration.Core.Chunks;
using WorldGeneration.Core.WaterBehavior;
using UnityEngine;
using WorldGeneration.Core.Locations;
using WorldGeneration.Core.Maps;

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

				ValueMapPoint[] chunkPoints =
				{ 
					//center
					_compositeMap.ComputeValues(Chunk.LocalToGlobalCoordinates(chunk.Value.Position, new(0.5f,0.5f))),
					//top left
					_compositeMap.ComputeValues(Chunk.LocalToGlobalCoordinates(chunk.Value.Position, new(0.25f,0.25f))),
					//top right
					_compositeMap.ComputeValues(Chunk.LocalToGlobalCoordinates(chunk.Value.Position, new(0.75f,0.25f))),
					//bottom left
					_compositeMap.ComputeValues(Chunk.LocalToGlobalCoordinates(chunk.Value.Position, new(0.25f,0.75f))),
					//bottom right
					_compositeMap.ComputeValues(Chunk.LocalToGlobalCoordinates(chunk.Value.Position, new(0.75f,0.75f))),
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

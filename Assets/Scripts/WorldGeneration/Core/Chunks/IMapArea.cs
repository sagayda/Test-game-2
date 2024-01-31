using UnityEngine;
using WorldGeneration.Core.Maps;

namespace WorldGeneration.Core.Chunks
{
	public interface IMapArea
	{
		public Vector2Int Position { get; }
#nullable enable
		public ValueMapPoint? Values { get; }
		public WaterMapPoint? Water { get; }
		public bool HasWater { get; }
	}
}

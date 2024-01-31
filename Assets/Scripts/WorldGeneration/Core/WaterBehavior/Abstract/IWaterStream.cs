using WorldGeneration.Core.Chunks;

namespace WorldGeneration.Core.WaterBehavior.Abstract
{
	public interface IWaterStream : IWaterStructure
	{
		public Chunk Source { get; }
		public Chunk Leakage { get; }
		public bool HasLeakage {get; }
		public float Strength {get; }
	}
}

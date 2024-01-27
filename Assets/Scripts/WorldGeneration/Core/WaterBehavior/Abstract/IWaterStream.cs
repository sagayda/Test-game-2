using Assets.Scripts.WorldGeneration.Core.Chunks;

namespace Assets.Scripts.WorldGeneration.Core.WaterBehavior.Abstract
{
	public interface IWaterStream
	{
		public Chunk Source { get; }
		public Chunk Leakage { get; }
		public bool HasLeakage {get; }
		public float Strength {get; }
	}
}

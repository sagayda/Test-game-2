using System.Collections.Generic;
using WorldGeneration.Core.Chunks;

namespace WorldGeneration.Core.WaterBehavior.Abstract
{
	public interface IWaterStructure
	{
		public List<IMapArea> IncludedArea { get; }
		public bool HasTributary { get; }
		public bool HasOutlet { get; }
		
	}
}
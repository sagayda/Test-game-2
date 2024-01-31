using System.Collections.Generic;

namespace WorldGeneration.Core.WaterBehavior.Abstract
{
	public interface IOutflowWaterDistributor : IWaterDistributor
	{
		public IEnumerable<IInflowWaterDistributor> Inflows { get; }
	}
}
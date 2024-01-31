using System.Collections.Generic;

namespace WorldGeneration.Core.WaterBehavior.Abstract
{
	public interface IInflowWaterDistributor : IWaterDistributor
	{
		public IEnumerable<IOutflowWaterDistributor> Outflows {get;}
		
		public float RequestInflowFor(IOutflowWaterDistributor outflow);
		
		public void InformAboutOutflow(IOutflowWaterDistributor outflow);
	}
}
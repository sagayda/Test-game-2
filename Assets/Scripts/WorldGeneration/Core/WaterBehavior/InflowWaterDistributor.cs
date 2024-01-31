using System;
using System.Collections.Generic;
using System.Linq;
using WorldGeneration.Core.WaterBehavior.Abstract;

namespace WorldGeneration.Core.WaterBehavior
{
	public class InflowWaterDistributor : IInflowWaterDistributor
	{
		private readonly List<WaterSource> _sources;
		private readonly List<IOutflowWaterDistributor> _outflows;
		
		public InflowWaterDistributor(WaterSource baseSource)
		{
			_sources = new();
			_outflows = new();
				
			_sources.Add(baseSource);
		}
		
		public IEnumerable<IOutflowWaterDistributor> Outflows => _outflows;
		
		public float RequestInflowFor(IOutflowWaterDistributor outflow)
		{
			if(_outflows.Contains(outflow) == false)
				return 0;
			
			//need prioritising
			float totalInflow = 0;
			foreach(var source in _sources)
			{
				totalInflow += source.Strength; 
			}
			
			return totalInflow / _outflows.Count;
		}
		
		public void InformAboutOutflow(IOutflowWaterDistributor outflow)
		{
			if(outflow.Inflows.Contains(this) == false)
				throw new ArgumentException("Can't notify about outflow: this inflow is not set as inflow!");
			
			_outflows.Add(outflow);
		}
	}
}
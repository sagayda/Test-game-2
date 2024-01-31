using System;
using System.Collections.Generic;
using System.Linq;
using WorldGeneration.Core.WaterBehavior.Abstract;

namespace WorldGeneration.Core.WaterBehavior
{
	public class CombinedWaterDistributor : IInflowWaterDistributor, IOutflowWaterDistributor
	{
		private readonly List<IInflowWaterDistributor> _inflows;
		private readonly List<IOutflowWaterDistributor> _outflows;
		
		public CombinedWaterDistributor(IInflowWaterDistributor baseInflow)
		{
			_inflows = new();
			_outflows = new();
			
			_inflows.Add(baseInflow);
			
			baseInflow.InformAboutOutflow(this);
		}
		
		public CombinedWaterDistributor(params IInflowWaterDistributor[] inflows)
		{
			_inflows = new();
			_outflows = new();
			
			_inflows.AddRange(inflows);
			
			foreach(var inflow in _inflows)
			{
				inflow.InformAboutOutflow(this);
			}
		}
		
		public IEnumerable<IInflowWaterDistributor> Inflows => _inflows;
		public IEnumerable<IOutflowWaterDistributor> Outflows => _outflows;
		
		public float RequestInflowFor(IOutflowWaterDistributor outflow)
		{
			if(_outflows.Contains(outflow) == false)
				return 0;
			
			//needs prioritising
			float totalInflow = 0;
			
			foreach(var inflow in _inflows)
			{
				totalInflow += inflow.RequestInflowFor(this);
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
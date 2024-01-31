
using System.Collections.Generic;
using WorldGeneration.Core.WaterBehavior.Abstract;

namespace WorldGeneration.Core.WaterBehavior
{
	public class OutflowWaterDistributor : IOutflowWaterDistributor
	{
		private readonly List<IInflowWaterDistributor> _inflows;
		private float _passiveOutflow;
		
		public OutflowWaterDistributor(float passiveOutflow, IInflowWaterDistributor baseInflow)
		{
			_inflows = new();
			_passiveOutflow = passiveOutflow;
			
			_inflows.Add(baseInflow);
			
			baseInflow.InformAboutOutflow(this);
		}
		
		public OutflowWaterDistributor(float passiveOutflow, params IInflowWaterDistributor[] inflows)
		{
			_inflows = new();
			_passiveOutflow = passiveOutflow;
			
			_inflows.AddRange(inflows);
			
			foreach(var inflow in _inflows)
			{
				inflow.InformAboutOutflow(this);
			}
		}
				
		public IEnumerable<IInflowWaterDistributor> Inflows => _inflows;
		public float PassiveOutflow => _passiveOutflow;
		
		public void SetPassiveOutflow(float value)
		{
			_passiveOutflow = value;
		}
		
		public float GetPassiveOutflow()
		{
			return _passiveOutflow;
		}
		
		public bool IsPassiveOutflowSatisfied()
		{
			float totalInflow = 0;
			
			foreach(var inflow in _inflows)
			{
				totalInflow += inflow.RequestInflowFor(this);				
			}
			
			return _passiveOutflow <= totalInflow;
		}
		
		
	}
}
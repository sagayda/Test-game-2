using System;
using System.Collections.Generic;
using WorldGeneration.Core.Chunks;
using WorldGeneration.Core.WaterBehavior.Abstract;

namespace WorldGeneration.Core.WaterBehavior
{
	public class WaterSource : IWaterStructure
	{
		private readonly Cell _cell;
		public float Strength { get; private set; }

		public WaterSource(float strength)
		{
			Strength = strength;
		}
		
		public List<IMapArea> IncludedArea => new() {_cell};
		public bool HasTributary => false;
		public bool HasOutlet => throw new NotImplementedException();
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using WorldGeneration.Core.Chunks;
using WorldGeneration.Core.WaterBehavior.Abstract;

namespace WorldGeneration.Core.WaterBehavior
{
	public class Pool : IWaterPool
	{
		private const float EVAPORATING_STRENGTH = 0.02f;

		// private readonly List<IMapArea> _includedArea;
		private readonly HashSet<IMapArea> _includedArea;
		private readonly List<IMapArea> _leakages;
		
		private readonly List<IWaterStream> _tributaries;
		private readonly List<IWaterStream> _outlets;
		
		private readonly Dictionary<IInflowWaterDistributor, IMapArea> _inflowDistributors;
		private readonly Dictionary<IOutflowWaterDistributor, IMapArea> _outflowDistibutors;
		private readonly OutflowWaterDistributor _evaporatingOutflow;

		public Pool()
		{
			_includedArea = new();
			_leakages = new();
			
			_tributaries = new();
			_outlets = new();
			
		}
		
		public Pool(IInflowWaterDistributor inflowDistributor, IMapArea inflowArea) //add evaporating outflow
		{
			_includedArea = new();
			
			_inflowDistributors = new();
			_outflowDistibutors = new();
			
			_inflowDistributors.Add(inflowDistributor, inflowArea);
			_evaporatingOutflow = new(0, inflowDistributor);
			
			_includedArea = new();
		}
		
		public List<IMapArea> IncludedArea => _includedArea.ToList();
		public IEnumerable<IMapArea> Leakages => _leakages;
		public float EvaporatingVolume => CalculateEvaporatingVolume(_includedArea.Count);
		
		public bool HasTributary => _tributaries.Count != 0;
		public bool HasOutlet => _outlets.Count != 0;
		
		public IEnumerable<IWaterStream> Tributaries => _tributaries;
		public IEnumerable<IWaterStream> Outlets => _outlets;
		
		public IInflowWaterDistributor BaseInflow => _inflowDistributors.First().Key;
		public bool IsEvaporatingSatisfied => _evaporatingOutflow.IsPassiveOutflowSatisfied();
		
		public float Volume { get; private set; }
		
		public bool TryAddSegment(params IMapArea[] areas)
		{
			throw new NotImplementedException("TryAddSegment is outdate");
			
			// foreach (var item in areas)
			// 	if(item.HasWater == false)
			// 		return false;

			// _includedArea.AddRange(areas);

			// return true;
		}
		
		public void AddArea(params IMapArea[] areas)
		{
			foreach(var area in areas)
			{
				_includedArea.Add(area);
			}
			
			_evaporatingOutflow.SetPassiveOutflow(_includedArea.Count * EVAPORATING_STRENGTH);
		}

		// public void AddOutflow(IOutflowWaterDistributor outflow, IMapArea distributorArea)
		// {
		// 	if(Contains(distributorArea) == false)
		// 		throw new ArgumentException($"Current pool doesn't include specified area {distributorArea.Position} to create outflow!");
			
		// 	_outflowDistibutors.Add(outflow,distributorArea);
		// }
		public CombinedWaterDistributor CreateOutflowAt(IMapArea area)
		{
			if(Contains(area) == false)
				throw new ArgumentException($"Current pool doesn't include specified area {area.Position} to create outflow!");
			
			CombinedWaterDistributor outflow = new(_inflowDistributors.Keys.ToArray());
			
			_outflowDistibutors.Add(outflow, area);
			
			return outflow;
		}
		
		
		//need neighbour check
		public bool TryAddOutlet(IWaterStream waterStream)
		{
			if(_includedArea.Contains(waterStream.Source) == false)
				return false;
			
			_leakages.Add(waterStream.Source);
			_outlets.Add(waterStream);
			return true;
		}
		
		public bool TryAddTributary(IWaterStream waterStream)
		{
			if(_includedArea.Contains(waterStream.Leakage) == false)
				return false;
			
			_tributaries.Add(waterStream);
			return true;
		}
		
		public void SetVolume(float volume)
		{
			Volume = volume;
		}
		
		public void RemoveLeakage(IMapArea leakage)
		{
			_leakages.Remove(leakage);
		}
		
		public float GetExcessWater()
		{
			float excessWater = 0;
			
			foreach(var tributary in Tributaries)
			{
				excessWater += tributary.Strength;
			}
			
			foreach(var outlet in Outlets)
			{
				excessWater -= outlet.Strength;
			}
			
			excessWater -= EvaporatingVolume;
			
			return excessWater;
		}

		public bool Contains(IMapArea area)
		{
			return _includedArea.Contains(area);		
			
			// foreach (var item in _includedArea)
			// 	if(item.Position == area.Position)
			// 		return true;
			
			// return false;
		}
		
		public static float CalculateEvaporatingVolume (int cellsCount)
		{
			return cellsCount * EVAPORATING_STRENGTH;
		}
		
	}
}

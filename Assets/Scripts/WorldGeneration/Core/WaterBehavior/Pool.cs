using System.Collections.Generic;
using Assets.Scripts.WorldGeneration.Core.Chunks;
using Assets.Scripts.WorldGeneration.Core.WaterBehavior.Abstract;

namespace WorldGeneration.Core.WaterBehavior
{
	public class Pool : IWaterPool
	{
		private const float EVAPORATING_STRENGTH = 0.02f;

		private readonly List<IMapArea> _includedArea;
		private readonly List<IMapArea> _leakages;
		
		private readonly List<IWaterStream> _tributaries;
		private readonly List<IWaterStream> _outlets;

		public Pool()
		{
			_includedArea = new();
			_leakages = new();
			
			_tributaries = new();
			_outlets = new();
		}
		
		public List<IMapArea> IncludedArea => _includedArea;
		public IEnumerable<IMapArea> Leakages => _leakages;
		public float EvaporatingVolume => CalculateEvaporatingVolume(_includedArea.Count);
		
		public IEnumerable<IWaterStream> Tributaries => _tributaries;
		public IEnumerable<IWaterStream> Outlets => _outlets;

		public float Volume { get; private set; }
		
		public bool TryAddSegment(params IMapArea[] areas)
		{
			foreach (var item in areas)
				if(item.HasWater == false)
					return false;

			_includedArea.AddRange(areas);

			return true;
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
			foreach (var item in _includedArea)
				if(item.Position == area.Position)
					return true;
			
			return false;
		}
		
		public static float CalculateEvaporatingVolume (int cellsCount)
		{
			return cellsCount * EVAPORATING_STRENGTH;
		}
	}
}

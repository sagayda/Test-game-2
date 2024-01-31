using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WorldGeneration.Core.Chunks;
using WorldGeneration.Core.WaterBehavior.Abstract;

namespace WorldGeneration.Core.WaterBehavior
{
	public class River : IWaterStream
	{
		//private readonly LinkedList<Chunk> _chunks; //???????
		private readonly LinkedList<IMapArea> _includedArea;
		
		private readonly Dictionary<IInflowWaterDistributor, IMapArea> _inflowDistributors;
		private readonly Dictionary<IOutflowWaterDistributor, IMapArea> _outflowDistibutors;
		
		public River(Chunk source)
		{
			if (source.HasWater == false)
				throw new ArgumentException("The chunk given as a river source has no water!", nameof(source));

			if (source.Water.IsSource == false)
				throw new ArgumentException("The chunk given as a river source is not source!", nameof(source));


			_includedArea = new();

			_includedArea.AddFirst(source);
		}
		
		public River(IInflowWaterDistributor source, IMapArea sorceChunk)
		{
			_inflowDistributors = new();
			_outflowDistibutors = new();
			
			_inflowDistributors.Add(source, sorceChunk);
			
			_includedArea = new();
			_includedArea.AddFirst(sorceChunk);
		}

		public River(Chunk source, IWaterPool poolSource)
		{
			if (source.HasWater == false)
				throw new ArgumentException("The chunk given as a river source has no water!", nameof(source));

			if (source.Water.IsSource == false)
				throw new ArgumentException("The chunk given as a river source is not source!", nameof(source));


			_includedArea = new();

			_includedArea.AddFirst(source);
			PoolSource = poolSource;
		}

		public IWaterPool PoolSource {get; private set;}

		public Chunk Source => _includedArea.First.Value as Chunk; //???
		public Chunk LastSegment => _includedArea.Last.Value as Chunk; //???
		public Chunk Leakage => HasLeakage ? _includedArea.Last.Value as Chunk : null; //???
		public float Strength => Source.Water.Source.Strength;
		public bool HasLeakage { get; private set; }
		
		public bool HasTributary => Source != null;
		public bool HasOutlet => HasLeakage;
		
		public List<IMapArea> IncludedArea => _includedArea.ToList();
		
		public bool TryAddSegment(Chunk chunk, bool markAsLeakage = false)
		{
			if (chunk.HasWater == false)
				return false;

			_includedArea.Last.Value.Water.Stream = chunk.Position - _includedArea.Last.Value.Position;

			_includedArea.AddLast(chunk);
			
			if(markAsLeakage)
				HasLeakage = true;
			
			return true;
		}

		public void CreateLeakage()
		{
			throw new NotImplementedException("CreateLeakage() is currently outdated");
			HasLeakage = true;
		}
		
		public CombinedWaterDistributor CreateOutlet()
		{
			HasLeakage = true; //????
			
			CombinedWaterDistributor outlet = new(_inflowDistributors.Keys.ToArray());
			_outflowDistibutors.Add(outlet, LastSegment);
			
			return outlet;
		}
		
		public void CreateVirtualOutlet()
		{
			HasLeakage = true; //????
			
			OutflowWaterDistributor outlet = new(float.MaxValue, _inflowDistributors.Keys.ToArray());
			_outflowDistibutors.Add(outlet, LastSegment);
		}

	}
}

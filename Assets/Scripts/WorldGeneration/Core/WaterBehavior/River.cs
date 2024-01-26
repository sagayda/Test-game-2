using System;
using System.Collections.Generic;
using Assets.Scripts.WorldGeneration.Core.Chunks;
using Assets.Scripts.WorldGeneration.Core.WaterBehavior.Abstract;

namespace WorldGeneration.Core.WaterBehavior
{
    public class River : IWaterStream
    {
        private readonly LinkedList<Chunk> _chunks;

        public River(Chunk source)
        {
            if (source.HasWater == false)
                throw new ArgumentException("The chunk given as a river source has no water!", nameof(source));

            if (source.Water.IsSource == false)
                throw new ArgumentException("The chunk given as a river source is not source!", nameof(source));


            _chunks = new();

            _chunks.AddFirst(source);
        }

        public Chunk Source => _chunks.First.Value;
        public Chunk LastSegment => _chunks.Last.Value;
        public Chunk Leakage => HasLeakage ? _chunks.Last.Value : null;
        public float Strength => Source.Water.Source.Strength;
        public bool HasLeakage { get; private set; }
        public IEnumerable<Chunk> Chunks => _chunks;

        public bool TryAddSegment(Chunk chunk, bool markAsLeakage = false)
        {
            if (chunk.HasWater == false)
                return false;

            _chunks.Last.Value.Water.Stream = chunk.Position - _chunks.Last.Value.Position;

            _chunks.AddLast(chunk);
            
            if(markAsLeakage)
                HasLeakage = true;
            
            return true;
        }

        public void CreateLeakage()
        {
            HasLeakage = true;
        }

    }
}

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

        public Pool()
        {
            _includedArea = new();
            _leakages = new();
        }

        public List<IMapArea> IncludedArea => _includedArea;
        public IEnumerable<IMapArea> Leakages => _leakages;
        public float EvaporatingVolume => _includedArea.Count * EVAPORATING_STRENGTH;

        public bool TryAddSegment(params IMapArea[] areas)
        {
            foreach (var item in areas)
                if (item.HasWater == false)
                    return false;

            _includedArea.AddRange(areas);

            return true;
        }

        //need neighbour check
        public bool TryAddLeakage(params IMapArea[] areas)
        {
            _leakages.AddRange(areas);
            return true;
        }

        public bool Contains(IMapArea area)
        {
            foreach (var item in _includedArea)
                if (item.Position == area.Position)
                    return true;

            return false;
        }
    }
}

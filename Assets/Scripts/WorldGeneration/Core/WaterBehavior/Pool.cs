using System.Collections.Generic;
using Assets.Scripts.WorldGeneration.Core.Chunks;
using Assets.Scripts.WorldGeneration.Core.WaterBehavior.Abstract;

namespace WorldGeneration.Core.WaterBehavior
{
    public class Pool : IWaterPool
    {
        private readonly List<IMapArea> _includedArea;

        public List<IMapArea> IncludedArea => _includedArea;

        public Pool(List<IMapArea> includedArea)
        {
            _includedArea = includedArea;
        }

        public bool Contains(IMapArea area)
        {
            foreach (var item in _includedArea)
                if(item.Position == area.Position)
                    return true;
            
            return false;
        }
    }
}

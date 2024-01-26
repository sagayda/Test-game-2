using System.Collections.Generic;
using Assets.Scripts.WorldGeneration.Core.Chunks;

namespace Assets.Scripts.WorldGeneration.Core.WaterBehavior.Abstract
{
    public interface IWaterPool
    {
        public List<IMapArea> IncludedArea { get; }
    }
}

using UnityEngine;

namespace WorldGeneration.Core.Maps
{
    public interface IValueMap
    {
        public int Seed { get; set; }
        public int Priority { get; }
        public MapValueType ValueType { get; }

        public ValueMapPoint ComputeValue(ValueMapPoint mapPoint, Vector2 position);
    }
}

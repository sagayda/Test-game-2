using UnityEngine;

namespace Map.Model
{
    public interface IDynamicMarkerData
    {
        public string Name { get; }
        public Sprite Sprite { get; }
    }
}
